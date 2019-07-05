using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core
{
    public class HttpServer
    {
        /// <summary>
        /// server path
        /// </summary>
        public string ServerPath { get; }
        /// <summary>
        /// environment
        /// </summary>
        public Env env { get; }

        public HttpServer()
        {
            this.ServerPath = @"E:\network\Test\bin\Debug";
        }
    }
}
