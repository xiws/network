using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    public interface IConnection
    {
        Action OnOpen { get; set; }
        Action OnClose { get; set; }
        Action<dataStruct> OnMessage { get; set; }
        Action<byte[]> OnBinary { get; set; }
        Action<byte[]> OnPing { get; set; }
        Action<byte[]> OnPong { get; set; }
        //Action BindUser { get; set; }
        Action<Exception> OnError { get; set; }
        Task Send(string message);
        Task Send(dataStruct message);
        Task SendPing(byte[] message);
        Task SendPong(byte[] message);
        void Close();
        bool IsAvailable { get; }
        object GetPaarmiter(string key);
        void Add(string key, Object data);
        void remove(string key);
    }
}
