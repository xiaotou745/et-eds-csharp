using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Extension;
using Ets.Model.DataModel.Bussiness;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.Bussiness;

namespace Ets.Dao.Bussiness
{
    /// <summary>
    /// 商户绑定骑士关系dao  caoheyang 20150608
    /// </summary>
    public class BusinessClienterRelationDao : DaoBase
    {
        /// <summary> 
        /// 根据骑士id查询骑士绑定商家列表   caoheyang 20150608
        /// </summary>
        public PageInfo<BCRelationGetByClienterIdDM> GetByClienterId(BCRelationGetByClienterIdPM model)
        {
            string where = string.Format(" ClienterId={0}", model.ClienterId);
            return new PageHelper().GetPages<BCRelationGetByClienterIdDM>(SuperMan_Read, model.PageIndex, where, "a.Id desc", "a.BusinessId,a.ClienterId,b.Name as BusinessName,b.PhoneNo as BusinessPhoneNo,b.Address as BusinessAddress,a.IsBind,a.UpdateBy,a.UpdateTime,c.TrueName as ClienterName",
                "dbo.BusinessClienterRelation a ( nolock )  join dbo.business b ( nolock ) on a.BusinessId = b.Id join dbo.clienter c ( nolock ) on c.Id = a.ClienterId", SystemConst.PageSize, true);
        }
    }
}
