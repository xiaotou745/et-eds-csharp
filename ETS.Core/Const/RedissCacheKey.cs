using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Const
{
    public class RedissCacheKey
    {
        public const string BusinessProvider_GetUserStatus = "BusinessProvider_GetUserStatus_{0}";//商户用户状态缓存key
        public const string ClienterProvider_GetUserStatus = "ClienterProvider_GetUserStatus_{0}";//骑士用户状态缓存key
    }
}
