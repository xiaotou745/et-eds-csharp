using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS
{
    public class Config
    {
        /// <summary>
        /// 读库
        /// </summary>
        public static string SuperMan_Read { get { return ConfigConnectionStrings("SuperMan_Read"); } }
        /// <summary>
        /// 写库
        /// </summary>
        public static string SuperMan_Write { get { return ConfigConnectionStrings("SuperMan_Write"); } }

        #region 取Web.Config值

        /// <summary>
        /// 取Web.Config值

        /// </summary>
        /// <param name="KeyName"></param>
        /// <returns></returns>
        public static string ConfigKey(string KeyName)
        {
            return System.Configuration.ConfigurationManager.AppSettings[KeyName];
        }
        /// <summary>
        /// 取wenconfig数据库连接字符串的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ConfigConnectionStrings(string name)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        #endregion
    }
}
