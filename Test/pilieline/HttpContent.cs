using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core;

namespace Test.pilieline
{
    public interface IHttpContent
    {
         HttpServer Server { get; }

         HttpRequest Request { get;  }

         HttpResponse Response { get;  }

         Session session { get; }
    }
}
