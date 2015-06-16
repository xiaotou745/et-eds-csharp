using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Bussiness;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.Business;
using Ets.Service.IProvider.Bussiness;
using Ets.Model.DataModel.Business;
using Ets.Model.ParameterModel.Order;

namespace Ets.Service.Provider.Bussiness
{
    /// <summary>
    /// 商户绑定骑士关系   caoheyang 20150608
    /// </summary>
   public class BusinessClienterRelationProvider : IBusinessClienterRelationProvider
    {
        private readonly BusinessClienterRelationDao businessClienterRelationDao = new BusinessClienterRelationDao();
       /// <summary> 
       /// 根据骑士id查询骑士绑定商家列表   caoheyang 20150608
       /// </summary>
       public PageInfo<BCRelationGetByClienterIdDM> GetByClienterId(BCRelationGetByClienterIdPM model)
       {
           return businessClienterRelationDao.GetByClienterId(model);
       }


       /// <summary>
       /// 增加一条记录
       /// </summary>
       /// <UpdateBy>hulingbo</UpdateBy>
       /// <UpdateTime>20150609</UpdateTime>
       /// <param name="model"></param>
       /// <returns></returns>      
       public int Create(BusinessClienterRelation businessClienterRelation)
       {
           return businessClienterRelationDao.Insert(businessClienterRelation);
       }

       /// <summary>
       /// 更新一条记录
       /// </summary>
       /// <UpdateBy>hulingbo</UpdateBy>
       /// <UpdateTime>20150609</UpdateTime>
       /// <param name="model"></param>
       /// <returns></returns>      
       public void Modify(BusinessClienterRelationPM businessClienterRelation)
       {
            businessClienterRelationDao.Update(businessClienterRelation);
       }

       /// <summary>
       /// 获取商户骑士对应关系     
       /// </summary>
       /// <UpdateBy>hulingbo</UpdateBy>
       /// <UpdateTime>20150609</UpdateTime>  
       /// <param name="model"></param>
       /// <returns></returns>   
       public BusinessClienterRelation GetDetails(BusinessClienterRelationPM businessClienterRelation)
       {
           return businessClienterRelationDao.GetDetails(businessClienterRelation); 
       }
    }
}
