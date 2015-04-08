using System;
using System.Collections.Generic;
using System.Linq;
using ETS.Const;
using Ets.Dao.GlobalConfig;
using Ets.Model.DomainModel.GlobalConfig;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;

namespace Ets.Service.Provider.Common
{
    public class GlobalConfigProvider
    {
        /// <summary>
        /// 数据访问类
        /// </summary>
        readonly GlobalConfigDao _dao = new GlobalConfigDao();

       
        /// <summary>
        /// 设置全局变量表数据TimeSubsidies
        /// </summary>
        /// <param name="opName"></param>
        /// <param name="value"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public bool UpdateTimeSubsidies(string opName, string value, string Remark)
        {
            bool result = false; 
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (_dao.AddTimeSubsidiesLog("TimeSubsidies", opName, Remark))
                {
                    result=_dao.UpdateTimeSubsidies(value);
                }
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                const string cacheKey = RedissCacheKey.Ets_Dao_GlobalConfig_GlobalConfigGet;
                redis.Delete(cacheKey);
                tran.Complete(); 
            }
            return result;
        } 
      
        /// <summary>
        /// 获取全局变量表数据TimeSubsidies
        /// </summary>
        /// <returns></returns>
        public List<GlobalConfigTimeSubsidies> GetTimeSubsidies()
        {
            var list = new List<GlobalConfigTimeSubsidies>();
            try
            {
                var result = _dao.GetTimeSubsidies();
                if (!string.IsNullOrEmpty(result))
                {
                    string[] times = result.Split(';');
                    if (times.Length > 0)
                    {
                        list.AddRange(from time in times
                            where !string.IsNullOrEmpty(time)
                            select time.Split(',')
                            into tt
                            select new GlobalConfigTimeSubsidies
                            {
                                Id = ParseHelper.ToInt(tt[2]), Time =ParseHelper.ToInt(tt[0]), Price = tt[1]
                            });
                    }
                }
            }
            catch (Exception ex)
            { 
                LogHelper.LogWriterFromFilter(ex);
            } 
            return list;
        }
    }
}
