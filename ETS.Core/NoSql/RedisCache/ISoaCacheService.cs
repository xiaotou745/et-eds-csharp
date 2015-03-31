using System.Collections.Generic;

namespace ETS.NoSql.RedisCache
{
    using System;

    /// <summary>
    /// 接口名称：ISoaCacheService
    /// 命名空间：Ets.SoaService.ICaches
    /// 接口功能：Soa缓存接口
    /// </summary>
    /// 创建者：周超
    /// 创建日期：7/1/2014 10:02 AM
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public interface ISoaCacheService
    {
        /// <summary>
        /// 添加缓存，并设置过期时间
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <param name="expiredTime">过期时间</param>
        /// 创建者：周超
        /// 创建日期：2013/10/17 20:35
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        void Add(string key, object value, DateTime expiredTime);

        void AddList(string listId, object value, DateTime expiredTime);

        /// <summary>
        /// 修改缓存的值，并设置过期时间
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <param name="expiredTime">过期时间</param>
        /// 创建者：周超
        /// 创建日期：2013/10/17 20:35
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        void Replace(string key, object value, DateTime expiredTime);

        /// <summary>
        /// 设置缓存，并修改过期时间
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <param name="expiredTime">过期时间</param>
        /// 创建者：周超
        /// 创建日期：2013/10/17 20:35
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        void Set(string key, object value, DateTime expiredTime);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// 创建者：周超
        /// 创建日期：2013/10/17 20:35
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        void Delete(string key);
        void DeleteListItem(string listId, object value);

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>
        /// 缓存的值
        /// </returns>
        /// 创建者：周超
        /// 创建日期：2013/10/17 20:35
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        T Get<T>(string key);

        List<T> GetList<T>(string listId);

        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>
        /// True存在，False不存在
        /// </returns>
        /// 创建者：周超
        /// 创建日期：2013/10/17 20:35
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        bool Exists(string key);
    }
}