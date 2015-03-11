using System;

namespace ETS.Cacheing
{
    /// <summary>
    /// 缓存抽象类
    /// </summary>
    public abstract class AbstractCache
    {

        #region 添加缓存对象
        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="files">缓存依赖对象</param>
        public abstract void AddObject(string key, object obj, params string[] files);
        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">缓存对象</param>
        public abstract void AddObject(string key, object obj);
        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="dateTime">缓存过期时间</param>
        public abstract void AddObject(string key, object obj, DateTime dateTime);
        #endregion

        #region 移除缓存对象
        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        public abstract void RemoveObject(string key);
        #endregion

        #region 返回一个指定的对象
        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public abstract object RetrieveObject(string key);
        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return RetrieveObject(key);
            }
        }
        #endregion

    }
}
