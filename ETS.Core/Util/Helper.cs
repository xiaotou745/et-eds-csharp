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
            if (appType == AppType.B端)
            {
                result = "1" + DateTime.Now.ToString("MMddHHmm") + phoneNo.Substring(6);
            }
            if (appType == AppType.C端)
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
        /// 获取图片路径+名称
        /// 窦海超
        /// 2015年4月9日 09:22:55
        /// </summary>
        /// <returns></returns>
        public static string CreateImageName(string fileExt = ".jpg")
        {
            ETS.NoSql.RedisCache.RedisCache redis = new NoSql.RedisCache.RedisCache();
            long imgName = redis.Incr(ETS.Const.RedissCacheKey.ImageIdentity, DateTime.Now.AddHours(1));//为当前图片增加每小时内的唯一标识
            return string.Concat(DateTime.Now.ToString("yyyy/MM/dd/HH/mmss"), imgName, fileExt);
        }
    }
}
