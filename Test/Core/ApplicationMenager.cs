using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Test.Tool;

namespace Test.Core
{
    delegate void Filter(HttpContent context);
    delegate void Route();
    class ApplicationMenager:Dictionary<string, HttpContent>
    {
        static ApplicationMenager app = new ApplicationMenager();
        NetCore netCore;

        private ApplicationMenager()
        {
            netCore = new NetCore();
        }
        public static ApplicationMenager ApplicationMenagerFactory()
        {
            return app;
        }

        Filter pieleline { get; set; }

        public void AddFilter(Filter action)
        {
            pieleline += action;
        }


        public ApplicationMenager SetPart(int part)
        {
             this.netCore.setPart(part);
             return this;
        }
        public void start()
        {
            netCore.Bind();
            netCore.Listener(100);
            netCore.Accept(OnAccept);
        }

        private void OnAccept(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;
            Socket new_client = socket.EndAccept(ar);
            try
            {
                try
                {

                    socket.BeginAccept(new AsyncCallback(OnAccept), socket);
                    byte[] recv_buffer = new byte[1024 * 640];
                    int real_recv = new_client.Receive(recv_buffer);
                    string recv_request = Encoding.UTF8.GetString(recv_buffer, 0, real_recv);
                    Console.WriteLine("ret"+recv_request);
                    HttpContent httpcontent = HttpContent.BuilderHttpContent(recv_request, new_client);
                    if (httpcontent.session!=null&&!this.ContainsKey(httpcontent.session.SessionId))
                    {
                        this.Add(httpcontent.session.SessionId, httpcontent);
                    }
                    pieleline(httpcontent);
                    //HttpContent
                    socket.Shutdown(SocketShutdown.Send);
                    socket.Close();
                }
                catch (HttpError e)
                {
                    setError(new_client, e.Message);
                }

            }
            catch(Exception e)
            {
                setError(new_client, "500");
            }
        }

        private void setError(Socket ar, string e)
        {
            Socket response = ar;
            string statusline = "HTTP/1.1 " + e + "\r\n";   //状态行
            byte[] statusline_to_bytes = Encoding.UTF8.GetBytes(statusline);
            if (response.Connected == false) return;
            response.Send(statusline_to_bytes);  //发送状态行
            //response.Send("text/html".getBytes());  //发送应答头
            //response.Send(new byte[] { (byte)'\r', (byte)'\n' });  //发送空行
            //byte[] buffer = Encoding.UTF8.GetBytes(e.Message);
            //response.Send(buffer);
            
            response.Shutdown(SocketShutdown.Send);
            response.Close();
        }
        public void RemoveSession(string sessionid)
        {
            if (this.ContainsKey(sessionid))
            {
                this.Remove(sessionid);
            }
            
        }
    }
}
