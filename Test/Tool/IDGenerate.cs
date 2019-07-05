using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Tool
{
    class IDGenerate
    {
        /// <summary>
        /// 秒级时间
        /// </summary>
        public DateTime date { get; set; }
        /// <summary>
        /// 创建类型
        /// </summary>
        public int type { get; set; }  
        /// <summary>
        /// 创建方法
        /// </summary>
        public int GenerateFun { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public int driverid { get; set; }

        public IDGenerate(DateTime date, int type, int generateFun, int id, int driverid)
        {
            this.date = date;
            this.type = type;
            GenerateFun = generateFun;
            this.id = id;
            this.driverid = driverid;
        }

        public IDGenerate()
        {

        }
        public void ConvertId()
        {
            int timestamp= Tool.Timestamp();
            

        }
        public static IDGenerate Convert()
        {
            return null;
        }
    }
}
