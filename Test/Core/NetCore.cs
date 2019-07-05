using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core
{
    class NetCore
    {
        int defaultPart = 8090;
        IPEndPoint ippoint;
        static readonly Socket socket= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public NetCore(int port)
        {
            defaultPart = port;
            ippoint = new IPEndPoint(IPAddress.Any, defaultPart);
        }
        public void setPart(int part)
        {
            ippoint = new IPEndPoint(IPAddress.Any, part);
        }
        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="backlog">挂起的连接队列的最大长度。</param>
        public void Listener(int backlog)
        {
            if (backlog > 0x7fffffff || backlog < 0) throw new ArgumentOutOfRangeException("backlog 超出最大值");
            socket.Listen(backlog);
        }

        public void Accept(AsyncCallback action)
        {
             socket.BeginAccept(action, socket);
        }


        public NetCore() { }

        /// <summary>
        /// 绑定地址
        /// </summary>
        public void Bind()
        {
            socket.Bind(ippoint);
        }

    }
}
