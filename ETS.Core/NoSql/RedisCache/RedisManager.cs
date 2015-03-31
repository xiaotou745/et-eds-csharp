using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceKit.Redis;
namespace ETS.NoSql.RedisCache
{
    /// <summary>
    /// Redis管理操作类
    /// </summary>
    public sealed class RedisManager
    {
        /// <summary>
        /// redis客户端连接池信息
        /// </summary>
        private static PooledRedisClientManager prcm;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }


        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            try
            {
                // ip1：端口1,ip2：端口2
                var serverlist = ConfigurationManager.AppSettings["Redis.ServerList"].Split(',');
                prcm = new PooledRedisClientManager(serverlist, serverlist,
                                 new RedisClientManagerConfig
                                 {
                                     MaxWritePoolSize = 400,
                                     MaxReadPoolSize = 400,
                                     AutoStart = true
                                 });
                // 连接配置
                //prcm.ConnectTimeout = config.ConnectTimeout;
                //prcm.PoolTimeOut = config.PoolTimeOut;
                //prcm.SocketSendTimeout = config.SocketSendTimeout;
                //prcm.SocketReceiveTimeout = config.SocketReceiveTimeout;
                prcm.Start();
            }
            catch (Exception exception)
            {
            }
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();

            return prcm.GetClient();
        }
    }
}
