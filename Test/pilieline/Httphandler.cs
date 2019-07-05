using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.pilieline
{
    public interface Httphandler
    {
        void Run(IHttpContent httpcontext);
    }
}
