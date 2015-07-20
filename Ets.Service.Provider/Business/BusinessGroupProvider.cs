using ETS.Const;
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
                }
                tran.Complete();
                DeleteGlobalConfigRedisByGroupId(globalConfigModel.GroupId);
                return true;
            }
        }
    }
}
