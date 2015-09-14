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
using Ets.Model.ParameterModel.Business;
namespace Ets.Service.IProvider.Business
{
    /// <summary>
    /// 门店集团业务逻辑接口实现类 
    /// </summary>
    public interface IGroupBusinessProvider
    {
        void UpdateGBalance(GroupBusinessPM groupBusinessPM);
    }
}

