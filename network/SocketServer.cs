using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace network
{
    class SocketServer:ISocket
    {
        readonly Socket _socket;

        public const UInt32 KeepAliveInterval = 6000;//设置后开始发送心跳
        public const UInt32 RetryInterval = 1000;//发送心跳的间隔

        private NetworkStream _stream;
        private CancellationTokenSource _tokenSource;
        private TaskFactory _taskFactory;

        public string RemoteIpAddress
        {
            get
            {
                var endpoint = _socket.RemoteEndPoint as IPEndPoint;
                return endpoint != null ? endpoint.Address.ToString() : null;
            }
        }

        public int RemotePort
        {
            get
            {
                var endpoint = _socket.RemoteEndPoint as IPEndPoint;
                return endpoint != null ? endpoint.Port : -1;
            }
        }

        public bool Connected
        {
            get { return _socket.Connected; }
        }

        public Stream Stream
        {
            get { return _stream; }
        }

        public EndPoint LocalEndPoint
        {
            get { return _socket.LocalEndPoint; }
        }

        public bool NoDelay { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SocketServer(Socket socket)
        {
            ///Task 终止令牌
            _tokenSource = new CancellationTokenSource();
            ///创建task
            _taskFactory = new TaskFactory(_tokenSource.Token);
            _socket = socket;
            //如果连接为ture 创建一个网络流
            if (_socket.Connected)
                _stream = new NetworkStream(_socket);
            
            // The tcp keepalive default values on most systems
            // are huge (~7200s). Set them to something more reasonable.
            SetKeepAlive(socket, KeepAliveInterval, RetryInterval);
        }

        /// <summary>
        /// 设置心跳，判断客户端意外关闭连接
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="keepAliveInterval">接收数据包</param>
        /// <param name="retryInterval">发出数据包</param>
        public void SetKeepAlive(Socket socket, UInt32 keepAliveInterval, UInt32 retryInterval)
        {

            //每四个字节一个整数，第一个整数1表示启用，第二个整数0x1388表示设置以后过5000毫秒开始发送心跳，第三个整数0x1388表示每5000毫秒发送一次心跳。
            //byte[] inValue = new byte[] { 1, 0, 0, 0, 0x88, 0x13, 0, 0, 0x88, 0x13, 0, 0 };
            //在接受方接受连接或者发起方连接成功之后，对该连接的套接字设置iocontrol
            // socket.IOControl(IOControlCode.KeepAliveValues, inValue, null);


            int size = sizeof(UInt32);//保存3个数据，没个数据为sizeof(UInt32) 所以为3*sizeof(UInt32)
            UInt32 on = 1;

            byte[] inArray = new byte[size * 3];
            Array.Copy(BitConverter.GetBytes(on), 0, inArray, 0, size);
            Array.Copy(BitConverter.GetBytes(keepAliveInterval), 0, inArray, size, size);
            Array.Copy(BitConverter.GetBytes(retryInterval), 0, inArray, size * 2, size);
            ///设定低级作业模式socket使用数值控制码
            ///指定要执行作业的控制码，包含作业所需资料，包含作业传回的输出资料
            socket.IOControl(IOControlCode.KeepAliveValues, inArray, null);
        }

        public Task Receive(Action<dataStruct> callback, Action<Exception> error, int offset)
        {
            byte[] head = new byte[dataStruct.headLength];
            try
            {
                    Func<AsyncCallback, object, IAsyncResult> begin =
                   (cb, s) =>
                        _stream.BeginRead(head, 0, head.Length, cb, s);

                    dataStruct ds = null;
                    Task<dataStruct> task = Task.Factory.FromAsync<dataStruct>(begin, key =>
                    {
                        if (!_stream.CanRead) return null;
                        try
                        {
                            int n = _stream.EndRead(key);
                        }
                        catch
                        {
                            
                        }
                        ds = dataStruct.byteToDataStruct(head);
                        return ds;
                    }, ds);
                    task.ContinueWith<dataStruct>(key=>
                    {
                        if (!_stream.CanRead) return null;
                         dataStruct dss= key.Result;
                         _stream.ReadAsync(dss.datalist, 0, dss.datalist.Length);
                        return dss;
                    })
                    .ContinueWith(t => callback(t.Result), TaskContinuationOptions.NotOnFaulted)
                    .ContinueWith(t => error(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
                    
                    task.ContinueWith(t => error(t.Exception), TaskContinuationOptions.OnlyOnFaulted);

                    return task;
            }
            catch (Exception e)
            {
                error(e);
                return null;
            }
        }
        public void Error(Exception err)
        {
            Console.WriteLine(err.Message);
        }
        public Task ReadBytes(byte[] head, Action<int> callback, Action<Exception> error)
        {
            
            try
            {
                Func<AsyncCallback, object, IAsyncResult> begin =
               (cb, s) =>
                    _stream.BeginRead(head, 0, head.Length, cb, s);

                Task<int> task = Task.Factory.FromAsync<int>(begin, _stream.EndRead, null);
                task.ContinueWith(t => callback(t.Result), TaskContinuationOptions.NotOnFaulted)
                    .ContinueWith(t => error(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
                task.ContinueWith(t => error(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
                return task;
            }
            catch (Exception e)
            {
                error(e);
                return null;
            }
        }


        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="backlog">挂起连接的最大数量</param>
        public void Listen(int backlog)
        {
            this._socket.Listen(backlog);
        }

        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="callback"> 回调方法</param>
        /// <param name="error"></param>
        /// <returns></returns>
        public Task<ISocket> Accept(Action<ISocket> callback, Action<Exception> error)
        {
            ///定义回调
            Func<IAsyncResult, ISocket> end = r => _tokenSource.Token.IsCancellationRequested ? null : new SocketServer(_socket.EndAccept(r));
            //用于启动异步操作的委托。 用于结束异步操作的委托。一个包含由 beginMethod 委托使用的数据的对象。TaskCreationOptions 值，用于控制创建的 Task 的行为。
            var task = _taskFactory.FromAsync(_socket.BeginAccept, end, null);

            task.ContinueWith(t => callback(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion)
                    .ContinueWith(t => error(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => error(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            if (_stream != null) _stream.Dispose();
            if (_socket != null) _socket.Dispose();
        }

        public void Close()
        {
            _tokenSource.Cancel();
            if (_stream != null) _stream.Close();
            if (_socket != null) _socket.Close();
        }

        public int EndSend(IAsyncResult asyncResult)
        {
            _stream.EndWrite(asyncResult);
            return 0;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="send"></param>
        /// <param name="callback"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public Task Send(dataStruct send, Action callback, Action<Exception> error)
        {
            var buffer = dataStruct.DataStructToByte(send);

            if (_tokenSource.IsCancellationRequested)
                return null;
            try
            {
                Func<AsyncCallback, object, IAsyncResult> begin =
                    (cb, s) => _stream.BeginWrite(buffer, 0, buffer.Length, cb, s);
                Task task = Task.Factory.FromAsync(begin, _stream.EndWrite, null);
                task.ContinueWith(t => callback(), TaskContinuationOptions.NotOnFaulted)
                    .ContinueWith(t => error(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
                task.ContinueWith(t => error(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
                return task;
            }
            catch (Exception e)
            {
                error(e);
                return null;
            }
        }

        /// <summary>
        /// 绑定ip
        /// </summary>
        /// <param name="ipLocal"></param>
        public void Bind(EndPoint ipLocal)
        {
            this._socket.Bind(ipLocal);
        }
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="IPlocal"></param>
        public void Connect(EndPoint IPlocal)
        {
            this._socket.Connect(IPlocal);
            if (this._socket.Connected) this._stream = new NetworkStream(this._socket);
        }
    }
}

