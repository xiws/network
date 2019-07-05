using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Test.Tool;

namespace Test.Core
{
    public class HttpResponse
    {
        /// <summary>
        /// 返回报文
        /// </summary>
        public Dictionary<string, string> ResponseHead { get; set; }
        /// <summary>
        /// response
        /// </summary>
        Socket response { get; set; }

        public HttpResponse(Socket so)
        {
            this.response = so;
            ResponseHead = new Dictionary<string, string>();
        }

        public void binayWrite(byte[] buffer)
        {
            string statusline = "HTTP/1.1 200 OK\r\n";   //状态行
            byte[] statusline_to_bytes = Encoding.UTF8.GetBytes(statusline);
            StringBuilder tmp = new StringBuilder();
            ResponseHead.Add("Content-Type", "text/html;charset=UTF-8");
            ResponseHead.Add("Content-Length", buffer.Length.ToString());
            foreach (var key in ResponseHead)
                tmp.AppendFormat("{0}:{1}\r\n", key.Key, key.Value);
            response.Send(statusline_to_bytes);  //发送状态行
            response.Send(tmp.ToString().getBytes());  //发送应答头
            response.Send(new byte[] { (byte)'\r', (byte)'\n' });  //发送空行
            response.Send(buffer);  //发送正文（html）
            response.Close();
        }

        public void Write(String str)
        {
            binayWrite(str.getBytes());
        }
        public void WriteError(float error,string msg)
        {
            string statusline = "HTTP/1.1 "+error+"\r\n";   //状态行
            byte[] statusline_to_bytes = Encoding.UTF8.GetBytes(statusline);
            StringBuilder tmp = new StringBuilder();
            foreach (var key in ResponseHead)
                tmp.AppendFormat("{0}:{1}\r\n", key.Key, key.Value);
            response.Send(statusline_to_bytes);  //发送状态行
            response.Send(tmp.ToString().getBytes());  //发送应答头
            response.Send(new byte[] { (byte)'\r', (byte)'\n' });  //发送空行
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            response.Send(buffer);  //发送正文（html）
            response.Close();
        }
    }
}
