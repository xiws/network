using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    public class Connection:IConnection
    {
        ISocket Socket { get; set; }
        public Action OnOpen { get ; set ; }
        public Action OnClose { get ; set ; }
        public Action<dataStruct> OnMessage { get ; set ; }
        public Action<byte[]> OnBinary { get ; set ; }
        public Action<byte[]> OnPing { get ; set ; }
        public Action<byte[]> OnPong { get ; set ; }
        public Action<Exception> OnError { get ; set ; }
        public Action<IConnection> _ac { get; set; }
        public Dictionary<string,object> parameter { get; set; }

        public bool IsAvailable => throw new NotImplementedException();

        const int ReadSize = 100;

        public Connection(ISocket socket,Action<IConnection> con)
        {
            parameter = new Dictionary<string, object>();
            this.Socket = socket;
            OnOpen = () => { };
            OnClose = () => { };
            OnMessage = x => { };
            OnBinary = x =>
            { };
            OnPing = x => SendPong(x);
            OnPong = x => { };
            OnError = x => { };
            Read();
            this._ac = con;
            this._ac(this);
            OnOpen();

        }

        /// <summary>
        /// 接受到一个数据后触发回调
        /// </summary>
        /// <param name="data"></param>
        /// <param name="buffer"></param>
        private void Read()
        {
            Socket.Receive(key =>
           {
              if (key.Data_Length <= 0)
                {
                   Close();
                  return;
               }
               OnMessage(key);
                Read();
            },
            HandleReadError);
        }

        private void HandleReadError(Exception e)
        {

            OnError(e);
            if (e is AggregateException)
            {
                var agg = e as AggregateException;
                HandleReadError(agg.InnerException);
                
                return;
            }
            Close();
        }

        public Task Send(string message)
        {
            var dl= Encoding.UTF8.GetBytes(message);
           return  Send(new dataStruct()
            {
                StreamType = StreamTypes.message,
                datalist = dl,
                FileName = null
            });
        }

        public Task Send(dataStruct message)
        {
           return this.Socket.Send(message,()=>
            {
                
            },e=>
            {
                Close();
            });
        }

        public Task SendPing(byte[] message)
        {
            throw new NotImplementedException();
        }

        public Task SendPong(byte[] message)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            OnClose();
            Socket.Close();
        }

        public object GetPaarmiter(string key)
        {
            if (key.Equals("Rip"))
                return Socket.RemoteIpAddress;
            if (key.Equals("ip")) return Socket.LocalEndPoint.ToString();
            if (parameter.ContainsKey(key))
                return parameter[key];
            return null;
        }
        public void Add(string key,Object data)
        {
            if (parameter.ContainsKey(key))
                parameter[key] = data;
            else parameter.Add(key, data);
        }
        public void remove(string key)
        {
            if (parameter.ContainsKey(key))
                parameter.Remove(key);
        }
    }
}
