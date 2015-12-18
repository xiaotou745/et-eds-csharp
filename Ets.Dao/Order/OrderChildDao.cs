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
    public class OrderChildDao : DaoBase
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
insert into OrderChild(OrderId,ChildId,TotalPrice,GoodPrice,DeliveryPrice,PayStyle,PayType,
PayStatus,PayBy,PayTime,PayPrice,HasUploadTicket,TicketUrl,CreateBy,CreateTime,UpdateBy,UpdateTime)
values(@OrderId,@ChildId,@TotalPrice,@GoodPrice,@DeliveryPrice,@PayStyle,@PayType,
@PayStatus,@PayBy,@PayTime,@PayPrice,@HasUploadTicket,@TicketUrl,@CreateBy,@CreateTime,@UpdateBy,@UpdateTime)

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
set  OrderId=@OrderId,ChildId=@ChildId,TotalPrice=@TotalPrice,GoodPrice=@GoodPrice,
DeliveryPrice=@DeliveryPrice,PayStyle=@PayStyle,PayType=@PayType,PayStatus=@PayStatus,
PayBy=@PayBy,PayTime=@PayTime,PayPrice=@PayPrice,HasUploadTicket=@HasUploadTicket,
TicketUrl=@TicketUrl,CreateBy=@CreateBy,CreateTime=@CreateTime,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime
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
        /// 更新状态
        /// </summary>
        /// 胡灵波
        /// 2015年11月19日 20:46:34
        /// <param name="orderChild"></param>
        public void UpdateStatus(OrderChild orderChild)
        {
            const string updateSql = @"
update  OrderChild
set  Status=@Status,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime
where  Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", orderChild.Id);
            dbParameters.AddWithValue("Status", orderChild.Status);
            dbParameters.AddWithValue("UpdateBy", orderChild.UpdateBy);
            dbParameters.AddWithValue("UpdateTime", orderChild.UpdateTime);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }
        /// <summary>
        /// 更新是否已付款
        /// </summary>
        /// <param name="orderId"></param>
        public bool UpdateIsPay(int id, int payType)
        {
            const string updateSql = @"
update  [OrderChild]
set PayStatus = 1,PayType=@PayType
where   id = @id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("id", DbType.Int32, 4).Value = id;
            dbParameters.Add("PayType", DbType.Int32, 4).Value = payType;
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters) == 1 ? true : false;
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
        public IList<OrderChild> Query(OrderChlidPM orderChildPM)
        {
            IList<OrderChild> models = new List<OrderChild>();
            string condition = BindQueryCriteria(orderChildPM);
            string querysql = @"
select  Id,OrderId,ChildId,TotalPrice,GoodPrice,DeliveryPrice,PayStyle,PayType,PayStatus,
PayBy,PayTime,PayPrice,HasUploadTicket,TicketUrl,CreateBy,CreateTime,UpdateBy,UpdateTime
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
select  Id,OrderId,ChildId,TotalPrice,GoodPrice,DeliveryPrice,PayStyle,PayType,PayStatus,
PayBy,PayTime,PayPrice,HasUploadTicket,TicketUrl,CreateBy,CreateTime,UpdateBy,UpdateTime
from  OrderChild (nolock)
where  Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int64, 8, id);

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
        public static string BindQueryCriteria(OrderChlidPM orderChildQueryDTO)
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
        /// <summary>
        /// 根据订单Id和子订单Id获取子订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderChildId"></param>
        /// <returns></returns>
        public List<OrderChildForTicket> GetOrderChildInfo(int orderId, int orderChildId)
        {
            List<OrderChildForTicket> oc = new List<OrderChildForTicket>();
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
select  o.Id OrderId ,
        oc.ChildId ,
        o.[Status] OrderStatus ,
        o.OrderCount NeedUploadCount ,
        isnull(oo.HadUploadCount, 0) HadUploadCount ,
        oc.HasUploadTicket ,
        oc.TicketUrl
from    dbo.[order] o ( nolock )
        join dbo.OrderChild oc ( nolock ) on o.Id = oc.OrderId
        left join dbo.OrderOther oo ( nolock ) on o.Id = oo.OrderId
where   1=1 and o.Id = @OrderId 
");
            if (orderChildId > 0)
            {
                sql.Append(" and ChildId = @ChildId ;");
            }
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderId", SqlDbType.Int).Value = orderId;
            parm.Add("@ChildId", SqlDbType.Int).Value = orderChildId;

            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql.ToString(), parm);
            if (!dt.HasData())
                return oc;
            return MapRows<List<OrderChildForTicket>>(dt)[0];
        }

        /// <summary>
        /// 更新微信的二维码地址，因为微信支付的时候生成二维码只能是一次
        /// 窦海超
        /// 2015年5月13日 16:09:50
        /// </summary>
        /// <param name="orderId">主订单号</param>
        /// <param name="orderChildId">子订单号</param>
        /// <param name="wxCodeUrl">微信二维码地址</param>
        /// <returns></returns>
        public bool UpdateWxCodeUrl(int orderId, int orderChildId, string wxCodeUrl)
        {
            string sql = "update dbo.OrderChild set WxCodeUrl = @WxCodeUrl where OrderId=@OrderId and ChildId=@ChildId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("WxCodeUrl", DbType.String, 256).Value = wxCodeUrl;
            parm.Add("OrderId", DbType.Int32, 4).Value = orderId;
            parm.Add("ChildId", DbType.Int32, 4).Value = orderChildId;
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm)) > 0 ? true : false;            
        }

        /// <summary>
        /// 获取订单状态
        /// 窦海超
        /// 2015年5月13日 11:04:41
        /// </summary>
        /// <param name="orderId">主订单号</param>
        /// <param name="orderChildId">子订单号</param>
        /// <returns>不存在返回-1</returns>
        public PayStatusModel GetPayStatus(int orderId, int orderChildId)
        {
            string sql = "SELECT  oc.PayStatus,oc.TotalPrice,oc.WxCodeUrl,o.TipAmount from dbo.OrderChild oc(nolock) left join dbo.[order] o (nolock) on oc.orderid=o.id   where o.id = @OrderId and oc.ChildId = @ChildId ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("OrderId", DbType.Int32, 4).Value = orderId;
            parm.Add("ChildId", DbType.Int32, 4).Value = orderChildId;
            //此表是要同步支付状态，请读写表
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, parm);
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<PayStatusModel>(dt)[0];
        }

        /// <summary>
        /// 获取订单状态
        /// 窦海超
        /// 2015年5月13日 11:04:41
        /// </summary>
        /// <param name="orderId">主订单号</param>
        /// <param name="orderChildId">子订单号</param>
        /// <returns>不存在返回-1</returns>
        public PayStatusModel GetPaySSStatus(int orderId, int orderChildId)
        {
            string sql = "SELECT  oc.PayStatus,oc.TotalPrice,oc.WxCodeUrl,o.TipAmount,o.OrderNo from dbo.OrderChild oc(nolock) left join dbo.[order] o (nolock) on oc.orderid=o.id   where o.id = @OrderId and oc.id = @OrderChildId ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("OrderId", DbType.Int32, 4).Value = orderId;
            parm.Add("OrderChildId", DbType.Int32, 4).Value = orderChildId;
            //此表是要同步支付状态，请读写表
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, parm);
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<PayStatusModel>(dt)[0];
        }

        public OrderChild GetDetailByOrderId(int orderId)
        {
            string sql = "SELECT top 1 id,OrderId,ChildId,TotalPrice,GoodPrice,DeliveryPrice,PayStyle,PayType,PayStatus,PayBy,PayTime,PayPrice,HasUploadTicket,TicketUrl,CreateBy,CreateTime,UpdateBy,UpdateTime  from dbo.OrderChild oc  where oc.OrderId = @OrderId ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("OrderId", DbType.Int32, 4).Value = orderId;          
            //此表是要同步支付状态，请读写表
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, parm);
         
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<OrderChild>(dt)[0];
        }


        /// <summary>
        /// 查询子订单状态，和是否有未完成的订单，APP刷新订单状态用
        /// 窦海超
        /// 2015年5月28日 14:33:12
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderChildId"></param>
        /// <returns></returns>
        public PayStatusModel GetChildPayStatus(int orderId, int orderChildId)
        {
            string sql = @"
select
        PayStatus, OrderId, ChildId, ( select   min(PayStatus)
                                       from     dbo.OrderChild oc ( nolock )
                                       where    OrderId = @OrderId
                                     ) unFinish
from    dbo.OrderChild oc ( nolock )
where   OrderId = @OrderId
        and ChildId = @ChildId
            
            ";//ThirdPayStatus as 
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("OrderId", DbType.Int32, 4).Value = orderId;
            parm.Add("ChildId", DbType.Int32, 4).Value = orderChildId;
            //此表是要同步支付状态，请读写表
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, parm);
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<PayStatusModel>(dt)[0];
        }
        /// <summary>
        /// 支付子订单完成
        /// 窦海超
        /// 2015年5月12日 15:36:34
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="orderChildId">子订单ID</param>
        /// <param name="payStyle">支付方式(1 用户支付 2 骑士代付)</param>
        /// <returns></returns>
        public bool FinishPayStatus(OrderChildFinishModel model)
        {
            //return ChangePayStatus(EnumOrderChildStatus.YiWanCheng.GetHashCode(), EnumOrderChildStatus.ZhiFuZhong.GetHashCode(), payStyle, orderId, orderChildId);
            string sql = @"
update OrderChild set PayStatus=@PayStatus,ThirdPayStatus=@PayStatus,PayStyle=@PayStyle,PayBy=@PayBy,PayTime=getdate(),
PayType=@PayType , OriginalOrderNo=@OriginalOrderNo
where OrderId=@OrderId and ChildId=@ChildId and ThirdPayStatus!=@PayStatus";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("PayStatus", DbType.Int32, 2).Value = PayStatusEnum.HadPay.GetHashCode();//变为已完成
            parm.Add("PayStyle", DbType.Int16).Value = model.payStyle;
            parm.Add("PayBy", DbType.String, 100).Value = model.payBy;
            parm.Add("PayType", DbType.Int16).Value = model.payType;
            parm.Add("OriginalOrderNo", DbType.String, 265).Value = model.originalOrderNo;
            parm.Add("OrderId", SqlDbType.Int, 4).Value = model.orderId;
            parm.Add("ChildId", SqlDbType.Int, 4).Value = model.orderChildId;
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm), 0) > 0 ? true : false;
        }

        /// <summary>
        /// 子订单支付中
        /// 窦海超
        /// 2015年5月12日 15:36:34
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="orderChildId">子订单ID</param>
        /// <returns></returns>
        public bool ZhuFuZhongPayStatus(int orderId, int orderChildId)
        {
            string sql = "update OrderChild set ThirdPayStatus=@ThirdPayStatus where OrderId=@OrderId and ChildId=@ChildId and ThirdPayStatus=@BeForeStatus";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("ThirdPayStatus", DbType.Int32, 4).Value = PayStatusEnum.WaitingPay.GetHashCode();//变为支付中
            parm.Add("BeForeStatus", DbType.Int32, 4).Value = PayStatusEnum.WaitPay.GetHashCode();//条件为待支付
            parm.Add("OrderId", SqlDbType.Int, 4).Value = orderId;
            parm.Add("ChildId", SqlDbType.Int, 4).Value = orderChildId;
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm), 0) > 0 ? true : false;
        }

        /// <summary>
        /// 获取子订单列表
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="id">订单Id</param>
        /// <returns></returns>
        public List<OrderChildInfo> GetByOrderId(long orderId)
        {
            List<OrderChildInfo> list = new List<OrderChildInfo>();

            const string querySql = @"
select  ChildId,TotalPrice,GoodPrice,DeliveryPrice,PayStyle,PayType,PayStatus,PayBy,
    PayTime,PayPrice,HasUploadTicket,TicketUrl
from  OrderChild (nolock)
where  OrderId=@OrderId 
order by ChildId asc";

            IDbParameters dbParameters = DbHelper.CreateDbParameters("OrderId", DbType.Int64, 8, orderId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, querySql, dbParameters);

            foreach (DataRow dataRow in dt.Rows)
            {
                OrderChildInfo ochildInfo = new OrderChildInfo();
                ochildInfo.ChildId = ParseHelper.ToInt(dataRow["ChildId"]);
                ochildInfo.TotalPrice = ParseHelper.ToDecimal(dataRow["TotalPrice"]);
                ochildInfo.GoodPrice = ParseHelper.ToDecimal(dataRow["GoodPrice"]);
                ochildInfo.DeliveryPrice = ParseHelper.ToDecimal(dataRow["DeliveryPrice"]);
                if (dataRow["PayStyle"] != null && dataRow["PayStyle"] != DBNull.Value)
                    ochildInfo.PayStyle = ParseHelper.ToInt(dataRow["PayStyle"]);
                if (dataRow["PayType"] != null && dataRow["PayType"] != DBNull.Value)
                    ochildInfo.PayType = ParseHelper.ToInt(dataRow["PayType"]);
                ochildInfo.PayStatus = ParseHelper.ToInt(dataRow["PayStatus"]);
                if (dataRow["PayBy"] != null && dataRow["PayBy"] != DBNull.Value)
                    ochildInfo.PayBy = dataRow["PayBy"].ToString();
                if (dataRow["PayTime"] != null && dataRow["PayTime"] != DBNull.Value)
                    ochildInfo.PayTime = ParseHelper.ToDatetime(dataRow["PayTime"]);
                ochildInfo.PayPrice = ParseHelper.ToDecimal(dataRow["PayPrice"]);
                ochildInfo.HasUploadTicket = ParseHelper.ToBool(dataRow["HasUploadTicket"]);
                if (dataRow["TicketUrl"] != null && dataRow["TicketUrl"] != DBNull.Value && dataRow["TicketUrl"].ToString() != "")
                    ochildInfo.TicketUrl = Ets.Model.Common.ImageCommon.ReceiptPicConvert(dataRow["TicketUrl"].ToString());
                list.Add(ochildInfo);
            }

            return list;
        }

        /// <summary>
        /// 获取子订单列表
        /// </summary>
        /// <UpdateBy>WangChao</UpdateBy>
        /// <returns></returns>
        public List<OrderChild> GetListByOrderId(List<int> orderIdList)
        {
            //var str= String.Join(",", orderIdList.ConvertAll<string>(new Converter<int, string>(m => m.ToString())).ToArray());
            List<OrderChild> list = new List<OrderChild>();
            string querySql = String.Format(@"
select OrderId, ChildId,TotalPrice,GoodPrice,DeliveryPrice,PayStyle,PayType,PayStatus,PayBy,
    PayTime,PayPrice,HasUploadTicket,TicketUrl 
from  OrderChild (nolock) where OrderId IN ({0})", String.Join(",", orderIdList.ConvertAll<string>(new Converter<int, string>(m => m.ToString())).ToArray()));
            //IDbParameters dbParameters = DbHelper.CreateDbParameters("orderIdList", DbType.Int64, 8, orderIdList);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, querySql);
            foreach (DataRow dataRow in dt.Rows)
            {
                OrderChild ochildInfo = new OrderChild();
                ochildInfo.OrderId = ParseHelper.ToInt(dataRow["OrderId"]);
                ochildInfo.ChildId = ParseHelper.ToInt(dataRow["ChildId"]);
                ochildInfo.TotalPrice = ParseHelper.ToDecimal(dataRow["TotalPrice"]);
                ochildInfo.GoodPrice = ParseHelper.ToDecimal(dataRow["GoodPrice"]);
                ochildInfo.DeliveryPrice = ParseHelper.ToDecimal(dataRow["DeliveryPrice"]);
                if (dataRow["PayStyle"] != null && dataRow["PayStyle"] != DBNull.Value)
                    ochildInfo.PayStyle = ParseHelper.ToInt(dataRow["PayStyle"]);
                if (dataRow["PayType"] != null && dataRow["PayType"] != DBNull.Value)
                    ochildInfo.PayType = ParseHelper.ToInt(dataRow["PayType"]);
                ochildInfo.PayStatus = ParseHelper.ToInt(dataRow["PayStatus"]);
                if (dataRow["PayBy"] != null && dataRow["PayBy"] != DBNull.Value)
                    ochildInfo.PayBy = dataRow["PayBy"].ToString();
                if (dataRow["PayTime"] != null && dataRow["PayTime"] != DBNull.Value)
                    ochildInfo.PayTime = ParseHelper.ToDatetime(dataRow["PayTime"]);
                ochildInfo.PayPrice = ParseHelper.ToDecimal(dataRow["PayPrice"]);
                ochildInfo.HasUploadTicket = ParseHelper.ToBool(dataRow["HasUploadTicket"]);
                if (dataRow["TicketUrl"] != null && dataRow["TicketUrl"] != DBNull.Value && dataRow["TicketUrl"].ToString() != "")
                    ochildInfo.TicketUrl = Ets.Model.Common.ImageCommon.ReceiptPicConvert(dataRow["TicketUrl"].ToString());
                list.Add(ochildInfo);
            }
            return list;
        }

        /// <summary>
        /// 查询对象(获取智能调度,未抢单数据)
        /// 胡灵波
        /// 2015年11月19日 20:27:33
        /// </summary>
        public IList<OrderChild> GetListByTime(string startTime, string endTime)
        {
            IList<OrderChild> list = new List<OrderChild>(); 
            string querySql = @" 
 select  oc.Id,oc.OrderId,o.OrderNo,o.ordercount,o.businessId,oc.SettleMoney,b.Name as BusinessName  from  orderchild oc 
left join [order] o on oc.orderid=o.id
left join dbo.business b on o.businessid=b.id
where  oc.platform=2 and oc.status=0 
and oc.CreateTime>=@startTime and oc.CreateTime<=@endTime";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("startTime", DbType.String, 100).Value = startTime;
            dbParameters.Add("endTime", DbType.String, 100).Value = endTime;
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, querySql, dbParameters);
          
            foreach (DataRow dataRow in dt.Rows)
            {
                OrderChild ochildInfo = new OrderChild();
                ochildInfo.Id = ParseHelper.ToInt(dataRow["Id"]);
                ochildInfo.OrderId = ParseHelper.ToInt(dataRow["OrderId"]);
                if (dataRow["OrderNo"]!=null)
                    ochildInfo.OrderNo = dataRow["OrderNo"].ToString();
                ochildInfo.OrderCount = ParseHelper.ToInt(dataRow["ordercount"]);
                ochildInfo.businessId = ParseHelper.ToInt(dataRow["businessId"]);
                ochildInfo.SettleMoney = ParseHelper.ToDecimal(dataRow["SettleMoney"]);
                if (dataRow["BusinessName"] != null)
                    ochildInfo.BusinessName = dataRow["BusinessName"].ToString();              
                list.Add(ochildInfo);
            }
            return list;            
        }

        /// <summary>
        /// 通过订单ID获取是否有子订单未支付
        /// 窦海超
        /// 2015年5月27日 18:04:53
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int CheckOrderChildPayStatus(int orderId)
        {
            string sql = @"
select  count(1)
from    dbo.[order] o ( nolock )
        join dbo.OrderChild oc ( nolock ) on o.Id = oc.OrderId
where   o.Id = @orderId
        and (o.IsPay = 1
        or o.MealsSettleMode = 0
        or oc.ThirdPayStatus = 0
        or oc.ThirdPayStatus = 2)
";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("orderId", DbType.Int32, 4).Value = orderId;
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm), 0);
        }

        /// <summary>
        /// 获取子订单是否完成上传小票
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150602</UpdateTime>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public bool GetHasUploadTicket(int orderId, int childId)
        {
            //事务中 去nolock，用写串
            string querySql = @"
select HasUploadTicket from  OrderChild oc     
where   oc.OrderId = @OrderId and ChildId=@ChildId ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            //dbParameters.AddWithValue("OrderId", orderId);
            //dbParameters.AddWithValue("ChildId", childId);
            dbParameters.Add("OrderId", DbType.Int32, 4).Value = orderId;
            dbParameters.Add("ChildId", DbType.Int32, 4).Value = childId;
            return ParseHelper.ToBool(DbHelper.ExecuteScalar(SuperMan_Write, querySql, dbParameters), false);
        }

        /// <summary>
        /// 现金支付更新状态
        /// wc
        /// </summary>
        /// <param name="model"></param>
        /// <param name="payBy"></param>
        /// <returns></returns>
        public int UpdateChildStatusFromCashOrder(PayModel model,string payBy)
        {
            string sql = @"
update  OrderChild
set     PayType = @PayType ,
        PayStyle = @PayStyle ,
        PayStatus = @PayStatus ,PayTime=getdate(),PayBy=@PayBy,PayPrice=GoodPrice 
where   OrderId = @OrderId
        and ChildId = @ChildId
        and PayStatus = @BefPayStatus";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("PayType", DbType.Int32, 4).Value = PayTypeEnum.CashPay.GetHashCode();//现金支付
            parm.Add("PayStyle", DbType.Int32, 4).Value = PayStyleEnum.BuyerPay.GetHashCode();//用户支付 
            parm.Add("PayBy", DbType.String).Value = payBy; 
            parm.Add("OrderId", SqlDbType.Int, 4).Value = model.orderId;
            parm.Add("ChildId", SqlDbType.Int, 4).Value = model.childId;
            parm.Add("PayStatus", SqlDbType.Int, 4).Value = PayStatusEnum.HadPay.GetHashCode();  //已支付
            parm.Add("BefPayStatus", SqlDbType.Int, 4).Value = PayStatusEnum.WaitPay; //待支付

            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm), 0);
        }



        /// <summary>
        /// 写入订单子表
        /// </summary>
        /// <returns>订单实体</returns>
        public void InsertList(order order)
        {            
            using (SqlBulkCopy bulk = new SqlBulkCopy(SuperMan_Write))
            {
                try
                {
                    bulk.BatchSize = 1000;
                    bulk.DestinationTableName = "OrderChild";
                    bulk.NotifyAfter = order.listOrderChild.Count;
                    bulk.ColumnMappings.Add("OrderId", "OrderId");
                    bulk.ColumnMappings.Add("ChildId", "ChildId");
                    bulk.ColumnMappings.Add("TotalPrice", "TotalPrice");
                    bulk.ColumnMappings.Add("GoodPrice", "GoodPrice");
                    bulk.ColumnMappings.Add("DeliveryPrice", "DeliveryPrice");
                    bulk.ColumnMappings.Add("PayStatus", "PayStatus");
                    bulk.ColumnMappings.Add("CreateBy", "CreateBy");
                    bulk.ColumnMappings.Add("UpdateBy", "UpdateBy");

                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("OrderId", typeof(int)));
                    dt.Columns.Add(new DataColumn("ChildId", typeof(int)));
                    dt.Columns.Add(new DataColumn("TotalPrice", typeof(decimal)));
                    dt.Columns.Add(new DataColumn("GoodPrice", typeof(decimal)));
                    dt.Columns.Add(new DataColumn("DeliveryPrice", typeof(decimal)));
                    dt.Columns.Add(new DataColumn("PayStatus", typeof(int)));
                    dt.Columns.Add(new DataColumn("CreateBy", typeof(string)));
                    dt.Columns.Add(new DataColumn("UpdateBy", typeof(string)));

                    for (int i = 0; i < order.listOrderChild.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["OrderId"] = order.Id;                        
                        int num = i + 1;
                        dr["ChildId"] = num;
                        decimal totalPrice = order.listOrderChild[i].GoodPrice + Convert.ToDecimal(order.DistribSubsidy);
                        dr["TotalPrice"] = totalPrice;
                        dr["GoodPrice"] = order.listOrderChild[i].GoodPrice;
                        dr["DeliveryPrice"] = order.DistribSubsidy;
                        if ((bool)order.IsPay ||
                            (!(bool)order.IsPay && order.MealsSettleMode == MealsSettleMode.LineOff.GetHashCode())
                            )//已付款 未付款线下付款
                            dr["PayStatus"] = 1;
                        else
                            dr["PayStatus"] = 0;
                        dr["CreateBy"] = order.BusinessName;
                        dr["UpdateBy"] = order.BusinessName;
                        dt.Rows.Add(dr);
                    }
                    bulk.WriteToServer(dt);
                }
                catch (Exception err)
                {
                    throw err;
                }
            }        
        }
    }

}
