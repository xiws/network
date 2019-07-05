using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    class Program
    {
        static void Main(string[] args)
        {

            NetServer server = new NetServer(4321);
            List<IConnection> list = new List<IConnection>();
            server.Start(key =>
            {
                key.OnMessage = s =>
                {
                    
                    Console.WriteLine("服务器接收到数据长度：" + s.Data_Length);
                    key.Send("服务器返回:ok");
                };
                key.OnOpen = () =>
                {
                    list.Add(key);
                    Console.WriteLine("open");
                };
                key.OnClose = () =>
                {
                    Console.WriteLine("close");
                };
            });
            test();
        }

        private static void C_reciveevent(dataStruct ds)
        {
            if (ds.StreamType == StreamTypes.message)
                Console.WriteLine(Encoding.Default.GetString(ds.datalist));
        }
        private static void test()
        {

            var con = NetServerFactory.IConnectionFactory("127.0.0.1", 4321, (key) =>
             {
                 key.OnOpen = () => { Console.WriteLine("sdf"); };
                 key.OnMessage = dt =>
                 {
                     Console.WriteLine(Encoding.UTF8.GetString(dt.datalist));
                 };
                 key.OnClose = () =>
                 {
                     Console.WriteLine("close");
                 };
             });
            while (true)
            {
                var line = Console.ReadLine();
                var dsdf = new dataStruct()
                {
                    datalist = Encoding.UTF8.GetBytes(line),
                    StreamType = StreamTypes.message
                };
                if (line.Equals("close"))
                {
                    con.Close();
                    continue;
                }
                con.Send(dsdf);
            }
        }

    }
}
