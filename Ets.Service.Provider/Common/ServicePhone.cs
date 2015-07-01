using ETS.Const;
using Ets.Dao.Common;
using Ets.Model.Common;
using Ets.Service.IProvider.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Common
{
    public class ServicePhone : IServicePhone
    {
        ServicePhoneDao sPhoneDao = new ServicePhoneDao();
        private int tryCount = 0;//重试次数
        /// <summary>
        /// 客服电话获取
        /// 窦海超
        /// 2015年3月16日 11:44:54
        /// </summary>
        /// <param name="CityName">城市名称</param>
        /// <returns></returns>
        public ResultModelServicePhone GetCustomerServicePhone(string CityName)
        {
            if (string.IsNullOrEmpty(CityName))
            {
                CityName = "北京市";
            }
            DataTable dt = null;
            if (tryCount > 3)
            {
                //重试次数4次
                return new ResultModelServicePhone();
            }
            #region 缓存验证
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            const string cacheKey = RedissCacheKey.Ets_Service_Provider_Common_ServicePhone;
            var cacheList = redis.Get<DataTable>(cacheKey); 
            //var cacheList = ETS.Cacheing.CacheFactory.Instance[cacheKey];
            if (cacheList == null)
            {
                dt = sPhoneDao.Query();
                redis.Add(cacheKey, dt, DateTime.Now.AddYears(365));//添加一年的生命周期
                //ETS.Cacheing.CacheFactory.Instance.AddObject(cacheKey, dt, DateTime.Now.AddYears(365));//添加一年的生命周期
            }
            else
            {
                dt = cacheList;
            }
            #endregion

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] rows = dt.Select("CityName='" + CityName + "'");
                if (rows.Length > 0)
                {
                    return new ResultModelServicePhone()
                    {
                        Phone = rows[0]["Phone"].ToString()
                    };
                }
            }
            tryCount++;//重试次数
            return GetCustomerServicePhone("北京市");
        }
    }
}
