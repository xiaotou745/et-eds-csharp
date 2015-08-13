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
        /// 更新一条记录
        /// </summary>
        public void UpdateGrab(string orderNo, float grabLongitude, float grabLatitude)
        {
            const string UPDATE_SQL = @"
update OrderOther 
set GrabTime=GETDATE(), GrabLongitude=@GrabLongitude,GrabLatitude=@GrabLatitude where orderid=(
select id from dbo.[order](nolock) where OrderNo=@OrderNo
)";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@GrabLongitude", grabLongitude);
            dbParameters.AddWithValue("@grabLatitude", grabLatitude);
            dbParameters.AddWithValue("@orderNo", orderNo);
            DbHelper.ExecuteNonQuery(SuperMan_Write, UPDATE_SQL, dbParameters);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150701</UpdateTime>
        /// <param name="orderNo"></param>
        /// <param name="completeLongitude"></param>
        /// <param name="completeLatitude"></param>
        public void UpdateComplete(OrderCompleteModel parModel)
        {
            const string UPDATE_SQL = @"
update OrderOther 
set CompleteLongitude=@CompleteLongitude,CompleteLatitude=@CompleteLatitude where orderid=(
select id from dbo.[order] where OrderNo=@OrderNo
)";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@CompleteLongitude", parModel.Longitude);
            dbParameters.AddWithValue("@CompleteLatitude", parModel.Latitude);
            dbParameters.AddWithValue("@orderNo", parModel.orderNo);
            DbHelper.ExecuteNonQuery(SuperMan_Write, UPDATE_SQL, dbParameters);
        }

        /// <summary>
        /// 更新已提现
        /// 胡灵波
        /// 2015年8月13日 11:54:51
        /// </summary>
        /// <param name="orderId"></param>
        public void UpdateJoinWithdraw(int orderId)
        {
            const string UPDATE_SQL = @"
update OrderOther set IsJoinWithdraw=1 where orderId=@orderId";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderId", orderId);
            DbHelper.ExecuteNonQuery(SuperMan_Write, UPDATE_SQL, dbParameters);
        }

        ///// <summary>
        ///// 更新订单是否加入已提现
        ///// 窦海超
        ///// 2015年5月15日 17:08:27
        ///// </summary>
        ///// <param name="orderId"></param>
        //public void UpdateJoinWithdraw(int orderId)
        //{
        //    string sql = @"update dbo.OrderOther set IsJoinWithdraw = 1 where OrderId=@orderId";
        //    IDbParameters parm = DbHelper.CreateDbParameters("orderId", DbType.Int32, 4, orderId);
        //    DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm);
        //}
        /// <summary>
        /// 更新审核状态
        /// 胡灵波
        /// 2015年8月12日 10:13:16
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="auditstatus"></param>
        public void UpdateAuditStatus(int orderId, int auditstatus)
        {
            string sql = @"update dbo.OrderOther set auditstatus = @auditstatus where OrderId=@orderId";
            IDbParameters parm = DbHelper.CreateDbParameters("orderId", DbType.Int32, 4, orderId);
            parm.AddWithValue("@auditstatus", auditstatus);
            DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm);
        }
        /// <summary>
        /// 更新订单是否无效的标记
        /// zhaohailong20150706
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="DeductCommissionReason">扣除订单补贴原因(如果需要扣除补贴需要此信息)</param>
        public void UpdateOrderIsReal(int orderId, string deductCommissionReason)
        {
            const string UPDATE_SQL = @"
                                        update OrderOther 
                                        set IsNotRealOrder=1,
                                        DeductCommissionReason=@DeductCommissionReason,
                                        DeductCommissionType=1
                                        where orderid=@orderId
                                        ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderId", orderId);
            dbParameters.AddWithValue("@DeductCommissionReason", deductCommissionReason);
            DbHelper.ExecuteNonQuery(SuperMan_Write, UPDATE_SQL, dbParameters);
        }

        /// <summary>
        /// 更新订单扣除补贴原因
        /// 彭宜20150803
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="deductCommissionReason">扣除订单补贴原因</param>
        /// <param name="deductCommissionType">扣除订单补贴方式   1 自动扣除    2手动扣除</param>
        public void UpdateOrderDeductCommissionReason(int orderId, string deductCommissionReason, int deductCommissionType)
        {
            const string UPDATE_SQL = @"
                                        update OrderOther 
                                        set DeductCommissionReason=@DeductCommissionReason,
                                        DeductCommissionType=@DeductCommissionType
                                        where orderid=@orderId
                                        ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderId", orderId);
            dbParameters.AddWithValue("@DeductCommissionReason", deductCommissionReason);
            dbParameters.AddWithValue("@DeductCommissionType", deductCommissionType);
            DbHelper.ExecuteNonQuery(SuperMan_Write, UPDATE_SQL, dbParameters);
        }

//        /// <summary>
//        /// 获取是否无效订单
//        /// </summary>
//        /// <UpdateBy>hulingbo</UpdateBy>
//        /// <UpdateTime>20150706</UpdateTime>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public int GetIsNotRealOrder(int orderId)
//        {
//            string querysql = @"  
//select IDbParameters from OrderOther nolock
//where OrderId=@OrderId";

//            IDbParameters dbParameters = DbHelper.CreateDbParameters();
//            dbParameters.AddWithValue("@OrderId", orderId);

//            object obj = DbHelper.ExecuteScalar(SuperMan_Read, querysql, dbParameters);
//            return ParseHelper.ToInt(obj, 0);
//        }

    }
}
