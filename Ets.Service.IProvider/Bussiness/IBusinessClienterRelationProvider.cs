using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.DataModel.Bussiness;
using Ets.Model.ParameterModel.Order;

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

        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150609</UpdateTime>
        /// <param name="model"></param>
        /// <returns></returns>      
        int Create(BusinessClienterRelation businessClienterRelation);

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150609</UpdateTime>
        /// <param name="model"></param>
        /// <returns></returns>      
        void Modify(BusinessClienterRelationPM businessClienterRelation);

        /// <summary>
        /// 获取商户骑士对应关系     
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150609</UpdateTime>  
        /// <param name="model"></param>
        /// <returns></returns>   
        BusinessClienterRelation GetDetails(BusinessClienterRelationPM businessClienterRelation);

    }
}
