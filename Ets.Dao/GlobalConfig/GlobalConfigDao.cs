using ETS.Data.Core;
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
                else if(item.ItemArray[0].ToString() == "CommissionFormulaMode")
                    model.CommissionFormulaMode = ParseHelper.ToInt(item["Value"], 0);
                else if (item.ItemArray[0].ToString() == "PriceSubsidies")
                    model.PriceSubsidies = item["Value"].ToString();
                else if (item.ItemArray[0].ToString() == "PriceCommissionRatio")
                    model.PriceCommissionRatio = ParseHelper.ToDouble(item["Value"], 0);
                else if (item.ItemArray[0].ToString() == "IsStarTimeSubsidies")
                    model.IsStarTimeSubsidies = item["Value"].ToString()=="1";
                else if (item.ItemArray[0].ToString() == "PriceSiteSubsidies")
                    model.PriceSiteSubsidies = ParseHelper.ToDouble(item["Value"], 0);
                else if (item.ItemArray[0].ToString() == "CommonCommissionRatio")
                    model.CommonCommissionRatio = ParseHelper.ToDouble(item["Value"], 0);
                else if (item.ItemArray[0].ToString() == "CommonSiteSubsidies")
                    model.CommonSiteSubsidies = ParseHelper.ToDouble(item["Value"], 0);
                else if (item.ItemArray[0].ToString() == "TimeSpanCommissionRatio")
                    model.TimeSpanCommissionRatio = ParseHelper.ToDouble(item["Value"], 0);
                else if (item.ItemArray[0].ToString() == "TimeSpanInPrice")
                    model.TimeSpanInPrice = ParseHelper.ToDouble(item["Value"], 0);
                else if (item.ItemArray[0].ToString() == "TimeSpanOutPrice")
                    model.TimeSpanOutPrice = ParseHelper.ToDouble(item["Value"], 0); 
            }

            return model;
        }

        /// <summary>
        /// 获取全局变量表数据CommissionFormulaMode
        /// 平扬
        /// 2015年4月9日 11:10:49
        /// </summary>
        /// <returns></returns>
        public string GetCommissionFormulaMode()
        {
            string sql = "select Value from GlobalConfig(nolock) where keyname='CommissionFormulaMode'";
            object dt = DbHelper.ExecuteScalar(SuperMan_Read, sql);
            if (dt != null)
            {
                return dt.ToString();
            }
            return "";
        }

        /// <summary>
        /// 设置全局变量表数据CommissionFormulaMode
        /// 平扬 2015年4月8日 11:10:49
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateCommissionFormulaMode(string value)
        {
            string sql = "update GlobalConfig set Value=@Value,LastUpdateTime=getdate() where keyname='CommissionFormulaMode'";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@value", SqlDbType.NVarChar, 100);
            dbParameters.SetValue("value", value);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            return i > 0;
        } 

        /// <summary>
        /// 获取全局变量表数据PriceSubsidies
        /// 平扬
        /// 2015年4月9日 11:10:49
        /// </summary>
        /// <returns></returns>
        public string GetPriceSubsidies()
        {
            string sql = "select Value from GlobalConfig(nolock) where keyname='PriceSubsidies'";
            object dt = DbHelper.ExecuteScalar(SuperMan_Read, sql);
            if (dt != null)
            {
                return dt.ToString();
            }
            return null;
        }

        /// <summary>
        /// 设置全局变量表数据PriceSubsidies
        /// 平扬 2015年4月8日 11:10:49
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdatePriceSubsidies(string value)
        {
            string sql = "update GlobalConfig set Value=@Value,LastUpdateTime=getdate() where keyname='PriceSubsidies'";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@value", SqlDbType.NVarChar, 100);
            dbParameters.SetValue("value", value);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            return i > 0;
        } 

           
        /// <summary>
        /// 获取全局变量表数据TimeSubsidies
        /// 平扬
        /// 2015年4月7日 11:10:49
        /// </summary>
        /// <returns></returns>
        public string GetTimeSubsidies()
        {
            string sql = "select Value from GlobalConfig(nolock) where keyname='TimeSubsidies'";
            object dt = DbHelper.ExecuteScalar(SuperMan_Read, sql);
            if (dt != null)
            {
                return dt.ToString();
            }
            return null;
        }

       /// <summary>
       /// 设置全局变量表数据TimeSubsidies
       /// 平扬 2015年4月7日 11:10:49
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
        public bool UpdateTimeSubsidies(string value)
        {
            string sql = "update GlobalConfig set Value=@Value,LastUpdateTime=getdate() where keyname='TimeSubsidies'";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@value", SqlDbType.NVarChar, 100);
            dbParameters.SetValue("value", value);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters); 
            return i > 0;
        } 
      
        /// <summary>
        /// 记录日志 GlobalConfigLog数据
        /// 平扬 2015年4月7日 11:10:49
        /// </summary>
        /// <param name="keyname"></param>
        /// <param name="opName"></param>
        /// <returns></returns>
        public bool AddSubsidiesLog(string keyname, string opName, string Remark)
        {
            string sql = @"INSERT INTO GlobalConfigLog
                               (KeyName
                               ,Value
                               ,InsertTime
                               ,OptName
                               ,Remark)
                         SELECT KeyName
                              ,Value
                              ,GETDATE()
                              ,@OptName
                              ,@Remark
                         FROM GlobalConfig where keyname=@keyname;SELECT @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@keyname", SqlDbType.NVarChar, 100);
            dbParameters.SetValue("keyname", keyname);
            dbParameters.Add("@OptName", SqlDbType.NVarChar, 50);
            dbParameters.SetValue("OptName", opName);
            dbParameters.Add("@Remark", SqlDbType.NVarChar, 500);
            dbParameters.SetValue("Remark", Remark);
            object i = DbHelper.ExecuteScalar(SuperMan_Write, sql, dbParameters); 
            return ParseHelper.ToInt(i) > 0;
        }

        /// <summary>
        /// 获取全局变量表数据
        /// 平扬
        /// 2015年4月7日 11:10:49
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSubsidies(string key)
        {
            string sql = "select Value from GlobalConfig(nolock) where keyname=@keyname";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@keyname", SqlDbType.NVarChar, 100);
            dbParameters.SetValue("keyname", key);
            object dt = DbHelper.ExecuteScalar(SuperMan_Read, sql, dbParameters);
            if (dt != null)
            {
                return dt.ToString();
            }
            return "";
        }

        /// <summary>
        /// 设置全局变量表数据
        /// 平扬 2015年4月7日 11:10:49
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool UpdateSubsidies(string value,string key)
        {
            string sql = "update GlobalConfig set Value=@Value,LastUpdateTime=getdate() where keyname=@keyname";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@value", SqlDbType.NVarChar, 100);
            dbParameters.SetValue("value", value);
            dbParameters.Add("@keyname", SqlDbType.NVarChar, 100);
            dbParameters.SetValue("keyname", key);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            return i > 0;
        } 

    }
}
