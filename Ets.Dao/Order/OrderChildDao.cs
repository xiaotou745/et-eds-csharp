﻿using System;
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

namespace Ets.Dao.Order
{
    /// <summary>
    /// 订单明细表 数据访问类OrderChildDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 18:48:39
    /// </summary>
    public class OrderChildDao :DaoBase
    {
        public OrderChildDao()
        { }
        #region IOrderChildRepos  Members

        /// <summary>
        /// 增加一条记录
        /// </summary>
        public long Insert(OrderChild orderChild)
        {
            const string insertSql = @"
insert into OrderChild(OrderId,ChildId,TotalPrice,GoodPrice,DeliveryPrice,PayStyle,PayType,PayStatus,PayBy,PayTime,PayPrice,HasUploadTicket,TicketUrl,CreateBy,CreateTime,UpdateBy,UpdateTime)
values(@OrderId,@ChildId,@TotalPrice,@GoodPrice,@DeliveryPrice,@PayStyle,@PayType,@PayStatus,@PayBy,@PayTime,@PayPrice,@HasUploadTicket,@TicketUrl,@CreateBy,@CreateTime,@UpdateBy,@UpdateTime)

select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("OrderId", orderChild.OrderId);
            dbParameters.AddWithValue("ChildId", orderChild.ChildId);
            dbParameters.AddWithValue("TotalPrice", orderChild.TotalPrice);
            dbParameters.AddWithValue("GoodPrice", orderChild.GoodPrice);
            dbParameters.AddWithValue("DeliveryPrice", orderChild.DeliveryPrice);
            dbParameters.AddWithValue("PayStyle", orderChild.PayStyle);
            dbParameters.AddWithValue("PayType", orderChild.PayType);
            dbParameters.AddWithValue("PayStatus", orderChild.PayStatus);
            dbParameters.AddWithValue("PayBy", orderChild.PayBy);
            dbParameters.AddWithValue("PayTime", orderChild.PayTime);
            dbParameters.AddWithValue("PayPrice", orderChild.PayPrice);
            dbParameters.AddWithValue("HasUploadTicket", orderChild.HasUploadTicket);
            dbParameters.AddWithValue("TicketUrl", orderChild.TicketUrl);
            dbParameters.AddWithValue("CreateBy", orderChild.CreateBy);
            dbParameters.AddWithValue("CreateTime", orderChild.CreateTime);
            dbParameters.AddWithValue("UpdateBy", orderChild.UpdateBy);
            dbParameters.AddWithValue("UpdateTime", orderChild.UpdateTime);
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters); //提现单号
            return ParseHelper.ToLong(result);         
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        public void Update(OrderChild orderChild)
        {
            const string updateSql = @"
update  OrderChild
set  OrderId=@OrderId,ChildId=@ChildId,TotalPrice=@TotalPrice,GoodPrice=@GoodPrice,DeliveryPrice=@DeliveryPrice,PayStyle=@PayStyle,PayType=@PayType,PayStatus=@PayStatus,PayBy=@PayBy,PayTime=@PayTime,PayPrice=@PayPrice,HasUploadTicket=@HasUploadTicket,TicketUrl=@TicketUrl,CreateBy=@CreateBy,CreateTime=@CreateTime,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime
where  Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", orderChild.Id);
            dbParameters.AddWithValue("OrderId", orderChild.OrderId);
            dbParameters.AddWithValue("ChildId", orderChild.ChildId);
            dbParameters.AddWithValue("TotalPrice", orderChild.TotalPrice);
            dbParameters.AddWithValue("GoodPrice", orderChild.GoodPrice);
            dbParameters.AddWithValue("DeliveryPrice", orderChild.DeliveryPrice);
            dbParameters.AddWithValue("PayStyle", orderChild.PayStyle);
            dbParameters.AddWithValue("PayType", orderChild.PayType);
            dbParameters.AddWithValue("PayStatus", orderChild.PayStatus);
            dbParameters.AddWithValue("PayBy", orderChild.PayBy);
            dbParameters.AddWithValue("PayTime", orderChild.PayTime);
            dbParameters.AddWithValue("PayPrice", orderChild.PayPrice);
            dbParameters.AddWithValue("HasUploadTicket", orderChild.HasUploadTicket);
            dbParameters.AddWithValue("TicketUrl", orderChild.TicketUrl);
            dbParameters.AddWithValue("CreateBy", orderChild.CreateBy);
            dbParameters.AddWithValue("CreateTime", orderChild.CreateTime);
            dbParameters.AddWithValue("UpdateBy", orderChild.UpdateBy);
            dbParameters.AddWithValue("UpdateTime", orderChild.UpdateTime);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        public void Delete(long id)
        {  
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        public IList<OrderChild> Query(OrderChildPM orderChildPM)
        {
            IList<OrderChild> models = new List<OrderChild>();
            string condition = BindQueryCriteria(orderChildPM);
            string querysql = @"
select  Id,OrderId,ChildId,TotalPrice,GoodPrice,DeliveryPrice,PayStyle,PayType,PayStatus,PayBy,PayTime,PayPrice,HasUploadTicket,TicketUrl,CreateBy,CreateTime,UpdateBy,UpdateTime
from  OrderChild (nolock)" + condition;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<OrderChild>(dt);
            }
            return models;
          }


        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        public OrderChild GetById(long id)
        {
            OrderChild model = new OrderChild();

            const string querysql = @"
select  Id,OrderId,ChildId,TotalPrice,GoodPrice,DeliveryPrice,PayStyle,PayType,PayStatus,PayBy,PayTime,PayPrice,HasUploadTicket,TicketUrl,CreateBy,CreateTime,UpdateBy,UpdateTime
from  OrderChild (nolock)
where  Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);


            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<OrderChild>(dt)[0];
            }
            return model;
        }

        #endregion


        #region  Other Members

        /// <summary>
        /// 构造查询条件
        /// </summary>
        public static string BindQueryCriteria(OrderChildPM orderChildQueryDTO)
        {
            var stringBuilder = new StringBuilder(" where 1=1 ");
            if (orderChildQueryDTO == null)
            {
                return stringBuilder.ToString();
            }

            //TODO:在此加入查询条件构建代码

            return stringBuilder.ToString();
        }

        #endregion
    }

}
