using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core
{
    public class Session
    {
        public string SessionId { get; private set; }

        public Session(Dictionary<string, string> cookie)
        {
            if(cookie.ContainsKey("ASP.NET_SessionId"))
                this.SessionId = cookie["ASP.NET_SessionId"];
            this.Cookie = cookie;
        }
        public Dictionary<string,string> Cookie { get; protected set; }

        public void Add(string key,string value)
        {

        }
    }
}
