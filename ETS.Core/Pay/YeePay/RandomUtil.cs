using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Wallet.Common.Utilities
{
    public class RandomUtil
    {
        /// <summary>
        /// 随机码
        /// </summary>
        /// <param name="pwdlen">随机码长度</param>
        /// <returns></returns>
        public static string MakeRandomStr(int pwdlen)
        {
            var tmpstr = "";
            const string randomchars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var rnd = new Random();
            for (int i = 0; i < pwdlen; i++)
            {
                int iRandNum = rnd.Next(randomchars.Length);
                tmpstr += randomchars[iRandNum];
            }
            return tmpstr;
        }
        public static string MakeRandomNum(int pwdlen)
        {
            var tmpstr = "";
            const string randomchars = "0123456789";
            var rnd = new Random();
            for (int i = 0; i < pwdlen; i++)
            {
                int iRandNum = rnd.Next(randomchars.Length);
                tmpstr += randomchars[iRandNum];
            }
            return tmpstr;
        }
    }
}
