#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data;
using Ets.Model.DataModel.Finance;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.DomainModel.Finance;
using ETS.Util;
using Ets.Model.ParameterModel.Finance;
#endregion
using Ets.Model.DataModel.Order;
using ETS.Data.Generic;
using Ets.Model.DomainModel.Order;
namespace Ets.Dao.Order
{ 
    public class OrderDetailDao : DaoBase
    {
        public OrderDetailDao()
        {

        }

        #region IOrderdetailRepos  Members  


        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        public List<OrderDetailInfo> GetByOrderNo(string orderNo)
        {
            List<OrderDetailInfo> list = new List<OrderDetailInfo>();

            const string querySql = @"
select  Id,OrderNo,ProductName,UnitPrice,Quantity,InsertTime,FormDetailID,GroupID
from  Orderdetail (nolock)
where  OrderNo=@OrderNo ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("OrderNo", orderNo);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, querySql, dbParameters);
            list = (List<OrderDetailInfo>)MapRows<OrderDetailInfo>(dt);

            return list; 
        }

        #endregion       
    }
}

