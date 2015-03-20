using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ETS.Util
{
    /// <summary>
    /// MDS5 加密类 add by caoheyang 20150203
    /// </summary>
    public class MD5Helper
    {
        private const string KEY = "etsapi2012611bhl";

        // 十六进制下数字到字符的映射数组
        private static string[] hexDigits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

        /// <summary>
        /// 为字符串加密  add by caoheyang 20150203
        /// </summary>
        /// <param name="inputStr">待加密字符串</param>
        /// <returns></returns>
        public static string MD5(string inputStr)
        {
            return EncodeByMD5(KEY + inputStr);
        }

        /// <summary>
        /// 字符串加密（无key）  add by caoheyang 20150203
        /// </summary>
        /// <param name="inputStr">待加密字符串</param>
        /// <returns></returns>
        public static string MD5NoKey(string inputStr)
        {
            return EncodeByMD5(inputStr);
        }

        /// <summary>
        /// 验证输入的密码是否正确 add by caoheyang 20150203
        /// </summary>
        /// <param name="password">真正的密码（加密后的真密码）</param>
        /// <param name="inputString">输入的字符串</param>
        /// <returns>n 验证结果，boolean类型</returns>
        public static bool AuthenticatePassword(string password, string inputString)
        {
            if (password.Equals(EncodeByMD5(inputString)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 对字符串进行MD5编码 add by caoheyang 20150203
        /// </summary>
        /// <param name="originString">字符串</param>
        /// <returns></returns>
        private static string EncodeByMD5(string originString)
        {
            if (originString != null)
            {
                try
                {
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    byte[] results = md5.ComputeHash(Encoding.UTF8.GetBytes(originString));   //得到加密字节数组
                    string result = ByteArrayToHexString(results);  // 将得到的字节数组变成字符串返回
                    return result;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return null;
        }

        /// <summary>
        /// 轮换字节数组为十六进制字符串 add by caoheyang 20150203
        /// </summary>
        /// <param name="b">字节数组</param>
        /// <returns>十六进制字符串</returns>
        private static string ByteArrayToHexString(byte[] b)
        {
            StringBuilder resultSb = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                resultSb.Append(ByteToHexString(b[i]));
            }
            return resultSb.ToString();
        }

        /// <summary>
        /// 将一个字节转化成十六进制形式的字符串 add by caoheyang 20150203
        /// </summary>
        /// <param name="b">字符串</param>
        /// <returns></returns>
        private static string ByteToHexString(byte b)
        {
            int n = b;
            if (n < 0)
            {
                n = 256 + n;
            }
            int d1 = n / 16;
            int d2 = n % 16;
            return hexDigits[d1] + hexDigits[d2];
        }
    }
}
