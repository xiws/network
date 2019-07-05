using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace network
{
    public class connect
    {
        public delegate void recive(dataStruct ds);
        public event recive reciveevent;
        public NetworkStream NetWorkStream { get; set; }
        public Socket socket { get; set; }
        List<Socket> lsocket { get; set; }
        public string IP { get; set; }
        /// <summary>
        /// 是否链接上
        /// </summary>
        public bool IsConnection { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// ip 列表
        /// </summary>
        public IPAddress[] Iplist { get { return Dns.GetHostAddresses(""); } }
        public connect()
        {
            Port = 555;
            IsConnection = false;
        }
        public connect(string ip,int port)
        {
            this.IP = ip;
            this.Port = port;
            IsConnection = false;

        }
        public connect(int port) 
            : this("", port)
        {
            socket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.IP);
            socket.Bind(new IPEndPoint(IPAddress.Any, this.Port));
            socket.Listen(100);
        }
        /// <summary>
        /// 监听
        /// </summary>
        public void Listen()
        {
            this._listen();
        }
        /// <summary>
        /// 链接
        /// </summary>
        public void Connection()
        {
            this._con();
        }
        /// <summary>
        /// 监听端口
        /// </summary>
        private void _listen()
        {
            Task.Run(() =>
            {
                var sockets= socket.Accept();
                this.IsConnection = true;
                lsocket.Add(sockets);
                _listen();
            });
        }
        private void _con()
        {
            socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.IP);
            socket.Connect(new IPEndPoint(IPAddress.Parse(this.IP), this.Port));
            IsConnection = true;
            //NetWorkStream = new NetworkStream(socket);
        }
        public void Send(dataStruct ds)
        {
            byte[] b = ds.GetStream();
            
            NetWorkStream.Write(b, 0, b.Length);


        }
        public async Task SendAsync(dataStruct ds)
        {
            byte[] b = ds.GetStream();
            await NetWorkStream.WriteAsync(b, 0, b.Length);
        }
        public dataStruct Recive(Action<dataStruct> action)
        {
            dataStruct ds = new dataStruct();

            var res=ds.GetDatastruct(NetWorkStream);
            //reciveevent(res);
            action(res);
            return res;
        }
        
    }
}
