using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
