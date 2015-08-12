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
    public class OrderSubsidiesLogDao : DaoBase
    {
   		public OrderSubsidiesLogDao()
		{}
		#region IOrderSubsidiesLogRepos  Members

		/// <summary>
		/// 增加一条记录
		/// </summary>
		public int Insert(OrderSubsidiesLog orderSubsidiesLog)
		{
			const string insertSql = @"
insert into OrderSubsidiesLog(OrderId,Price,OptName,Remark,OptId,OrderStatus,Platform)
values(@OrderId,@Price,@OptName,@Remark,@OptId,@OrderStatus,@Platform)

select @@IDENTITY";

			IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("OrderId", orderSubsidiesLog.OrderId);
            dbParameters.AddWithValue("Price", orderSubsidiesLog.Price);     
            dbParameters.AddWithValue("OptName", orderSubsidiesLog.OptName);
            dbParameters.AddWithValue("Remark", orderSubsidiesLog.Remark);
            dbParameters.AddWithValue("OptId", orderSubsidiesLog.OptId);
            dbParameters.AddWithValue("OrderStatus", orderSubsidiesLog.OrderStatus);
            dbParameters.AddWithValue("Platform", orderSubsidiesLog.Platform);


			object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
			if (result == null)
			{
				return 0;
			}
			return int.Parse(result.ToString());
		}	
	
		#endregion	

    }
}

