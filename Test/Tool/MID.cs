using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Test.Tool
{
    public struct MID
    {
        byte[] list;
        public static MID Parse(string str)
        {
            byte b = new byte();
            b = 1 << 7;
            b = 1 << 6;
            int t= Tool.Timestamp();
            return MID.Parse("");
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    internal sealed class __DynamicallyInvokableAttribute : Attribute
    {
    }

}
