using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ETS.Util
{
    public abstract class Helper
    {
        /// <summary>
        /// 根据手机号生成code (13位)
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="appType"></param>
        /// <returns></returns>
        public static string generateCode(string phoneNo, AppType appType)
        {
            string result = "";
            if (appType == AppType.FormB)
            {
                result = "1" + DateTime.Now.ToString("MMddHHmm") + phoneNo.Substring(6);
            }
            if (appType == AppType.FormC)
            {
                result = "0" + DateTime.Now.ToString("MMddHHmm") + phoneNo.Substring(6);
            }
            return result;
        }
        /// <summary>
        /// 根据userId生成订单号(15位)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string generateOrderCode(int userId)
        {
            string result = userId.ToString() + DateTime.Now.ToString("yyMMddHHmmss") + new Random().Next(100).ToString("D3");
            return result;
        }



        /// <summary>
        /// 生成guid
        /// </summary>
        /// <returns></returns>
        public static string Uuid()
        {
            StringBuilder SB = new StringBuilder();
            string[] uuid = Guid.NewGuid().ToString().Split('-');
            foreach (var item in uuid)
            {
                SB.Append(item);
            }
            return SB.ToString();
        }

        /// <summary>
        /// 生成guid
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
           return Uuid();
        }

        /// <summary>
        /// 获取图片路径+名称
        /// 窦海超
        /// 2015年4月9日 09:22:55
        /// </summary>
        /// <returns></returns>
        public static string CreateImageName(string fileExt = ".jpg")
        {
            //ETS.NoSql.RedisCache.RedisCache redis = new NoSql.RedisCache.RedisCache();
            //long imgName = redis.Incr(ETS.Const.RedissCacheKey.ImageIdentity, DateTime.Now.AddHours(1));//为当前图片增加每小时内的唯一标识 
            //return string.Concat(DateTime.Now.ToString("yyyy/MM/dd/HH/mmss"), new Random().Next(100).ToString("D3"), fileExt);
            return string.Concat(new Random().Next(1000).ToString("D4"), fileExt);
        }
        #region 生成订单号专用

        public static string CreateRandomData(int len)
        {
            int length = len - 1;
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            StringBuilder validateNumberStr = new StringBuilder();
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            return validateNumberStr.ToString();
        }

        /// <summary>
        /// 截取timespan做为订单号
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static string SplitTimeSpan(string timeSpan, int splitLen)
        {
            if (string.IsNullOrEmpty(timeSpan) || timeSpan.Length < splitLen)
            {
                return string.Empty;
            }
            return timeSpan.Substring(timeSpan.Length - splitLen, splitLen);
        }

        /// <summary>
        /// 根据userId+时间+时间戳+随机数 生成订单号(15位)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string generateOrderCode(int userId, string timeSpan)
        {
            string result = userId.ToString() + DateTime.Now.ToString("yyMMddHHmmss") + CreateRandomData(3) + SplitTimeSpan(timeSpan, 3);
            return result;
        }
        #endregion


        /// <summary>
        /// 随机出的字符串
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GenCode(int num)
        {
            string str = "123456789abcdefghjkmnpqrstuvwxyz";
            char[] chastr = str.ToCharArray();
            string code = "";
            Random rd = new Random();
            int i;
            for (i = 0; i < num; i++)
            {
                code += str.Substring(rd.Next(0, str.Length), 1);
            }
            return code;
        }
    }
}
