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
set GrabLongitude=@GrabLongitude,GrabLatitude=@GrabLatitude where orderid=(
select id from dbo.[order] where OrderNo=@OrderNo
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
        /// <param name="orderNo"></param>
        /// <param name="completeLongitude"></param>
        /// <param name="completeLatitude"></param>
        public void UpdateComplete(string orderNo, float completeLongitude, float completeLatitude)
        {
            const string UPDATE_SQL = @"
update OrderOther 
set CompleteLongitude=@CompleteLongitude,CompleteLatitude=@CompleteLatitude where orderid=(
select id from dbo.[order] where OrderNo=@OrderNo
)";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@CompleteLongitude", completeLongitude);
            dbParameters.AddWithValue("@CompleteLatitude", completeLatitude);
            dbParameters.AddWithValue("@orderNo", orderNo);
            DbHelper.ExecuteNonQuery(SuperMan_Write, UPDATE_SQL, dbParameters);
        }
    }

}
