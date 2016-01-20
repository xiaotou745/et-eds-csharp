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
using ETS.Enums;
namespace Ets.Dao.Order
{
    /// <summary>
    /// 订单OrderOther 数据访问类OrderOtherDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 18:48:39
    /// </summary>
    public class OrderOtherDao : DaoBase
    {
        public OrderOtherDao()
        { }

        /// <summary>
        /// 更新抢单记录
        /// 胡灵波
        /// 2015年8月18日 17:47:35
        /// </summary>
        public void UpdateGrab(OrderCompleteModel parModel)
        {
            const string updateSql = @"
update OrderOther 
set GrabTime=GETDATE(), GrabLongitude=@GrabLongitude,
GrabLatitude=@GrabLatitude,IsGrabTimely=@IsGrabTimely
where orderid=(
select id from dbo.[order](nolock) where OrderNo=@OrderNo
)";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@GrabLongitude", parModel.Longitude);
            dbParameters.AddWithValue("@grabLatitude", parModel.Latitude);
            dbParameters.AddWithValue("@IsGrabTimely", parModel.IsTimely);
            dbParameters.AddWithValue("@orderNo", parModel.orderNo);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 更新完成记录
        /// 胡灵波
        /// 2015年8月18日 17:47:58
        /// </summary>   
        /// <param name="orderNo"></param>
        /// <param name="completeLongitude"></param>
        /// <param name="completeLatitude"></param>
        public void UpdateComplete(OrderCompleteModel parModel)
        {
            const string updateSql = @"
update OrderOther 
set CompleteLongitude=@CompleteLongitude,CompleteLatitude=@CompleteLatitude,
IsCompleteTimely=@IsCompleteTimely
where orderid=(
select id from dbo.[order] where OrderNo=@OrderNo
)";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@CompleteLongitude", parModel.Longitude);
            dbParameters.AddWithValue("@CompleteLatitude", parModel.Latitude);
            dbParameters.AddWithValue("@IsCompleteTimely", parModel.IsTimely);
            dbParameters.AddWithValue("@orderNo", parModel.orderNo);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 更新已提现状态
        /// 胡灵波
        /// 2015年8月13日 11:54:51
        /// </summary>
        /// <param name="orderId"></param>
        public void UpdateJoinWithdraw(int orderId)
        {
            const string updateSql = @"
update OrderOther set IsJoinWithdraw=1 where orderId=@orderId";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderId", orderId);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 更新审核状态
        /// 胡灵波
        /// 2015年8月12日 10:13:16
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="auditstatus"></param>
        /// <param name="auditOptName">订单审核操作人</param>
        public void UpdateAuditStatus(int orderId, int auditstatus,string auditOptName)
        {
            if (auditOptName == null) auditOptName = "";
            string updateSql = @"update dbo.OrderOther set auditstatus = @auditstatus,AuditDate=getdate(),AuditOptName=@AuditOptName where OrderId=@orderId";
            IDbParameters parm = DbHelper.CreateDbParameters("orderId", DbType.Int32, 4, orderId);
            parm.Add("AuditOptName", DbType.String, 30).Value = auditOptName;
            parm.AddWithValue("@auditstatus", auditstatus);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, parm);
        }
        /// <summary>
        /// 更新订单是否无效的标记
        /// zhaohailong20150706
        /// 修改人：胡灵波
        /// 2015年8月18日 17:54:54
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="DeductCommissionReason">扣除订单补贴原因(如果需要扣除补贴需要此信息)</param>
        public void UpdateOrderIsReal(OrderOtherPM  orderOtherPM)
        {
            const string updateSql = @"
update OrderOther 
set IsNotRealOrder=1, DeductCommissionReason=@DeductCommissionReason,
DeductCommissionType=@DeductCommissionType
where orderid=@orderId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderId", orderOtherPM.OrderId);
            dbParameters.AddWithValue("@DeductCommissionReason", orderOtherPM.DeductCommissionReason);
            dbParameters.AddWithValue("@DeductCommissionType", orderOtherPM.DeductCommissionType);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 更新取消时间
        /// 胡灵波
        /// 2015年8月18日 17:57:11
        /// </summary>
        /// <param name="orderId"></param>
        public bool UpdateCancelTime(int orderId)
        {
            const string updateSql = @"
update OrderOther set CancelTime=getdate() where orderId=@orderId";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderId", orderId);            

            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters) > 0 ? true : false;
        }


        /// <summary>
        /// 根据订单号查子订单信息
        /// caoheyang 2015117
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public OrderOther GetByOrderNo(string orderNo)
        {
            string sql = @"SELECT 
o.PubLongitude
,o.PubLatitude
,o.GrabTime
,o.GrabLongitude
,o.GrabLatitude
,o.CompleteLongitude
,o.CompleteLatitude
,o.TakeTime
,o.TakeLongitude
,o.TakeLatitude
,o.DeductCommissionType
,o.AuditStatus
,o.CancelTime
,o.AuditDate
,o.AuditOptName  FROM dbo.OrderOther (nolock )  o   join [order](nolock ) a  on o.OrderId=a.Id  where  a.OrderNo=  @OrderNo ";

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderNo", SqlDbType.NVarChar);
            parm.SetValue("@OrderNo", orderNo);
       
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            return ConvertDataTableList<OrderOther>(dt)[0];

        }
  

        public int Insert(OrderOtherModel orderOther)
        {

            const string insertSql = @"
insert into OrderOther(OrderId,NeedUploadCount,ReceiptPic,HadUploadCount,IsJoinWithdraw,
PubLongitude,PubLatitude,GrabTime,GrabLongitude,GrabLatitude,CompleteLongitude,CompleteLatitude,
TakeTime,TakeLongitude,TakeLatitude,
OneKeyPubOrder,IsNotRealOrder,DeductCommissionReason,DeductCommissionType,AuditStatus,IsOrderChecked,
CancelTime,IsAllowCashPay,IsPubDateTimely,IsGrabTimely,IsTakeTimely,IsCompleteTimely,AuditDate,AuditOptName,
DeliveryOrderNo,NotifyTime,EndTime,ExpectedTakeTime,ExpectedDelivery,ReceiptId
)
values(@OrderId,@NeedUploadCount,@ReceiptPic,@HadUploadCount,@IsJoinWithdraw,@PubLongitude,
@PubLatitude,@GrabTime,@GrabLongitude,@GrabLatitude,@CompleteLongitude,@CompleteLatitude
,@TakeTime,@TakeLongitude,@TakeLatitude,
@OneKeyPubOrder,@IsNotRealOrder,@DeductCommissionReason,@DeductCommissionType,@AuditStatus,@IsOrderChecked,
@CancelTime,@IsAllowCashPay,@IsPubDateTimely,@IsGrabTimely,@IsTakeTimely,@IsCompleteTimely,@AuditDate,@AuditOptName,
@DeliveryOrderNo,@NotifyTime,@EndTime,@ExpectedTakeTime,@ExpectedDelivery,@ReceiptId
)

select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("OrderId", orderOther.OrderId);
            dbParameters.AddWithValue("NeedUploadCount", orderOther.NeedUploadCount);
            dbParameters.AddWithValue("ReceiptPic", orderOther.ReceiptPic);
            dbParameters.AddWithValue("HadUploadCount", orderOther.HadUploadCount);
            dbParameters.AddWithValue("IsJoinWithdraw", orderOther.IsJoinWithdraw);
            dbParameters.AddWithValue("PubLongitude", orderOther.PubLongitude);
            dbParameters.AddWithValue("PubLatitude", orderOther.PubLatitude);
            dbParameters.AddWithValue("GrabTime", orderOther.GrabTime);
            dbParameters.AddWithValue("GrabLongitude", orderOther.GrabLongitude);
            dbParameters.AddWithValue("GrabLatitude", orderOther.GrabLatitude);
            dbParameters.AddWithValue("CompleteLongitude", orderOther.CompleteLongitude);
            dbParameters.AddWithValue("CompleteLatitude", orderOther.CompleteLatitude);
            dbParameters.AddWithValue("TakeTime", orderOther.TakeTime);
            dbParameters.AddWithValue("TakeLongitude", orderOther.TakeLongitude);
            dbParameters.AddWithValue("TakeLatitude", orderOther.TakeLatitude);      
            dbParameters.AddWithValue("OneKeyPubOrder", orderOther.OneKeyPubOrder);
            dbParameters.AddWithValue("IsNotRealOrder", orderOther.IsNotRealOrder);
            dbParameters.AddWithValue("DeductCommissionReason", orderOther.DeductCommissionReason);
            dbParameters.AddWithValue("DeductCommissionType", orderOther.DeductCommissionType);
            dbParameters.AddWithValue("AuditStatus", orderOther.AuditStatus);
            dbParameters.AddWithValue("IsOrderChecked", orderOther.IsOrderChecked);
            dbParameters.AddWithValue("CancelTime", orderOther.CancelTime);
            dbParameters.AddWithValue("IsAllowCashPay", orderOther.IsAllowCashPay);
            dbParameters.AddWithValue("IsPubDateTimely", orderOther.IsPubDateTimely);
            dbParameters.AddWithValue("IsGrabTimely", orderOther.IsGrabTimely);
            dbParameters.AddWithValue("IsTakeTimely", orderOther.IsTakeTimely);
            dbParameters.AddWithValue("IsCompleteTimely", orderOther.IsCompleteTimely);
            dbParameters.AddWithValue("AuditDate", orderOther.AuditDate);
            dbParameters.AddWithValue("AuditOptName", orderOther.AuditOptName);
            dbParameters.AddWithValue("DeliveryOrderNo", orderOther.DeliveryOrderNo);
            dbParameters.AddWithValue("NotifyTime", orderOther.NotifyTime);
            dbParameters.AddWithValue("EndTime", orderOther.EndTime);
            dbParameters.AddWithValue("ExpectedTakeTime", orderOther.ExpectedTakeTime);
            dbParameters.AddWithValue("ExpectedDelivery", orderOther.ExpectedDelivery);
            dbParameters.AddWithValue("ReceiptId", orderOther.ReceiptId);

            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            if (result == null)
            {
                return 0;
            }
            return int.Parse(result.ToString());            
        }

        public bool IsExistByOrderNo(string deliveryOrderNo)
        {
            bool isExist;

            const string querysql = @"
select  count(1) 
from  dbo.[orderother]  
where DeliveryOrderNo=@DeliveryOrderNo";
            IDbParameters dbSelectParameters = DbHelper.CreateDbParameters();
            dbSelectParameters.AddWithValue("DeliveryOrderNo", deliveryOrderNo);            
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Write, querysql, dbSelectParameters);
            isExist = ParseHelper.ToInt(executeScalar, 0) > 0;

            return isExist;
        }
    }
}
