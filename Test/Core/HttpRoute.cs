using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.pilieline;

namespace Test.Core
{
    class HttpRoute : IHttpRoute
    {
        public void Route(RouteInfo route)
        {
            throw new NotImplementedException();
        }
    }

    public class RouteInfo
    {
        /// <summary>
        /// 路由路径
        /// </summary>
        public string RoutePath { get; set; }

        /// <summary>
        /// k开启静态页面
        /// </summary>
        public bool StaticPage { get; set; }
    }
}
