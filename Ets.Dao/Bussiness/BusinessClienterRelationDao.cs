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
            string where = string.Format(" ClienterId=", model.ClienterId);
            return new PageHelper().GetPages<BCRelationGetByClienterIdDM>(SuperMan_Read, model.PageIndex, where, "Id desc", " BusinessId,ClienterId,IsEnable,UpdateBy,UpdateTime",
                " BusinessClienterRelation a (nolock)", SystemConst.PageSize, true);
        }
    }
}
