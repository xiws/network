using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core;
using Test.pilieline;

namespace Test.http
{
    class FilterLine : IHttpFilter
    {
        public void FilterAfter(HttpContent content)
        {
            throw new NotImplementedException();
        }

        public void FilterAfterAsyc(HttpContent content)
        {
            throw new NotImplementedException();
        }

        public void FilterBefore(HttpContent content)
        {
            throw new NotImplementedException();
        }

        public void FilterBeforeAsyc(HttpContent content)
        {
            throw new NotImplementedException();
        }
    }
}
