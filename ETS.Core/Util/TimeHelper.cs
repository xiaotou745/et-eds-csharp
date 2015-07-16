using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Util
{

    /// <summary>
    /// 时间相关帮助类 add by caoheyang 20150326
    /// </summary>
    public  class TimeHelper
    {
        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public static string GetTimeStamp(bool bflag = true)
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret =Math.Round(ts.TotalSeconds).ToString();
            else
                ret =Math.Round(ts.TotalMilliseconds).ToString();
            return ret;
        }

        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="timestamp">时间戳</param>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public static DateTime TimeStampToDateTime(string timestamp, bool bflag = true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime dtZone = new DateTime(1970, 1, 1, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                return dtZone.AddSeconds(Convert.ToDouble(timestamp));
            else
                return dtZone.AddMilliseconds(Convert.ToDouble(timestamp));
        }  
    }
}
