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
insert into OrderTipCost(OrderId,Amount,TipAmount,CreateName,CreateTime,PayStates,OriginalOrderNo,PayType,OutTradeNo)
values(@OrderId,@Amount,@TipAmount,@CreateName,@CreateTime,@PayStates,@OriginalOrderNo,@PayType,@OutTradeNo);
select @@IDENTITY
";

			IDbParameters dbParameters = DbHelper.CreateDbParameters();
			dbParameters.AddWithValue("OrderId", orderTipCost.OrderId);
			dbParameters.AddWithValue("Amount", orderTipCost.Amount);
            dbParameters.AddWithValue("TipAmount", orderTipCost.TipAmount);
			dbParameters.AddWithValue("CreateName", orderTipCost.CreateName);
			dbParameters.AddWithValue("CreateTime", orderTipCost.CreateTime);
			dbParameters.AddWithValue("PayStates", orderTipCost.PayStates);
            dbParameters.AddWithValue("OriginalOrderNo", orderTipCost.OriginalOrderNo);
            dbParameters.AddWithValue("PayType", orderTipCost.PayType);
            dbParameters.AddWithValue("OutTradeNo", orderTipCost.OutTradeNo);

    object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters); //提现单号
            return ParseHelper.ToLong(result);
		}

        /// <summary>
        /// 判断第三方平台的充值单号存不存在 
        /// 胡灵波
        /// 2015年12月17日 19:24:05
        /// </summary>
        /// <param name="OriginalOrderNo"></param>
        /// <returns>true=存在</returns>
        public bool Check(string OriginalOrderNo)
        {
            string sql = @"
SELECT count(1) FROM dbo.OrderTipCost  where OriginalOrderNo=@OriginalOrderNo";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("OriginalOrderNo", DbType.String, 100).Value = OriginalOrderNo;
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm), 0) > 0 ? true : false;
        }
        public bool CheckOutTradeNo(string OutTradeNo)
        {
            string sql = @"
SELECT count(1) FROM dbo.OrderTipCost (nolock)  where OutTradeNo=@OutTradeNo";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("OutTradeNo", DbType.String, 100).Value = OutTradeNo;
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm), 0) > 0 ? true : false;
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
        /// 取消订单 更新小费状态
        /// </summary>
        /// <param name="orderTipCost"></param>
        public int UpdatePayStates(OrderTipCost orderTipCost)
        {
            const string updateSql = @"
update  OrderTipCost
set  PayStates=@PayStates
where Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", orderTipCost.Id);   
            dbParameters.AddWithValue("PayStates", orderTipCost.PayStates);

            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }
        /// <summary>
        /// 更新一条记录
        /// </summary>
        public void UpdateByOutTradeNo(OrderTipCost orderTipCost)
        {
            const string updateSql = @"
update  OrderTipCost
set   UpdateName=@UpdateName,OriginalOrderNo=@OriginalOrderNo, UpdateTime=getdate(),PayStates=1
where  PayType=@PayType and OutTradeNo=@OutTradeNo";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("UpdateName", orderTipCost.UpdateName);
            dbParameters.AddWithValue("PayType", orderTipCost.PayType);
            dbParameters.AddWithValue("OutTradeNo", orderTipCost.OutTradeNo);
            dbParameters.AddWithValue("OriginalOrderNo", orderTipCost.OriginalOrderNo);

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
where  Id=@Id";		


            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int64, 8, id);

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<OrderTipCost>(dt)[0];
            }
            return model;
		}


        public IList<OrderTipCost> GetListByOrderId(int orderId)
        {
            IList<OrderTipCost> models = new List<OrderTipCost>();
            const string querysql = @"
select Id,OrderId,Amount,CreateName,CreateTime,PayStates,OriginalOrderNo,PayType,OutTradeNo from OrderTipCost where orderId=@orderId and PayStates>-1";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("orderId", orderId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return models;
            }
            return MapRows<OrderTipCost>(dt);
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
