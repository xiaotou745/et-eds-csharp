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
using ETS.Const;

namespace Ets.Dao.User
{
    public class BusinessDao : DaoBase
    {
        /// <summary>
        /// 商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <param name="paraModel">查询条件实体</param>
        public virtual PageInfo<T> GetOrdersAppToSql<T>(Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp paraModel)
        {
            string whereStr = "1=1 ";  //where查询条件实体类
            string orderByColumn = "a.id ";  //排序条件
            string columnList = "a.* ";  //列名
            string tableList = "dbo.[order] AS a ";  //表名
            if (paraModel.userId != null)  //订单商户id
                whereStr = whereStr + " and a.businessId=" + paraModel.userId.ToString();
            if (paraModel.Status != null)  //订单状态
                whereStr = whereStr + " and a.Status=" + paraModel.Status.ToString();
            return new PageHelper().GetPages<T>(SuperMan_Read, 1, whereStr, orderByColumn, columnList, tableList, 10, true);  
        }
    }
}
