using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    public class NetServerFactory
    {
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IConnection IConnectionFactory(string ip,int port,Action<IConnection> action)
        {
            var socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            ISocket socket = new SocketServer(socket2);
            socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            IConnection con = new Connection(socket, action);
            return con;
        }
    }
}
