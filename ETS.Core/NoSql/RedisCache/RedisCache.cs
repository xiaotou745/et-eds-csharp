using System;
using System.Collections.Generic;
using System.Linq;
using NServiceKit.Redis;
using NServiceKit.Redis.Support;
using ETS.Util;

namespace ETS.NoSql.RedisCache
{
    /// <summary>
    /// 类名称：RedisCache
    /// 命名空间：Ets.SoaService.Caches
    /// 类功能：缓存类
    /// </summary>
    /// 创建者：单琪彬
    /// 创建日期：7/15/2014 10:19 AM
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public class RedisCache : ISoaCacheService
    {
        /// <summary>
        /// 字段Success
        /// </summary>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 1:58 PM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        internal const int Success = 1;

        /// <summary>
        /// 字段client
        /// </summary>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 1:56 PM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        //private readonly RedisClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCache"/> class.
        /// </summary>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 3:08 PM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        public RedisCache()
        {
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 10:19 AM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        /// <exception cref="System.NotImplementedException"></exception>
        public void Add(string key, object value)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                if (Redis.ContainsKey(key))
                {
                    Redis.Set<byte[]>(key, new ObjectSerializer().Serialize(value));
                }
                else
                {
                    Redis.Add<byte[]>(key, new ObjectSerializer().Serialize(value));
                }
            }
        }

        /// <summary>
        /// 添加缓存，并设置过期时间
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <param name="expiredTime">过期时间</param>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 10:19 AM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        /// <exception cref="System.NotImplementedException"></exception>
        public void Add(string key, object value, System.DateTime expiredTime)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                if (Redis.ContainsKey(key))
                {
                    Redis.Set<byte[]>(key, new ObjectSerializer().Serialize(value), expiredTime);
                }
                else
                {
                    Redis.Add<byte[]>(key, new ObjectSerializer().Serialize(value), expiredTime);
                }
            }
        }

        public void AddList(string listId, object value, DateTime expiredTime)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                if (Redis.ContainsKey(listId))
                {
                    Redis.AddItemToList(listId, value.Serialize());
                    Redis.ExpireEntryAt(listId, expiredTime);
                }
                else
                {
                    Redis.AddItemToList(listId, value.Serialize());
                    Redis.ExpireEntryAt(listId, expiredTime);
                }
            }
        }

        /// <summary>
        /// 修改缓存的值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 10:19 AM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        /// <exception cref="System.NotImplementedException"></exception>
        public void Replace(string key, object value)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                Redis.Replace<byte[]>(key, new ObjectSerializer().Serialize(value));
            }
        }

        /// <summary>
        /// 修改缓存的值，并设置过期时间
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <param name="expiredTime">过期时间</param>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 10:20 AM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        /// <exception cref="System.NotImplementedException"></exception>
        public void Replace(string key, object value, System.DateTime expiredTime)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                Redis.Replace<byte[]>(key, new ObjectSerializer().Serialize(value), expiredTime);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 10:20 AM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        /// <exception cref="System.NotImplementedException"></exception>
        public void Set(string key, object value)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                Redis.Set<byte[]>(key, new ObjectSerializer().Serialize(value));
            }
        }

        /// <summary>
        /// 设置缓存，并修改过期时间
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <param name="expiredTime">过期时间</param>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 10:20 AM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        /// <exception cref="System.NotImplementedException"></exception>
        public void Set(string key, object value, System.DateTime expiredTime)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                Redis.Set<byte[]>(key, new ObjectSerializer().Serialize(value), expiredTime);
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 10:20 AM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(string key)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                Redis.Remove(key);
            }
        }

        public void DeleteListItem(string listId, object value)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                Redis.RemoveItemFromList(listId, value.Serialize());
            }
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>
        /// 缓存的值
        /// </returns>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 10:20 AM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        /// <exception cref="System.NotImplementedException"></exception>
        public T Get<T>(string key)
        {
            try
            {
                using (IRedisClient Redis = RedisManager.GetClient())
                {
                    var obj = new ObjectSerializer().Deserialize(Redis.Get<byte[]>(key));
                    return (T)obj;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public List<T> GetList<T>(string listId)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                List<string> list = Redis.GetAllItemsFromList(listId);
                List<T> result = list.Select(p => p.Deserialize<T>()).ToList();
                return result;
            }
        }

        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>
        /// True存在，False不存在
        /// </returns>
        /// 创建者：单琪彬
        /// 创建日期：7/15/2014 10:20 AM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Exists(string key)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                return Redis.ContainsKey(key);
            }
        }

        /// <summary>
        /// 获取自增数据
        /// 窦海超
        /// 2015年4月9日 09:18:02
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiredTime">生命周期</param>
        /// <returns>自增值</returns>
        public long Incr(string key, DateTime expiredTime)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                long tempNum = Redis.IncrementValue(key);
                if (expiredTime != null)
                {
                    Redis.ExpireEntryAt(key, expiredTime);
                }
                return tempNum;
            }
        }
        /// <summary>
        /// 查询keys
        /// 窦海超
        /// 2015年4月14日 13:55:05
        /// </summary>
        /// <param name="likeKey">键,查询相关的KEY，用*来代替未知数据如：*123*，则查询中间内容为123的相似键</param>
        /// <returns></returns>
        public List<string> Keys(string likeKey)
        {
            using (IRedisClient Redis = RedisManager.GetClient())
            {
                return Redis.SearchKeys(likeKey);
            }
        }
    }
}
