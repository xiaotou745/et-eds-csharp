using ETS.Data.PageData;
using System.Collections.Generic;
using Ets.Model.ParameterModel.Business;
using System;
using Ets.Model.Common;
using Ets.Model.DomainModel.Business;
using Ets.Model.DataModel.Business;
using ETS.Enums;
using System.Data;
using Ets.Model.DataModel.Group;
using Ets.Model.ParameterModel.User;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DomainModel.GlobalConfig;

namespace Ets.Service.IProvider.User
{
    /// <summary>
    /// 商家分组业务逻辑接口实现类 
    /// </summary>
    public interface IBusinessGroupProvider
    {
        /// <summary>
        /// 获取商家分组列表
        /// 胡灵波-20150504
        /// </summary>
        /// <returns></returns>
        IList<BusinessGroupModel> GetBusinessGroupList();

        BusinessGroupModel GetCurrenBusinessGroup(int businessId);   
        /// <summary>
        /// 修改补贴策略
        /// danny-20150506
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool ModifySubsidyFormulaMode(GlobalConfigModel globalConfigModel);
        /// <summary>
        /// 修改公共配置信息
        /// danny-20150518
        /// </summary>
        /// <param name="globalConfigModel"></param>
        /// <returns></returns>
        bool ModifyGlobalConfig(GlobalConfigModel globalConfigModel);
    }
}

