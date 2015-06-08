using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Bussiness;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Service.IProvider.Bussiness;

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
    }
}
