using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Net.Sockets;

namespace network
{
    public enum StreamTypes { bin, message, cmd };

    public class dataStruct
    {
        public const int headLength = 64;

        /// <summary>
        /// 文件类型保存位置
        /// </summary>
        public const int filetypeindex = 0;

        /// <summary>
        /// 文件长度保存位置
        /// </summary>
        public const int filelengthindex = 1;

        public const int filenameindex = 5;

        /// <summary>
        /// 发送的数据流类型
        /// </summary>
        public StreamTypes StreamType { get;  set; }
        /// <summary>
        /// 如果发送的是文件，保留文件名
        /// </summary>
        public string FileName { get;  set; }
        /// <summary>
        /// 数据流的长度
        /// </summary>
        public long Data_Length { get { if (datalist == null) return 0;return datalist.Length; } }
        /// <summary>
        /// 编码
        /// </summary>
        public byte[] datalist { get; set; }

        public static byte[] DataStructToByte(dataStruct res)
        {
            byte[] head = new byte[headLength+res.Data_Length];
            head[dataStruct.filetypeindex] =(byte)res.StreamType;
            byte[] length= BitConverter.GetBytes(res.Data_Length);
            length.CopyTo(head, dataStruct.filelengthindex);
            if (res.FileName != null)
            {
                var fl = Encoding.UTF8.GetBytes(res.FileName);
                fl.CopyTo(head, dataStruct.filenameindex);
            }
            res.datalist.CopyTo(head, headLength);
            return head;
        }
        /// <summary>
        /// 返回datastruct
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static dataStruct byteToDataStruct(byte[] b)
        {
            dataStruct dt = new dataStruct();
            dt.StreamType = (StreamTypes)b[dataStruct.filetypeindex];
            int datalength= BitConverter.ToInt32(b,dataStruct.filelengthindex);
            dt.datalist = new byte[datalength];
            int fc = dataStruct.headLength - dataStruct.filenameindex;
            if (dt.StreamType == StreamTypes.bin)
            {
                byte[] filename = new byte[fc];
                for (int i = 0; i < dataStruct.headLength - dataStruct.filenameindex; i++)
                {
                    filename[i] = b[i + dataStruct.filenameindex];
                }
                dt.FileName = Encoding.Default.GetString(filename);
            }
            return dt;
        }
        public dataStruct GetDatastruct(Stream st)
        {
            return null;
        }
        public byte[] GetStream()
        {
            return null;
        }
    }
}
