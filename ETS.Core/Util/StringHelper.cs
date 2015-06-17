using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Letao.Util
{
    public class StringHelper
    {
        /// <summary>
        /// 获得一个唯一字符，16位
        /// </summary>
        /// <returns></returns>
        public static string GuidString()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        /// <summary>
        /// 截断字符串，保留指定字节数
        /// </summary>
        /// <param name="failReason"></param>
        /// <returns></returns>
        public static string TruncateString(string input, uint keepByteCount)
        {
            while (Encoding.GetEncoding("gb2312").GetByteCount(input) > keepByteCount)
            {
                input = input.Remove(input.Length - 1);
            }
            return input;
        }
        /// <summary>
        /// 允许0开头，如果allow_zero是true
        /// </summary>
        /// <param name="random_length"></param>
        /// <param name="alow_zero"></param>
        /// <returns></returns>
        public static string GetRandomNum(int random_length, bool allow_zero)
        {
            int i, p;
            string r_string;

            if (random_length <= 0 || random_length > 20)
                return null;

            Random rd = StringHelper.SYS_RD;

            //再用一个随机因子，时间。。。
            long tick = DateTime.Now.Ticks;
            Random rd2 = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            if (!allow_zero)
                while ((p = GetNextRandDigit(rd, rd2)) == 0)
                { }
            else
                p = GetNextRandDigit(rd, rd2);

            r_string = p.ToString();

            for (i = 0; i < random_length - 1; i++)
            {
                p = GetNextRandDigit(rd, rd2);
                r_string += p.ToString();
            }

            return r_string;
        }
        /// <summary>
        /// 为更强的随机数准备的
        /// </summary>
        /// <param name="rd1"></param>
        /// <param name="rd2"></param>
        /// <returns></returns>
        private static int GetNextRandDigit(Random rd1, Random rd2)
        {
            return (rd1.Next(10) + rd2.Next(10)) % 10;
        }
        public static Random SYS_RD
        {
            get
            {
                return new Random(GetRandomSeed());

                //if (_SYS_RD == null) //全局共享rd会造成死锁问题
                //    _SYS_RD = new Random(GetRandomSeed());

                //return _SYS_RD;

            }
        }


        private static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <param name="extStr"></param>
        /// <returns></returns>
        public static string SubString(string source, int length, string extStr)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            if (source.Length <= length)
                return source;

            return source.Substring(0, length) + extStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeLen"></param>
        /// <returns></returns>
        public static string CreateVerifyCode(int codeLen)
        {
            //const string codeSerial = "1,2,3,4,5,6,7,8,9,0,A,B,";
            string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";


            string[] arr = codeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }

        /// <summary>
        /// Gets the PY string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string GetPYString(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {//字母和符号原样保留
                    tempStr += c.ToString();
                }
                else
                {//累加拼音声母
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }

        /// <summary>
        /// Gets the PY char.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private static string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));

            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "g";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";

            return "*";
        }


        /// <summary>
        /// Creates the number.
        /// </summary>
        /// <param name="codeLen">The code len.</param>
        /// <returns></returns>
        public static string CreateNumber(int codeLen)
        {
            const string codeSerial = "1,2,3,4,5,6,7,8,9,0";
            string[] arr = codeSerial.Split(',');

            return CreateRandomStr(codeLen, arr);
        }

        /// <summary>
        /// Creates the password.
        /// </summary>
        /// <param name="codeLen">The code len.</param>
        /// <returns></returns>
        public static string CreatePassword(int codeLen)
        {
            const string codeSerial = "1,2,3,4,5,6,7,8,9,0,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] arr = codeSerial.Split(',');

            return CreateRandomStr(codeLen, arr);
        }



        /// <summary>
        /// Creates the random STR.
        /// </summary>
        /// <param name="codeLen">The code len.</param>
        /// <param name="baseSerial">The base serial.</param>
        /// <returns></returns>
        public static string CreateRandomStr(int codeLen, string[] baseSerial)
        {
            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, baseSerial.Length - 1);

                code += baseSerial[randValue];
            }

            return code;
        }

        /// <summary>
        /// Gets the friendly date.
        /// </summary>
        /// <param name="dtTime">The dt time.</param>
        /// <returns></returns>
        public static string GetFriendlyTime(DateTime dtTime)
        {
            DateTime dtNow = DateTime.Now;
            TimeSpan tsDiff = dtNow - dtTime;
            string lastDateStr = "";
            if (tsDiff.TotalMinutes <= 1)
            {
                lastDateStr = "<span class='s time'>" + (int)tsDiff.TotalSeconds + "秒前</span>";
            }
            else if (tsDiff.TotalHours <= 1)
            {
                lastDateStr = "<span class='m time'>" + (int)tsDiff.TotalMinutes + "分前</span>";
            }
            else if (tsDiff.TotalHours <= 8)
            {
                lastDateStr = "<span class='h time'>" + (int)tsDiff.TotalHours + "小时前</span>";
            }
            else if (tsDiff.Days == 0 && dtNow.Day == dtTime.Day)
            {
                lastDateStr = "<span class='td time'>今天 " + dtTime.ToString("HH:mm") + "</span>";
            }
            else if (tsDiff.Days <= 1 && dtNow.Day == dtTime.Day + 1)
            {
                lastDateStr = "<span class='yt time'>昨天 " + dtTime.ToString("HH:mm") + "</span>";
            }
            else
            {
                lastDateStr = "<span class='t time'>" + dtTime.ToString("yy-MM-dd hh:mm") + "</span>";
            }
            return lastDateStr;// +dtTime + ":::" + tsDiff.Days;
        }

        /// <summary>
        /// 验证手机号是否有效
        /// 2015年6月17日 10:54:01
        /// 窦海超
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <returns></returns>
        public static bool CheckPhone(string phoneNo)
        {
            return Regex.IsMatch(phoneNo, @"^1(\d{10})$");//只验证了第一位手机是1，且满足11位
        }

    }


    /// <summary>
    /// string 比较器重写 实现类似 reason排在 reason_code之前功能 add by caoheyang  20150249 
    /// </summary>
    public class NewStringComparer : IComparer<string>
    {
        public int Compare(string a, string b)
        {
            string akey = a.Substring(0, a.IndexOf('='));
            string bkey = b.Substring(0, b.IndexOf('='));
            if (akey.IndexOf(bkey) == 0)
                return 1;
            else if (bkey.IndexOf(akey) == 0)
                return -1;
            return a.CompareTo(b);
        }
    }
}
