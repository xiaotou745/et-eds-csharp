using Ets.Model.DomainModel.GlobalConfig;
using ETS.Const;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.GlobalConfig
{
    public class GlobalConfigDao : ETS.Dao.DaoBase
    {
        private static string CurrentGlobalVersion = string.Empty;
        /// <summary>
        /// 获取全局变量表数据,已加入缓存
        /// 窦海超
        /// 2015年4月2日 13:15:02
        /// </summary>
        public static GlobalConfigModel GlobalConfigGet
        {
            get
            {
                GlobalConfigModel model = null;
                #region redis判断，如果没有加到redis中
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                const string cacheKey = RedissCacheKey.Ets_Dao_GlobalConfig_GlobalConfigGet;
                model = redis.Get<GlobalConfigModel>(cacheKey);
                if (CurrentGlobalVersion != ETS.Config.GlobalVersion || model == null)
                {
                    CurrentGlobalVersion = ETS.Config.GlobalVersion;
                    model = new GlobalConfigDao().GlobalConfigMethod();
                    redis.Set(cacheKey, model);
                }
                #endregion

                return model;
            }
        }

        /// <summary>
        /// 获取全局变量表数据，此方法需要做优化，由于时间紧做了临时的负值
        /// 窦海超
        /// 2015年4月2日 13:10:49
        /// </summary>
        /// <returns></returns>
        public GlobalConfigModel GlobalConfigMethod()
        {
            //这里允许用*是因为配置是需要全部加载
            string sql = "select KeyName,Value from GlobalConfig(nolock) order by id desc";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            DataRowCollection rows = dt.Rows;
            GlobalConfigModel model = new GlobalConfigModel();
            foreach (DataRow item in dt.Rows)
            {
                if (item.ItemArray[0].ToString() == "CommissionRatio")
                {
                    model.CommissionRatio = ParseHelper.ToDouble(item["Value"], 0);
                }
                else if (item.ItemArray[0].ToString() == "SiteSubsidies")
                {
                    model.SiteSubsidies = ParseHelper.ToDouble(item["Value"], 0);
                }
                else if (item.ItemArray[0].ToString() == "TimeSubsidies")
                {
                    model.TimeSubsidies = item["Value"].ToString();
                }
            }

            return model;
        }
    }
}
