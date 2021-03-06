﻿using System.Data;
using System.Text;
using ETS.Const;
using Ets.Dao.User;
using Ets.Model.Common;
using Ets.Model.DomainModel.Bussiness;
using Ets.Service.IProvider.User;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using CalculateCommon;
using Ets.Model.ParameterModel.Bussiness;
using System.Linq;
using ETS.Enums;
using Ets.Model.DataModel.Bussiness;
using ETS.Util;
using ETS.Cacheing;
using Ets.Model.DataModel.Group;
using ETS.Validator;
using ETS;
using System.Threading.Tasks;
using ETS.Sms;
using ETS.Transaction.Common;
using ETS.Transaction;
using Ets.Model.ParameterModel.User;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DomainModel.GlobalConfig;
namespace Ets.Service.Provider.User
{
    /// <summary>
    /// 商家分组业务逻辑接口实现类 
    /// </summary>
    public class BusinessGroupProvider : IBusinessGroupProvider
    {
        private BusinessGroupDao dao = new BusinessGroupDao();

        /// <summary>
        /// 获取商家分组列表
        /// 胡灵波-20150504
        /// </summary>
        /// <returns></returns>
        public IList<BusinessGroupModel> GetBusinessGroupList()
        {
            return dao.GetBusinessGroupList();
        }

        public BusinessGroupModel GetCurrenBusinessGroup(int businessId)
        {
            return dao.GetCurrenBusinessGroup(businessId);
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
                    globalConfigModel.GroupId = dao.AddBusinessGroup(businessGroupModel);
                    if (globalConfigModel.GroupId > 0)
                    {
                        var r = dao.CopyGlobalConfigMode(globalConfigModel.GroupId, globalConfigModel.OptName);
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
                    dao.UpdateBusinessGroup(businessGroupModel);
                }

                #region 动态时间补贴

                GlobalConfig globalConfig = new GlobalConfig()
                {
                    KeyName = "IsStarTimeSubsidies",
                    Value = globalConfigModel.IsStarTimeSubsidies,
                    GroupId = globalConfigModel.GroupId,
                    StrategyId = globalConfigModel.StrategyId,
                    OptName = globalConfigModel.OptName
                };
                dao.UpdateGlobalConfig(globalConfig);

                #endregion

                #region 跨店时间奖励

                globalConfig.KeyName = "IsStartOverStoreSubsidies";
                globalConfig.Value = globalConfigModel.IsStartOverStoreSubsidies;
                dao.UpdateGlobalConfig(globalConfig);

                #endregion

                #region 补贴策略

                switch (globalConfigModel.StrategyId) //使用switch-case开关语句，根据按键次数执行相应分支
                {
                    case 0: //普通补贴
                        globalConfig.KeyName = "CommonCommissionRatio";
                        globalConfig.Value = globalConfigModel.CommonCommissionRatio;
                        dao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "CommonSiteSubsidies";
                        globalConfig.Value = globalConfigModel.CommonSiteSubsidies;
                        dao.UpdateGlobalConfig(globalConfig);
                        break;
                    case 1: //时间段补贴
                        globalConfig.KeyName = "TimeSpanCommissionRatio";
                        globalConfig.Value = globalConfigModel.TimeSpanCommissionRatio;
                        dao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "TimeSpanInPrice";
                        globalConfig.Value = globalConfigModel.TimeSpanInPrice;
                        dao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "TimeSpanOutPrice";
                        globalConfig.Value = globalConfigModel.TimeSpanOutPrice;
                        dao.UpdateGlobalConfig(globalConfig);
                        break;
                    case 2: //保本补贴
                        globalConfig.KeyName = "CommissionRatio";
                        globalConfig.Value = globalConfigModel.CommissionRatio;
                        dao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "SiteSubsidies";
                        globalConfig.Value = globalConfigModel.SiteSubsidies;
                        dao.UpdateGlobalConfig(globalConfig);
                        break;
                    case 3: //满金额补贴
                        globalConfig.KeyName = "PriceCommissionRatio";
                        globalConfig.Value = globalConfigModel.PriceCommissionRatio;
                        dao.UpdateGlobalConfig(globalConfig);
                        globalConfig.KeyName = "PriceSiteSubsidies";
                        globalConfig.Value = globalConfigModel.PriceSiteSubsidies;
                        dao.UpdateGlobalConfig(globalConfig);
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
            var globalConfig = new GlobalConfig()
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
                    reg = dao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                }
                if (globalConfigModel.UploadTimeInterval != "0")
                {
                    globalConfig.KeyName = "UploadTimeInterval";
                    globalConfig.Value = globalConfigModel.UploadTimeInterval;
                    reg = dao.UpdateGlobalConfig(globalConfig);
                    if (!reg)
                    {
                        return false;
                    }
                }
                if (globalConfigModel.ClienterOrderPageSize != "0")
                {
                    globalConfig.KeyName = "ClienterOrderPageSize";
                    globalConfig.Value = globalConfigModel.ClienterOrderPageSize;
                    reg = dao.UpdateGlobalConfig(globalConfig);
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