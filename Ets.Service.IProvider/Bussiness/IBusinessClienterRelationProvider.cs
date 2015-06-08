using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.Bussiness;

namespace Ets.Service.IProvider.Bussiness
{
    /// <summary>
    /// 商户绑定骑士关系接口caoheyang 20150608
    /// </summary>
    public interface IBusinessClienterRelationProvider
    {
        /// <summary> 
        /// 根据骑士id查询骑士绑定商家列表   caoheyang 20150608
        /// </summary>
        PageInfo<BCRelationGetByClienterIdDM> GetByClienterId(BCRelationGetByClienterIdPM model);
    }
}
