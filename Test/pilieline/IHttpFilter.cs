using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core;

namespace Test.pilieline
{
     interface IHttpFilter
     {
        void FilterBefore(HttpContent content);
        void FilterAfter(HttpContent content);
        void FilterBeforeAsyc(HttpContent content);
        void FilterAfterAsyc(HttpContent content);
      }
}
