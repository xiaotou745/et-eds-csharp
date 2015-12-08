using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using ETS.Util;
using ETS.Data.Generic;
using Ets.Model.DomainModel.Order;
using Ets.Model.ParameterModel.AliPay;
using ETS.Enums;
using Letao.Util;
using System.Data.SqlClient;

namespace Ets.Dao.Order
{
    /// <summary>
    /// 订单明细表 数据访问类OrderChildDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 18:48:39
    /// </summary>
    public class OrderTipCostDao : DaoBase
    {
        public OrderTipCostDao()
        { }
       
        		/// <summary>
		/// 增加一条记录
		/// </summary>
		public long Insert(OrderTipCost orderTipCost)
		{
			const string insertSql = @"
insert into OrderTipCost(OrderId,Amount,CreateName,CreateTime,PayStates)
values(@OrderId,@Amount,@CreateName,@CreateTime,@PayStates);
select @@IDENTITY
";

			IDbParameters dbParameters = DbHelper.CreateDbParameters();
			dbParameters.AddWithValue("OrderId", orderTipCost.OrderId);
			dbParameters.AddWithValue("Amount", orderTipCost.Amount);
			dbParameters.AddWithValue("CreateName", orderTipCost.CreateName);
			dbParameters.AddWithValue("CreateTime", orderTipCost.CreateTime);
			dbParameters.AddWithValue("PayStates", orderTipCost.PayStates);

    object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters); //提现单号
            return ParseHelper.ToLong(result);
		}

		/// <summary>
		/// 更新一条记录
		/// </summary>
		public void Update(OrderTipCost orderTipCost)
		{
			const string updateSql = @"
update  OrderTipCost
set  OrderId=@OrderId,Amount=@Amount,CreateName=@CreateName,CreateTime=@CreateTime,PayStates=@PayStates
where  ";

			IDbParameters dbParameters = DbHelper.CreateDbParameters();
			dbParameters.AddWithValue("Id", orderTipCost.Id);
			dbParameters.AddWithValue("OrderId", orderTipCost.OrderId);
			dbParameters.AddWithValue("Amount", orderTipCost.Amount);
			dbParameters.AddWithValue("CreateName", orderTipCost.CreateName);
			dbParameters.AddWithValue("CreateTime", orderTipCost.CreateTime);
			dbParameters.AddWithValue("PayStates", orderTipCost.PayStates);


			  DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
		}	

		
		/// <summary>
		/// 根据ID获取对象
		/// </summary>
        public OrderTipCost GetById(long id)
		{
            OrderTipCost model = new OrderTipCost();

			const string querysql = @"
select  Id,OrderId,Amount,CreateName,CreateTime,PayStates
from  OrderTipCost (nolock)
where  ";		


            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int64, 8, id);

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<OrderTipCost>(dt)[0];
            }
            return model;
		}

	

		#region  Nested type: OrderTipCostRowMapper

		/// <summary>
		/// 绑定对象
		/// </summary>
		private class OrderTipCostRowMapper : IDataTableRowMapper<OrderTipCost>
		{
			public OrderTipCost MapRow(DataRow dataReader)
			{
				var result = new OrderTipCost();
				object obj;
				obj = dataReader["Id"];
				if (obj != null && obj != DBNull.Value)
				{
					result.Id = int.Parse(obj.ToString());
				}
				obj = dataReader["OrderId"];
				if (obj != null && obj != DBNull.Value)
				{
					result.OrderId = int.Parse(obj.ToString());
				}
				obj = dataReader["Amount"];
				if (obj != null && obj != DBNull.Value)
				{
					result.Amount = decimal.Parse(obj.ToString());
				}
				result.CreateName = dataReader["CreateName"].ToString();
				obj = dataReader["CreateTime"];
				if (obj != null && obj != DBNull.Value)
				{
					result.CreateTime = DateTime.Parse(obj.ToString());
				}
				obj = dataReader["PayStates"];
				if (obj != null && obj != DBNull.Value)
				{
					result.PayStates = int.Parse(obj.ToString());
				}

				return result;
			}
		}

		#endregion
    }

}
