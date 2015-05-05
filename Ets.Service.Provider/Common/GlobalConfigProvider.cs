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
        /// 获取全局变量表数据
        /// </summary>
        /// <returns></returns>
        public GlobalConfigModel GlobalConfigMethod()
        {
            try
            {
                return _dao.GlobalConfigMethod();
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }

       
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
                if (_dao.AddSubsidiesLog("TimeSubsidies", opName, Remark))
                {
                    result=_dao.UpdateTimeSubsidies(value);
                }
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                const string cacheKey = RedissCacheKey.Ets_Dao_GlobalConfig_GlobalConfigGet;
                redis.Delete(cacheKey);
                var model = new GlobalConfigDao().GlobalConfigMethod();
                redis.Set(cacheKey, model);
                tran.Complete(); 
            }
            return result;
        } 
      
        /// <summary>
        /// 获取全局变量表数据TimeSubsidies
        /// </summary>
        /// <returns></returns>
        public List<GlobalConfigSubsidies> GetTimeSubsidies()
        {
            var list = new List<GlobalConfigSubsidies>();
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
                            select new GlobalConfigSubsidies
                            {
                                Id = ParseHelper.ToInt(tt[2]), Value1 = tt[0], Value2 = tt[1]
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

        /// <summary>
        /// 设置全局变量表数据PriceSubsidies
        /// </summary>
        /// <param name="opName"></param>
        /// <param name="value"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public bool UpdatePriceSubsidies(string opName, string value, string Remark)
        {
            bool result = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (_dao.AddSubsidiesLog("PriceSubsidies", opName, Remark))
                {
                    result = _dao.UpdatePriceSubsidies(value);
                }
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                const string cacheKey = RedissCacheKey.Ets_Dao_GlobalConfig_GlobalConfigGet;
                redis.Delete(cacheKey);
                var model = new GlobalConfigDao().GlobalConfigMethod();
                redis.Set(cacheKey, model);
                tran.Complete();
            }
            return result;
        } 
      

        /// <summary>
        /// 获取全局变量表数据PriceSubsidies
        /// </summary>
        /// <returns></returns>
        public List<GlobalConfigSubsidies> GetPriceSubsidies()
        {
            var list = new List<GlobalConfigSubsidies>();
            try
            {
                var result = _dao.GetPriceSubsidies();
                if (!string.IsNullOrEmpty(result))
                {
                    string[] times = result.Split(';');
                    if (times.Length > 0)
                    {
                        list.AddRange(from time in times
                                      where !string.IsNullOrEmpty(time)
                                      select time.Split(',')
                                          into tt
                                          select new GlobalConfigSubsidies
                                          {
                                              Id = ParseHelper.ToInt(tt[2]),
                                              Value1 = tt[0],
                                              Value2 = tt[1]
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

        /// <summary>
        /// 获取全局变量表数据佣金方式CommissionFormulaMode
        /// </summary>
        /// <returns></returns>
        public int GetCommissionFormulaMode()
        {
            try
            {
                var result = _dao.GetCommissionFormulaMode();
                if (!string.IsNullOrEmpty(result))
                {
                    return ParseHelper.ToInt(result);
                }
            }
            catch (Exception ex)
            { 
                LogHelper.LogWriterFromFilter(ex);
                return -1;
            }
            return -1;
        }

        /// <summary>
        /// 设置全局变量表数据佣金方式CommissionFormulaMode
        /// </summary>
        /// <param name="opName"></param>
        /// <param name="value"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public bool UpdateCommissionFormulaMode(string opName, string value, string Remark)
        {
            bool result = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (_dao.AddSubsidiesLog("CommissionFormulaMode", opName, Remark))
                {
                    result = _dao.UpdateCommissionFormulaMode(value);
                }
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                const string cacheKey = RedissCacheKey.Ets_Dao_GlobalConfig_GlobalConfigGet;
                redis.Delete(cacheKey);
                var model = new GlobalConfigDao().GlobalConfigMethod();
                redis.Set(cacheKey, model);
                tran.Complete();
            }
            return result;
        }


        /// <summary>
        /// 设置全局变量表数据
        /// </summary>
        /// <param name="opName"></param>
        /// <param name="value"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public bool UpdateSubsidies(string opName, string value, string Remark,string keyName)
        {
            bool result = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (_dao.AddSubsidiesLog(keyName, opName, Remark))
                {
                    result = _dao.UpdateSubsidies(value,keyName);
                }
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                const string cacheKey = RedissCacheKey.Ets_Dao_GlobalConfig_GlobalConfigGet;
                redis.Delete(cacheKey); 
                tran.Complete();
            }
            return result;
        }


        /// <summary>
        /// 获取全局变量表数据
        /// </summary>
        /// <returns></returns>
        public string GetSubsidies(string key)
        {
            try
            {
                var result = _dao.GetSubsidies(key);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }
            catch (Exception ex)
            { 
                LogHelper.LogWriterFromFilter(ex);
                return "";
            }
            return "";
        }


        /// <summary>
        /// 获取全局变量表数据PriceSubsidies
        /// </summary>
        /// <returns></returns>
        public List<GlobalConfigSubsidies> GetOverStoreSubsidies()
        {
            var list = new List<GlobalConfigSubsidies>();
            try
            {
                var result = _dao.GetSubsidies("OverStoreSubsidies");
                if (!string.IsNullOrEmpty(result))
                {
                    string[] times = result.Split(';');
                    if (times.Length > 0)
                    {
                        list.AddRange(from time in times
                                      where !string.IsNullOrEmpty(time)
                                      select time.Split(',')
                                          into tt
                                          select new GlobalConfigSubsidies
                                          {
                                              Id = ParseHelper.ToInt(tt[2]),
                                              Value1 = tt[0],
                                              Value2 = tt[1]
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
