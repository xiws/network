using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Test.Tool;

namespace Test.Core
{
    public class HttpRequest
    {
        const char Enter = '\n';
        /// <summary>
        /// 接收到正文
        /// </summary>
        string Content { get; set; }
        /// <summary>
        /// http报头
        /// </summary>
        public Dictionary<string,string> Head { get;  }
        /// <summary>
        /// get post 方法
        /// </summary>
        public string Method { get;  }
        /// <summary>
        /// host
        /// </summary>
        public string url { get;  }

        /// <summary>
        /// http 版本
        /// </summary>
        public string version { get;  }

        private Socket response;

        public HttpRequest(string Http,Socket soc)
        {
            this.response = soc;
            Head=new Dictionary<string, string>();
            int index= Http.IndexOf("\r\n\r\n");
            string head= Http.Substring(0, index);
            string content = Http.Substring(index + 1).Trim();
            var list= head.Split(Enter);
            for(int i = 1; i < list.Length; i++)
            {
                var kv= list[i].Split(':');
                Head.Add(kv[0].Trim(), kv[1].Trim());
            }
            list= list[0].Split(' ');
            Method = list[0];
            url = list[1];
            version = list[2].Trim();
        }

        public HttpResponse GetResponse()
        {
            return new HttpResponse(this.response);
        }

    }
}
