using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    public interface ISocket
    {
        bool Connected { get; }
        string RemoteIpAddress { get; }
        int RemotePort { get; }
        Stream Stream { get; }
        bool NoDelay { get; set; }
        EndPoint LocalEndPoint { get; }

        Task<ISocket> Accept(Action<ISocket> callback, Action<Exception> error);
        Task Send(dataStruct buffer, Action callback, Action<Exception> error);
        Task Receive( Action<dataStruct> callback, Action<Exception> error, int offset = 0);
        void Dispose();
        void Connect(EndPoint ipLocal);
        void Close();

        void Bind(EndPoint ipLocal);
        void Listen(int backlog);
    }
}
