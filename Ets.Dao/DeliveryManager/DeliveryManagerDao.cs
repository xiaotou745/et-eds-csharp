using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Clienter;
using Ets.Model.ParameterModel.Order;
using ETS.Util;

namespace Ets.Dao.DeliveryManager
{
    public class DeliveryManagerDao : DaoBase
    {
        /// <summary>
        /// 物流订单管理-获取骑士列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetClienterList<T>(ClienterSearchCriteria criteria)
        {
            string columnList = @"
                 C.[Id]			--ID
                ,C.[PhoneNo]	--电话
                ,ISNULL(C.[TrueName],'') AS TrueName	--姓名
                ,ISNULL(C.[IDCard],'') AS IDCard	--身份照号
                ,ISNULL(C.[PicUrl],'') AS PicUrl	--照片
                ,C.[Status]		--审核状态
                ,C.[InsertTime]	--申请时间
                ,C.[WorkStatus]	--工作状态 
                ,ISNULL(C.PicWithHandUrl,'') AS PicWithHandUrl
                ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(criteria.clienterName))
            {
                sbSqlWhere.AppendFormat(" AND C.TrueName='{0}' ", criteria.clienterName);
            }
            if (!string.IsNullOrEmpty(criteria.clienterPhone))
            {
                sbSqlWhere.AppendFormat(" AND C.PhoneNo='{0}' ", criteria.clienterPhone);
            }
            if (criteria.Status != -1)
            {
                sbSqlWhere.AppendFormat(" AND C.Status={0} ", criteria.Status);
            }
            if (!string.IsNullOrEmpty(criteria.businessCity))
            {
                sbSqlWhere.AppendFormat(" AND C.City='{0}' ", criteria.businessCity.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.deliveryCompany) && criteria.deliveryCompany != "0")
            {
                sbSqlWhere.AppendFormat(" AND C.DeliveryCompanyId={0} ", criteria.deliveryCompany);
            }
            if (!string.IsNullOrEmpty(criteria.AuthorityCityNameListStr)&&criteria.UserType!=0)
            {
                sbSqlWhere.AppendFormat(" AND C.City IN ({0}) ", criteria.AuthorityCityNameListStr.Trim());
            }
            string tableList = @" clienter C WITH (NOLOCK) ";
            string orderByColumn = " C.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetOrderList<T>(OrderSearchCriteria criteria)
        {
            string columnList = @"       o.[Id]
                                    ,o.[OrderNo]
                                    ,o.[PubDate]
                                    ,o.[ActualDoneDate]
                                    ,o.[Amount]
                                    ,o.[DistribSubsidy]
                                    ,o.[Status]
                                    ,o.[clienterId]
                                    ,o.[OrderCount]
                                    ,c.TrueName ClienterName
                                    ,c.PhoneNo ClienterPhoneNo
                                    ,b.Name BusinessName
                                    ,b.PhoneNo BusinessPhoneNo
                                    ,ISNULL(oo.HadUploadCount,0) HadUploadCount
                                    ,o.BusinessCommission --商家结算比例 
		                            ,CASE dc.SettleType WHEN 1 THEN '比例计算' WHEN 2 THEN '固定结算' ELSE '' END AS SettleType--结算类型
		                            ,CASE DC.SettleType WHEN 1 THEN dc.DeliveryCompanyRatio WHEN 2 THEN DC.DeliveryCompanySettleMoney ELSE 0 END AS SettleValue
		                            ,CASE DC.SettleType WHEN 1 THEN dc.ClienterSettleRatio WHEN 2 THEN DC.ClienterFixMoney ELSE 0 END AS SuperManSettleValue--骑士固定金额
		                            ,CASE dc.SettleType WHEN 1 THEN  o.[Amount]*dc.DeliveryCompanyRatio*0.01 WHEN 2 THEN o.[OrderCount]*DC.DeliveryCompanySettleMoney END  AS SettleValueAll
		                            ,CASE dc.SettleType WHEN 1 THEN  o.[Amount]*dc.ClienterSettleRatio*0.01 WHEN 2 THEN o.[OrderCount]*DC.ClienterFixMoney END AS SuperManSettleValueAll
                                    ,oo.IsNotRealOrder
                                    ,oo.GrabTime,oo.TakeTime,
                                    ,b.City AS BusinessCity
                                    ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(criteria.deliveryCompany))
            {
                sbSqlWhere.AppendFormat(" AND c.DeliveryCompanyId={0} ", criteria.deliveryCompany);
            }
            if (!string.IsNullOrWhiteSpace(criteria.orderId))
            {
                sbSqlWhere.AppendFormat(" AND o.OrderNo='{0}' ", criteria.orderId);
            }
            if (criteria.orderStatus != -1)
            {
                sbSqlWhere.AppendFormat(" AND o.Status={0} ", criteria.orderStatus);
            }
            if (!string.IsNullOrWhiteSpace(criteria.superManName))
            {
                sbSqlWhere.AppendFormat(" AND c.TrueName='{0}' ", criteria.superManName);
            }
            if (!string.IsNullOrWhiteSpace(criteria.superManPhone))
            {
                sbSqlWhere.AppendFormat(" AND c.PhoneNo='{0}' ", criteria.superManPhone);
            }
            if (!string.IsNullOrWhiteSpace(criteria.orderPubStart))
            {
                sbSqlWhere.AppendFormat(" AND o.PubDate>='{0}' ", ParseHelper.ToDatetime(criteria.orderPubStart.Trim(), DateTime.Now).ToString());
            }
            if (!string.IsNullOrWhiteSpace(criteria.orderPubEnd))
            {
                sbSqlWhere.AppendFormat(" AND o.PubDate<='{0}' ", ParseHelper.ToDatetime(criteria.orderPubEnd.Trim(), DateTime.Now).AddDays(1).ToString());
            }
            if (!string.IsNullOrWhiteSpace(criteria.businessCity))
            {
                sbSqlWhere.AppendFormat(" AND b.City='{0}' ", criteria.businessCity.Trim());
            }
            if (criteria.AuthorityCityNameListStr != null && !string.IsNullOrEmpty(criteria.AuthorityCityNameListStr.Trim())&&criteria.UserType!=0)
            {
                sbSqlWhere.AppendFormat(" AND b.City IN({0}) ", criteria.AuthorityCityNameListStr.Trim());
            }
            string tableList = @" [order] o WITH ( NOLOCK )
                                    LEFT JOIN clienter c WITH ( NOLOCK ) ON c.Id = o.clienterId
                                    JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                    JOIN dbo.OrderOther oo (nolock) ON o.Id = oo.OrderId 
                                    JOIN dbo.DeliveryCompany DC(NOLOCK) ON C.DeliveryCompanyId=DC.Id";
            string orderByColumn = " o.Status ASC,o.Id DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderListModel GetOrderByNo(string orderNo, int orderId)
        {
            if (string.IsNullOrWhiteSpace(orderNo))
            {
                return new OrderListModel();
            }
            #region 查询脚本
            //获取订单主单信息以及附带的商户信息、骑士信息、小票信息
            StringBuilder sql = new StringBuilder(@"SELECT top 1 o.[Id]
                                        ,o.[OrderNo]
                                        ,o.[PickUpAddress]
                                        ,o.PubDate
                                        ,o.[ReceviceName]
                                        ,o.[RecevicePhoneNo]
                                        ,o.[ReceviceAddress]
                                        ,o.[ActualDoneDate]
                                        ,o.[IsPay]
                                        ,o.[Amount]
                                        ,o.[OrderCommission]
                                        ,o.[DistribSubsidy]
                                        ,o.[WebsiteSubsidy]
                                        ,o.[Remark]
                                        ,o.[Status]
                                        ,o.[clienterId]
                                        ,o.[businessId]
                                        ,o.[ReceviceCity]
                                        ,o.[ReceviceLongitude]
                                        ,o.[ReceviceLatitude]
                                        ,o.[OrderFrom]
                                        ,o.[OriginalOrderId]
                                        ,o.[OriginalOrderNo]
                                        ,o.[Quantity]
                                        ,o.[Weight]
                                        ,o.[ReceiveProvince]
                                        ,o.[ReceiveArea]
                                        ,o.[ReceiveProvinceCode]
                                        ,o.[ReceiveCityCode]
                                        ,o.[ReceiveAreaCode]
                                        ,o.[OrderType]
                                        ,o.[KM]
                                        ,o.[GuoJuQty]
                                        ,o.[LuJuQty]
                                        ,o.[SongCanDate]
                                        ,o.[OrderCount]
                                        ,o.[CommissionRate] 
                                        ,b.[City] BusinessCity
                                        ,b.Name BusinessName
                                        ,b.PhoneNo BusinessPhoneNo
                                        ,b.Address BusinessAddress
                                        ,c.PhoneNo ClienterPhoneNo
                                        ,c.TrueName ClienterTrueName
                                        ,c.TrueName ClienterName
                                        ,c.AccountBalance AccountBalance                                      
                                        ,b.GroupId
                                        ,case when o.orderfrom=0 then '客户端' else g.GroupName end GroupName
                                        ,o.OriginalOrderNo
                                        ,oo.NeedUploadCount
                                        ,oo.HadUploadCount
                                        ,oo.ReceiptPic                                        
                                        ,o.OtherCancelReason
                                        ,o.OriginalOrderNo
                                        ,o.IsEnable
                                    FROM [order] o WITH ( NOLOCK )
                                    LEFT JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                    LEFT JOIN clienter c WITH (NOLOCK) ON o.clienterId=c.Id
                                    LEFT JOIN OrderOther oo WITH (NOLOCK) ON oo.OrderId=o.Id
                                    LEFT JOIN [group] g WITH ( NOLOCK ) ON g.Id = o.orderfrom
                                    WHERE 1=1 AND o.OrderNo=@OrderNo;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderNo", SqlDbType.NVarChar).Value = orderNo;
            parm.Add("@OrderId", SqlDbType.Int).Value = orderId;
            //获取订单详情信息
            sql.Append(@"
select  od.Id ,
        od.OrderNo ,
        od.ProductName ,
        od.UnitPrice ,
        od.Quantity ,
        od.InsertTime ,
        od.FormDetailID ,
        od.GroupID
from    dbo.OrderDetail od ( nolock )
where   od.OrderNo = @OrderNo;");
            //获取子订单信息
            sql.Append(@"
select  oc.Id ,
        oc.OrderId ,
        oc.ChildId ,
        oc.TotalPrice ,
        oc.GoodPrice ,
        oc.DeliveryPrice ,
        oc.PayStyle ,
        oc.PayType ,
        oc.PayStatus ,
        oc.PayBy ,
        oc.PayTime ,
        oc.PayPrice ,
        oc.HasUploadTicket ,
        oc.TicketUrl ,
        oc.CreateBy ,
        oc.CreateTime ,
        oc.UpdateBy ,
        oc.UpdateTime
from    dbo.OrderChild oc ( nolock )
where   oc.OrderId = @OrderId;
");
            #endregion
            var ds = DbHelper.ExecuteDataset(SuperMan_Read, sql.ToString(), parm);

            var order = ConvertDataTableList<OrderListModel>(ds.Tables[0]);
            if (order != null && order.Count > 0)
            {
                var orderDetailList = ConvertDataTableList<Ets.Model.DataModel.Order.OrderDetail>(ds.Tables[1]);
                var orderChildList = ConvertDataTableList<OrderChild>(ds.Tables[2]);

                order[0].OrderDetailList = orderDetailList.ToList();
                order[0].OrderChildList = orderChildList.ToList();
                return order[0];
            }
            return new OrderListModel();
        }
        /// <summary>
        /// 获取订单操作流水
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IList<OrderSubsidiesLog> GetOrderOptionLog(int orderId)
        {
            string sql = @"  SELECT  Id,
                                    OrderId,
                                    OrderStatus,
                                    OptId,
                                    OptName,
                                    InsertTime,
                                    Platform,
                                    Remark
                            FROM OrderSubsidiesLog(nolock)
                            WHERE OrderId=@OrderId
                            ORDER BY Id;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OrderId", orderId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<OrderSubsidiesLog>(dt);
        }
    }
}
