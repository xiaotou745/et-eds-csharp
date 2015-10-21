﻿using ETS.Const;
using Ets.Dao.User;
using Ets.Model.Common;
using System.Collections.Generic;
using Ets.Model.DataModel.Business;
using ETS.Transaction.Common;
using ETS.Transaction;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.IProvider.Business;
using Ets.Dao.Business;
using ETS.Util;

namespace Ets.Service.Provider.Business
{
    /// <summary>
    /// 商家分组业务逻辑接口实现类 
    /// </summary>
    public class BusinessGroupProvider : IBusinessGroupProvider
    {
        private readonly BusinessGroupDao businessGroupDao = new BusinessGroupDao();

        /// <summary>
        /// 获取商家分组列表
        /// 胡灵波-20150504
        /// </summary>
        /// <returns></returns>
        public IList<BusinessGroupModel> GetBusinessGroupList()
        {
            return businessGroupDao.GetBusinessGroupList();
        }

        public BusinessGroupModel GetCurrenBusinessGroup(int businessId)
        {
            return businessGroupDao.GetCurrenBusinessGroup(businessId);
        }

        /// <summary>
        /// 添加商家分组
        /// danny-20150506
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifySubsidyFormulaMode(GlobalConfigModel globalConfigModel)
        {

            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (globalConfigModel.GroupId == 0) //新增
                {
                    BusinessGroupModel businessGroupModel = new BusinessGroupModel()
                    {
                        Name = globalConfigModel.GroupName,
                        StrategyId = globalConfigModel.StrategyId,
                        CreateBy = globalConfigModel.OptName
                    };
                    globalConfigModel.GroupId = businessGroupDao.AddBusinessGroup(businessGroupModel);
                    if (globalConfigModel.GroupId > 0)
                    {
                        var r = businessGroupDao.CopyGlobalConfigMode(globalConfigModel.GroupId,
                            globalConfigModel.OptName);
                    }
                }
                else //修改
                {
                    BusinessGroupModel businessGroupModel = new BusinessGroupModel()
                    {
                        Id = globalConfigModel.GroupId,
                        Name = globalConfigModel.GroupName,
                        StrategyId = globalConfigModel.StrategyId,
                        UpdateBy = globalConfigModel.OptName

                    };
                    businessGroupDao.UpdateBusinessGroup(businessGroupModel);
                }

                #region 动态时间补贴

                Ets.Model.Common.GlobalConfig globalConfig = new Ets.Model.Common.GlobalConfig()
                {
                    KeyName = "IsStarTimeSubsidies",
                    Value = globalConfigModel.IsStarTimeSubsidies,
                    GroupId = globalConfigModel.GroupId,
                    StrategyId = globalConfigModel.StrategyId,
                    OptName = globalConfigModel.OptName
                };
                businessGroupDao.UpdateGlobalConfig(globalConfig);

                #endregion

                #region 跨店时间奖励

                globalConfig.KeyName = "IsStartOverStoreSubsidies";
                globalConfig.Value = globalConfigModel.IsStartOverStoreSubsidies;
                businessGroupDao.UpdateGlobalConfig(globalConfig);

                #endregion

                #region 补贴策略

                switch (globalConfigModel.StrategyId) //使用switch-case开关语句，根据按键次数执行相应分支
                {
                    case 0: //普通补贴
                        globalConfig.KeyName = "CommonCommissionRatio";
                        globalConfig.Value = globalConfigModel.CommonCommissionRatio;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "CommonSiteSubsidies";
                        globalConfig.Value = globalConfigModel.CommonSiteSubsidies;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        break;
                    case 1: //时间段补贴
                        globalConfig.KeyName = "TimeSpanCommissionRatio";
                        globalConfig.Value = globalConfigModel.TimeSpanCommissionRatio;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "TimeSpanInPrice";
                        globalConfig.Value = globalConfigModel.TimeSpanInPrice;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "TimeSpanOutPrice";
                        globalConfig.Value = globalConfigModel.TimeSpanOutPrice;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        break;
                    case 2: //保本补贴
                        globalConfig.KeyName = "CommissionRatio";
                        globalConfig.Value = globalConfigModel.CommissionRatio;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "SiteSubsidies";
                        globalConfig.Value = globalConfigModel.SiteSubsidies;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        break;
                    case 3: //满金额补贴
                        globalConfig.KeyName = "PriceCommissionRatio";
                        globalConfig.Value = globalConfigModel.PriceCommissionRatio;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "PriceSiteSubsidies";
                        globalConfig.Value = globalConfigModel.PriceSiteSubsidies;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        break;
                    case 4: //基本佣金补贴
                        globalConfig.KeyName = "BaseCommission";
                        globalConfig.Value = globalConfigModel.BaseCommission;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "BaseSiteSubsidies";
                        globalConfig.Value = globalConfigModel.BaseSiteSubsidies;
                        businessGroupDao.UpdateGlobalConfig(globalConfig);
                        break;
                }

                #endregion

                DeleteGlobalConfigRedisByGroupId(globalConfigModel.GroupId);
                tran.Complete();

            }
            return true;

        }

        /// <summary>
        /// 根据分组Id删除公共配置缓存
        /// danny-20150506
        /// </summary>
        /// <param name="GroupId"></param>
        private void DeleteGlobalConfigRedisByGroupId(int GroupId)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string cacheKey = string.Format(RedissCacheKey.Ets_Dao_GlobalConfig_GlobalConfigGet, GroupId); //缓存的KEY
            redis.Delete(cacheKey);
        }

        /// <summary>
        /// 修改公共配置信息
        /// danny-20150518
        /// </summary>
        /// <param name="globalConfigModel"></param>
        /// <returns></returns>
        public bool ModifyGlobalConfig(GlobalConfigModel globalConfigModel)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCachePublic();            

            var globalConfig = new Ets.Model.Common.GlobalConfig()
            {
                OptName = globalConfigModel.OptName,
                GroupId = 0,
                StrategyId = -1
            };
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                bool reg;
                if (globalConfigModel.PushRadius != "0")
                {
                    globalConfig.KeyName = "PushRadius";
                    globalConfig.Value = globalConfigModel.PushRadius;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_PushRadius, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.PushRadius);

                }
                if (globalConfigModel.AllFinishedOrderUploadTimeInterval != "0")
                {
                    globalConfig.KeyName = "AllFinishedOrderUploadTimeInterval";
                    globalConfig.Value = globalConfigModel.AllFinishedOrderUploadTimeInterval;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_AllFinishedOrderUploadTimeInterval, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.AllFinishedOrderUploadTimeInterval);
                }
                if (globalConfigModel.SearchClienterLocationTimeInterval != "0")
                {
                    globalConfig.KeyName = "SearchClienterLocationTimeInterval";
                    globalConfig.Value = globalConfigModel.SearchClienterLocationTimeInterval;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_SearchClienterLocationTimeInterval, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.SearchClienterLocationTimeInterval);
                }
                if (globalConfigModel.HasUnFinishedOrderUploadTimeInterval != "0")
                {
                    globalConfig.KeyName = "HasUnFinishedOrderUploadTimeInterval";
                    globalConfig.Value = globalConfigModel.HasUnFinishedOrderUploadTimeInterval;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_HasUnFinishedOrderUploadTimeInterval, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.HasUnFinishedOrderUploadTimeInterval);
                }
                if (globalConfigModel.BusinessUploadTimeInterval != "0")
                {
                    globalConfig.KeyName = "BusinessUploadTimeInterval";
                    globalConfig.Value = globalConfigModel.BusinessUploadTimeInterval;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_BusinessUploadTimeInterval, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.BusinessUploadTimeInterval);
                }
                if (globalConfigModel.ClienterWithdrawCommissionAccordingMoney != "0")
                {
                    globalConfig.KeyName = "ClienterWithdrawCommissionAccordingMoney";
                    globalConfig.Value = globalConfigModel.ClienterWithdrawCommissionAccordingMoney;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_ClienterWithdrawCommissionAccordingMoney, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.ClienterWithdrawCommissionAccordingMoney);
                }
                if (globalConfigModel.ExclusiveOrderTime != "0")
                {
                    globalConfig.KeyName = "ExclusiveOrderTime";
                    globalConfig.Value = globalConfigModel.ExclusiveOrderTime;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_ExclusiveOrderTime, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.ExclusiveOrderTime);
                }
                if (globalConfigModel.ClienterOrderPageSize != "0")
                {
                    globalConfig.KeyName = "ClienterOrderPageSize";
                    globalConfig.Value = globalConfigModel.ClienterOrderPageSize;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_ClienterOrderPageSize, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.ClienterOrderPageSize);
                }
                if (ParseHelper.ToInt(globalConfigModel.CompleteTimeSet) >= 0)
                {
                    globalConfig.KeyName = "CompleteTimeSet";
                    globalConfig.Value = globalConfigModel.CompleteTimeSet;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_CompleteTimeSet, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.CompleteTimeSet);
                }
                if (!string.IsNullOrEmpty(globalConfigModel.EmployerTaskTimeSet))
                {
                    globalConfig.KeyName = "EmployerTaskTimeSet";
                    globalConfig.Value = globalConfigModel.EmployerTaskTimeSet;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_EmployerTaskTimeSet, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.EmployerTaskTimeSet);
                }
                if (!string.IsNullOrWhiteSpace(globalConfigModel.WithdrawCommission))
                {
                    globalConfig.KeyName = "WithdrawCommission";
                    globalConfig.Value = globalConfigModel.WithdrawCommission;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_WithdrawCommission, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.WithdrawCommission);
                }
                if (ParseHelper.ToInt(globalConfigModel.OrderCountSetting) >= 0)
                {
                    globalConfig.KeyName = "OrderCountSetting";
                    globalConfig.Value = globalConfigModel.OrderCountSetting;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_OrderCountSetting, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.OrderCountSetting);
                }
                if (ParseHelper.ToInt(globalConfigModel.GrabToCompleteDistance) >= 0)
                {
                    globalConfig.KeyName = "GrabToCompleteDistance";
                    globalConfig.Value = globalConfigModel.GrabToCompleteDistance;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_GrabToCompleteDistance, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.GrabToCompleteDistance);
                }
                if (ParseHelper.ToInt(globalConfigModel.YeepayWithdrawCommission) >= 0)
                {
                    globalConfig.KeyName = "YeepayWithdrawCommission";
                    globalConfig.Value = globalConfigModel.YeepayWithdrawCommission;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_YeepayWithdrawCommission, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.YeepayWithdrawCommission);
                }
                if (ParseHelper.ToInt(globalConfigModel.AlipayWithdrawCommission) >= 0)
                {
                    globalConfig.KeyName = "AlipayWithdrawCommission";
                    globalConfig.Value = globalConfigModel.AlipayWithdrawCommission;
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_AlipayWithdrawCommission, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.AlipayWithdrawCommission);
                }
                if (!string.IsNullOrWhiteSpace(globalConfigModel.AlipayPassword))
                {
                    globalConfig.KeyName = "AlipayPassword";
                    globalConfig.Value = ParseHelper.ToAesEncrypt(globalConfigModel.AlipayPassword);
                    reg = businessGroupDao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                    string cacheKey = string.Format(RedissCacheKey.GlobalConfig_AlipayPassword, globalConfigModel.GroupId);
                    redis.Set(cacheKey, globalConfigModel.AlipayPassword);
                }
                tran.Complete();
                //DeleteGlobalConfigRedisByGroupId(globalConfigModel.GroupId);
                return true;
            }
        }
    }
}
