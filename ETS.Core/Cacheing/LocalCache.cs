using System;
using System.Web;
using System.Web.Caching;
namespace ETS.Cacheing
{
    /// <summary>
    /// 客户端缓存
    /// </summary>
    public sealed class LocalCache : AbstractCache
    {

        #region 变量
        /// <summary>
        /// 内存缓存
        /// </summary>
        private static readonly Cache WebCache = HttpRuntime.Cache;
        #endregion

        #region 添加缓存对象
        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="files">缓存依赖对象</param>
        public override void AddObject(string key, object obj, params string[] files)
        {
            WebCache.Insert(key, obj, new CacheDependency(files), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }
        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">缓存对象</param>
        public override void AddObject(string key, object obj)
        {
            WebCache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }
        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="dateTime">缓存过期时间</param>
        public override void AddObject(string key, object obj, DateTime dateTime)
        {
            WebCache.Insert(key, obj, null, dateTime, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }
        #endregion

        #region 移除缓存对象
        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        public override void RemoveObject(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            WebCache.Remove(key);
        }
        #endregion

        #region 返回一个指定的对象
        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public override object RetrieveObject(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return WebCache.Get(key);
        }

        #endregion

    }
}
