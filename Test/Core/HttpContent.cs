using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Test.pilieline;

namespace Test.Core
{
    class HttpContent: IHttpContent
    {

        public HttpServer Server { get; private set; }

        public HttpRequest Request { get; private set; }

        public HttpResponse Response { get; private set; }

        public Session session { get; private set; }

        private HttpContent() { }

        public static HttpContent BuilderHttpContent(string http,Socket socket)
        {
            HttpContent https = new HttpContent();
            https.Server = new HttpServer();
            https.Request = new HttpRequest(http, socket);
            https.Response = https.Request.GetResponse();
            
            if(https.Request.Head.ContainsKey("Cookie"))
            {
                var str= https.Request.Head["Cookie"];
                var list= str.Split(';');
                Dictionary<string, string> tmp = new Dictionary<string, string>();
                for(int i = 0; i < list.Length; i++)
                {
                    var dictmp = list[i].Split('=');
                    if(!tmp.ContainsKey(dictmp[0]))
                    tmp.Add(dictmp[0], dictmp[1]);
                }
                https.session = new Session(tmp);
                
            }
            
            return https;
        }
    }
}
