﻿using ETS.Data.Core;
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
        //private static string CurrentGlobalVersion = string.Empty;
        /// <summary>
        /// 获取全局变量表数据,已加入缓存
        /// 窦海超
        /// 2015年4月2日 13:15:02
        /// </summary>
        public static GlobalConfigModel GlobalConfigGet(int GroupId)
        {
            GlobalConfigModel model = new GlobalConfigModel();
            model = new GlobalConfigDao().GlobalConfigMethod(GroupId);
            //#region redis判断，如果没有加到redis中
            //var redis = new ETS.NoSql.RedisCache.RedisCache();
            //string cacheKey =string.Format(RedissCacheKey.Ets_Dao_GlobalConfig_GlobalConfigGet, GroupId);//缓存的KEY
            //model = redis.Get<GlobalConfigModel>(cacheKey);
            //if (model == null)
            //{
            //    //CurrentGlobalVersion = ETS.Config.GlobalVersion;
            //    model = new GlobalConfigDao().GlobalConfigMethod(GroupId);
            //    redis.Set(cacheKey, model);
            //}


            #region redis判断，如果没有加到redis中              
           
            var redis = new ETS.NoSql.RedisCache.RedisCachePublic();
            string PushRadius= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_PushRadius,GroupId));
            string AllFinishedOrderUploadTimeInterval= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_AllFinishedOrderUploadTimeInterval,GroupId));
            string SearchClienterLocationTimeInterval= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_SearchClienterLocationTimeInterval,GroupId));
            string HasUnFinishedOrderUploadTimeInterval= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_HasUnFinishedOrderUploadTimeInterval,GroupId));
            string BusinessUploadTimeInterval= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_BusinessUploadTimeInterval,GroupId));
            string ClienterWithdrawCommissionAccordingMoney= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_ClienterWithdrawCommissionAccordingMoney,GroupId));
            string ExclusiveOrderTime= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_ExclusiveOrderTime,GroupId));
            string ClienterOrderPageSize= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_ClienterOrderPageSize,GroupId));
            string CompleteTimeSet= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_CompleteTimeSet,GroupId));
            string EmployerTaskTimeSet= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_EmployerTaskTimeSet,GroupId));
            string WithdrawCommission= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_WithdrawCommission,GroupId));
            string OrderCountSetting= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_OrderCountSetting,GroupId));
            string YeepayWithdrawCommission= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_YeepayWithdrawCommission,GroupId));
            string GrabToCompleteDistance= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_GrabToCompleteDistance,GroupId));
            string AlipayWithdrawCommission= redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_AlipayWithdrawCommission,GroupId));
            string AlipayPassword = redis.Get<string>(string.Format(RedissCacheKey.GlobalConfig_AlipayPassword, GroupId));
            if (string.IsNullOrEmpty(PushRadius))
            {
                

                string value= new GlobalConfigDao().GetSubsidies("PushRadius", GroupId);
                if(value!=null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_PushRadius, GroupId), value);
            }
            else
            {
                model.PushRadius = PushRadius;
            }
            if (string.IsNullOrEmpty(AllFinishedOrderUploadTimeInterval))
            {
                string value= new GlobalConfigDao().GetSubsidies("AllFinishedOrderUploadTimeInterval", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_AllFinishedOrderUploadTimeInterval,GroupId), value);
            }
            else
            {
                model.AllFinishedOrderUploadTimeInterval=AllFinishedOrderUploadTimeInterval;
            }
            if(string.IsNullOrEmpty(SearchClienterLocationTimeInterval))
            {
                string value= new GlobalConfigDao().GetSubsidies("SearchClienterLocationTimeInterval", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_SearchClienterLocationTimeInterval,GroupId), value);
            }
            else
            {
                model.SearchClienterLocationTimeInterval = SearchClienterLocationTimeInterval;
            }
            if (string.IsNullOrEmpty(HasUnFinishedOrderUploadTimeInterval))
            {
                string value= new GlobalConfigDao().GetSubsidies("HasUnFinishedOrderUploadTimeInterval", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_HasUnFinishedOrderUploadTimeInterval,GroupId), value);
            }
            else
            {
                model.HasUnFinishedOrderUploadTimeInterval = HasUnFinishedOrderUploadTimeInterval;
            }
            if(string.IsNullOrEmpty(BusinessUploadTimeInterval))
            {
                string value= new GlobalConfigDao().GetSubsidies("BusinessUploadTimeInterval", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_BusinessUploadTimeInterval,GroupId), value);
            }
            else
            {
                model.BusinessUploadTimeInterval = BusinessUploadTimeInterval;
            }
            if(string.IsNullOrEmpty(ClienterWithdrawCommissionAccordingMoney))
            {
                string value= new GlobalConfigDao().GetSubsidies("ClienterWithdrawCommissionAccordingMoney", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_ClienterWithdrawCommissionAccordingMoney,GroupId), value);
            }
            else
            {
                model.ClienterWithdrawCommissionAccordingMoney =ClienterWithdrawCommissionAccordingMoney;
            }
            if(string.IsNullOrEmpty(ExclusiveOrderTime))
            {
                string value= new GlobalConfigDao().GetSubsidies("ExclusiveOrderTime", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_ExclusiveOrderTime,GroupId), value);
            }
            else
            {
                model.ExclusiveOrderTime = ExclusiveOrderTime;
            }
            if (string.IsNullOrEmpty(ClienterOrderPageSize))
            {
                string value = new GlobalConfigDao().GetSubsidies("ClienterOrderPageSize", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_ClienterOrderPageSize, GroupId), value);
            }
            else
            {
                model.ClienterOrderPageSize = ClienterOrderPageSize;
            }            
            if (string.IsNullOrEmpty(CompleteTimeSet))
            {
                string value = new GlobalConfigDao().GetSubsidies("CompleteTimeSet", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_CompleteTimeSet, GroupId), value);
            }
            else
            {
                model.CompleteTimeSet = CompleteTimeSet;
            }            
            if (string.IsNullOrEmpty(EmployerTaskTimeSet))
            {
                string value = new GlobalConfigDao().GetSubsidies("EmployerTaskTimeSet", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_EmployerTaskTimeSet, GroupId), value);
            }
            else
            {
                model.EmployerTaskTimeSet = EmployerTaskTimeSet;
            }            
            if (string.IsNullOrEmpty(WithdrawCommission))
            {
                string value = new GlobalConfigDao().GetSubsidies("WithdrawCommission", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_WithdrawCommission, GroupId), value);
            }
            else
            {
                model.WithdrawCommission = WithdrawCommission;
            }            
            if (string.IsNullOrEmpty(OrderCountSetting))
            {
                string value = new GlobalConfigDao().GetSubsidies("OrderCountSetting", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_OrderCountSetting, GroupId), value);
            }
            else
            {
                model.OrderCountSetting = OrderCountSetting;
            }            
            if (string.IsNullOrEmpty(YeepayWithdrawCommission))
            {
                string value = new GlobalConfigDao().GetSubsidies("YeepayWithdrawCommission", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_YeepayWithdrawCommission, GroupId), value);
            }
            else
            {
                model.YeepayWithdrawCommission = YeepayWithdrawCommission;
            }            
            if (string.IsNullOrEmpty(GrabToCompleteDistance))
            {
                string value = new GlobalConfigDao().GetSubsidies("GrabToCompleteDistance", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_GrabToCompleteDistance, GroupId), value);
            }
            else
            {
                model.GrabToCompleteDistance = GrabToCompleteDistance;
            }            
            if (string.IsNullOrEmpty(AlipayWithdrawCommission))
            {
                string value = new GlobalConfigDao().GetSubsidies("AlipayWithdrawCommission", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_AlipayWithdrawCommission, GroupId), value);
            }
            else
            {
                model.AlipayWithdrawCommission = AlipayWithdrawCommission;
            }
            if (string.IsNullOrEmpty(AlipayPassword))
            {
                string value = new GlobalConfigDao().GetSubsidies("AlipayPassword", GroupId);
                if (value != null)
                redis.Set(string.Format(RedissCacheKey.GlobalConfig_AlipayPassword, GroupId), value);
            }
            else
            {
                model.AlipayPassword = AlipayPassword;
            }                       
        
            #endregion

            return model;
        }

        /// <summary>
        /// 获取全局变量表数据，此方法需要做优化，由于时间紧做了临时的负值
        /// 窦海超
        /// 2015年4月2日 13:10:49
        /// </summary>
        /// <returns></returns>
        public GlobalConfigModel GlobalConfigMethod(int GroupId)
        {
            //这里允许用*是因为配置是需要全部加载
            IDbParameters parms = DbHelper.CreateDbParameters();
            parms.AddWithValue("@GroupId", GroupId);
            DataTable dtGlobal = DbHelper.StoredExecuteDataTable(SuperMan_Read, "SP_get_GlobalGroupConfig", parms);
            if (dtGlobal == null || dtGlobal.Rows.Count <= 0)
            {
                return null;
            }
            return ConvertDataTableList<GlobalConfigModel>(dtGlobal)[0];
        }

        /// <summary>
        /// 获取全局变量表数据CommissionFormulaMode
        /// 平扬
        /// 2015年4月9日 11:10:49
        /// </summary>
        /// <returns></returns>
        public string GetCommissionFormulaMode(int GroupId)
        {
            string sql = "select Value from GlobalConfig(nolock) where keyname='CommissionFormulaMode'and GroupId=" + GroupId;
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
        public bool UpdateCommissionFormulaMode(string value, int GroupId)
        {
            string sql = "update GlobalConfig set Value=@Value,LastUpdateTime=getdate() where keyname='CommissionFormulaMode' and GroupId=" + GroupId;
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@value", SqlDbType.NVarChar, 500);
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
        public string GetPriceSubsidies(int GroupId)
        {
            string sql = "select Value from GlobalConfig(nolock) where keyname='PriceSubsidies' and GroupId=" + GroupId;
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
        public bool UpdatePriceSubsidies(string value, int GroupId)
        {
            string sql = "update GlobalConfig set Value=@Value,LastUpdateTime=getdate() where keyname='PriceSubsidies' and GroupId=" + GroupId;
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@value", SqlDbType.NVarChar, 500);
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
        public string GetTimeSubsidies(int GroupId)
        {
            string sql = "select Value from GlobalConfig(nolock) where keyname='TimeSubsidies' and GroupId=" + GroupId;
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
        public bool UpdateTimeSubsidies(string value, int GroupId)
        {
            string sql = "update GlobalConfig set Value=@Value,LastUpdateTime=getdate() where keyname='TimeSubsidies' and GroupId=" + GroupId;
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@value", SqlDbType.NVarChar, 500);
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
        public bool AddSubsidiesLog(string keyname, string opName, string Remark, int GroupId, int StrategyId)
        {
            string sql = @"INSERT INTO GlobalConfigLog
                               (KeyName
                               ,Value
                               ,InsertTime
                               ,OptName
                               ,Remark
                               ,GroupId
                               ,StrategyId
                            )
                         SELECT KeyName
                              ,Value
                              ,GETDATE()
                              ,@OptName
                              ,@Remark
                              ,@GroupId
                              ,@StrategyId
                         FROM GlobalConfig where keyname=@keyname;SELECT @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@keyname", SqlDbType.NVarChar, 100);
            dbParameters.SetValue("keyname", keyname);
            dbParameters.Add("@OptName", SqlDbType.NVarChar, 50);
            dbParameters.SetValue("OptName", opName);
            dbParameters.Add("@Remark", SqlDbType.NVarChar, 500);
            dbParameters.SetValue("Remark", Remark);
            dbParameters.Add("@GroupId", SqlDbType.Int);
            dbParameters.SetValue("GroupId", GroupId);
            dbParameters.Add("@StrategyId", SqlDbType.Int);
            dbParameters.SetValue("StrategyId", GroupId);
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
        public string GetSubsidies(string key, int GroupId)
        {
            string sql = "select Value from GlobalConfig(nolock) where keyname=@keyname and GroupId=@GroupId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@keyname", SqlDbType.NVarChar, 100);
            dbParameters.SetValue("keyname", key);
            dbParameters.Add("@GroupId", SqlDbType.Int);
            dbParameters.SetValue("GroupId", GroupId);
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
        public bool UpdateSubsidies(string value, string key, int GroupId)
        {
            string sql = "update GlobalConfig set Value=@Value,LastUpdateTime=getdate() where keyname=@keyname  and GroupId=@GroupId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@value", SqlDbType.NVarChar, 500);
            dbParameters.SetValue("value", value);
            dbParameters.Add("@keyname", SqlDbType.NVarChar, 100);
            dbParameters.SetValue("keyname", key);
            dbParameters.Add("@GroupId", SqlDbType.Int);
            dbParameters.SetValue("GroupId", GroupId);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            return i > 0;
        }

    }
}
