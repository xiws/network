using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Tool
{
    public static class MExtend
    {
        /// <summary>
        /// string to bytes
        /// </summary>
        /// <param name="str"></param>
        /// <param name="EncodingName">编码名字</param>
        /// <returns></returns>
        public static byte[] getBytes(this string str,string EncodingName="UTF-8")
        {
            return Encoding.GetEncoding(EncodingName).GetBytes(str);
        }
    }
}
