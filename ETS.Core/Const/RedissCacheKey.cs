using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Const
{
    public class RedissCacheKey
    {
        /// <summary>
        /// 商户用户状态缓存key
        /// </summary>
        public const string BusinessProvider_GetUserStatus = "BusinessProvider_GetUserStatus_{0}";
        /// <summary>
        /// 骑士用户状态缓存key
        /// </summary>
        public const string ClienterProvider_GetUserStatus = "ClienterProvider_GetUserStatus_{0}";
    }
}
