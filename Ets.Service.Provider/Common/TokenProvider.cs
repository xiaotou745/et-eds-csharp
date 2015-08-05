using Ets.Dao.Common;
using Ets.Model.Common;
using Ets.Service.IProvider.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Util;
namespace Ets.Service.Provider.Common
{

    public class TokenProvider : ITokenProvider
    {
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150731</UpdateTime>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetToken(TokenModel model)
        {
            string cacheKey = model.Ssid + "_" + model.Appkey;
            string cacheKeyOld = model.Ssid + "_" + model.Appkey + "_old";
            string cacheValue, cacheValueOld;
            var redis = new ETS.NoSql.RedisCache.RedisCache();

            cacheValue = redis.Get<string>(cacheKey);//获取当前Token值
            redis.Set(cacheKeyOld, cacheValue, new TimeSpan(0, 2,0));//把当前值赋值到旧的Token中
            cacheValue = Helper.GetToken();//生成新的Token
            redis.Set(cacheKey, cacheValue, new TimeSpan(2, 0, 0));
            return cacheValue;
        }
    }
}
