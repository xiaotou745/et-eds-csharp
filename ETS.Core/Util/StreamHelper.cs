using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Util
{
  public static  class StreamHelper
    {
        /// <summary>
        /// Stream Stream 转换为 byte 数组
        /// </summary>
        /// <returns></returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }



        /// <summary>
        /// 转换为 string，使用 Encoding.Default 编码
        /// </summary>
        /// <returns></returns>
        public static string ToStr(this byte[] arr)
        {
            return System.Text.Encoding.Default.GetString(arr);
        }
    }
}
