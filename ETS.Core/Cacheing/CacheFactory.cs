using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ETS.Cacheing
{
    /// <summary>
    /// 缓存抽象工厂类
    /// </summary>
    public class CacheFactory
    {

        #region 变量

        /// <summary>
        /// 线程安全
        /// </summary>
        private static object lockHelper = new object();

        /// <summary>
        /// 数据库操作类
        /// </summary>
        private static AbstractCache abstractCache = null;

        #endregion


        #region 缓存类实例

        /// <summary>
        /// 缓存类实例
        /// </summary>
        public static AbstractCache Instance
        {
            get
            {
                if (abstractCache == null)
                {
                    lock (lockHelper)
                    {
                        if (abstractCache == null)
                        {
                            try
                            {
                                abstractCache =
                                    (AbstractCache)
                                        Activator.CreateInstance(
                                            Type.GetType("ETS.Cacheing." + ConfigurationManager.AppSettings["CacheType"]));
                            }
                            catch
                            {
                                throw new Exception("请检查web.config节点CacheType是否正确");
                            }
                        }
                    }
                }
                return abstractCache;
            }
        }

        public static string Get(string key)
        {
            if (Instance[key] == null)
            {
                return string.Empty;
            }
            return Instance[key].ToString();
        }
        #endregion
    }
}
