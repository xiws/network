using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core
{
    static class mapping
    {
        public static void fileMapping(HttpContent context)
        {
            var list= context.Request.url.Split('/');
            string html = "";
            if (context.Request.url.Equals("/")|| list[list.Length-1]=="")
            {
                  html= context.Server.ServerPath + context.Request.url+ "/index.html";
            }
            else
            {
                html = context.Server.ServerPath + context.Request.url;
            }
            if (System.IO.File.Exists(html))
            {
                string content = System.IO.File.ReadAllText(html);
                context.Response.Write(content);
            }
            else
            {
                throw new HttpError(404);
            }
        }
    }
}
