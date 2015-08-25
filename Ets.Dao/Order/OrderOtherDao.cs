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
        public void UpdateAuditStatus(int orderId, int auditstatus)
        {
            string updateSql = @"update dbo.OrderOther set auditstatus = @auditstatus where OrderId=@orderId";
            IDbParameters parm = DbHelper.CreateDbParameters("orderId", DbType.Int32, 4, orderId);
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
        public void UpdateOrderIsReal(int orderId, string deductCommissionReason,int deductCommissionType)
        {
            const string updateSql = @"
update OrderOther 
set IsNotRealOrder=1, DeductCommissionReason=@DeductCommissionReason,
DeductCommissionType=@DeductCommissionType
where orderid=@orderId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderId", orderId);
            dbParameters.AddWithValue("@DeductCommissionReason", deductCommissionReason);
            dbParameters.AddWithValue("@DeductCommissionType", deductCommissionType);
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
    }
}
