using ETS.Data.PageData;
using System.Collections.Generic;
using Ets.Model.ParameterModel.Bussiness;
using System;
using Ets.Model.Common;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.DataModel.Bussiness;
using ETS.Enums;
using System.Data;
using Ets.Model.DataModel.Group;
using Ets.Model.ParameterModel.User;
using Ets.Model.ParameterModel.Order;

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
        IList<BusinessGroupModel> GetStrategyList(); 
   }
}

