using ETS;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.User
{
    public class BusinessDao : DaoBase
    {
        /// <summary>
        /// 商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <param name="paraModel">查询条件实体</param>
        public virtual void GetOrdersAppToSql(Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp paraModel)
        {
            string whereStr = "1=1 ";  //where查询条件实体类
            //if (paraModel.userId != 0)
                
            
        }
    }
}
