using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    public class NetServer
    {
        private readonly IPAddress _locationIP;
        private Action<IConnection> _config;

        public ISocket ListenerSocket { get; set; }
        public string Location { get; private set; }
        public int Port { get; private set; }
        public bool RestartAfterListenError { get; set; }

        private IPAddress ParseIPAddress(string ip)
        {
            string ipStr = ip;

            if (ipStr == "0.0.0.0")
            {
                return IPAddress.Any;
            }
            else if (ipStr == "[0000:0000:0000:0000:0000:0000:0000:0000]")
            {
                return IPAddress.IPv6Any;
            }
            else
            {
                try
                {
                    return IPAddress.Parse(ipStr);
                }
                catch (Exception ex)
                {
                    throw new FormatException("Failed to parse the IP address part of the location. Please make sure you specify a valid IP address. Use 0.0.0.0 or [::] to listen on all interfaces.", ex);
                }
            }
        }

        public NetServer(int port)
        {
            Port = port;
            _locationIP = IPAddress.Any;
            var socket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.IP);
            ListenerSocket = new SocketServer(socket);
        }
        public NetServer(string ip,int port)
        {
            _locationIP = IPAddress.Parse(ip);
            Port = port;
            var socket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.IP);
            ListenerSocket = new SocketServer(socket);
        }
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="config"></param>
        public void Start(Action<IConnection> config)
        {
            var ipLocal = new IPEndPoint(IPAddress.Any, Port);
            ListenerSocket.Bind(ipLocal);
            ListenerSocket.Listen(100);
            Port = ((IPEndPoint)ListenerSocket.LocalEndPoint).Port;
            
            ListenForClients();
            byte[] buffer = new byte[245];
            _config = config;
        }

        public ISocket Connect(Action<IConnection> config)
        {
            var ipLocal = new IPEndPoint(_locationIP, Port);
            ListenerSocket.Connect(ipLocal);
            _config = config;
            return ListenerSocket;
        }
        /// <summary>
        /// 监听事件
        /// </summary>
        private void ListenForClients()
        {
            ListenerSocket.Accept(OnClientConnect, e => {
                
                if (RestartAfterListenError)
                {
                    try
                    {
                        ListenerSocket.Dispose();
                        var socket = new Socket(_locationIP.AddressFamily, SocketType.Stream, ProtocolType.IP);
                        ListenerSocket = new SocketServer(socket);
                        Start(_config);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            });
        }

        /// <summary>
        /// 当有用户连接上时触发的回调
        /// </summary>
        /// <param name="clientSocket"></param>
        private void OnClientConnect(ISocket clientSocket)
        {
            if (clientSocket == null) return; // socket closed

            //重新创建一个异步监听
            ListenForClients();

            Connection connection = null;
            ///把socket 保存到websocketconnection
            connection = new Connection(clientSocket,this._config);

        }
        
        
    }
}
