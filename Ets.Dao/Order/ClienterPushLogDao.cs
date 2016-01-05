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
using ETS.Util;

namespace Ets.Dao.Order
{
  public  class ClienterPushLogDao : DaoBase
    {
        /// <summary>
        /// 增加一条记录
        /// </summary>
        public long Insert(ClienterPushLog clienterPushLog)
        {
            const string insertSql = @"
insert into ClienterPushLog(OrderId,ClienterIds)
values(@OrderId,@ClienterIds)

select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("OrderId", clienterPushLog.OrderId);
            dbParameters.AddWithValue("ClienterIds", clienterPushLog.ClienterIds);

            return ParseHelper.ToLong(DbHelper.ExecuteScalar(base.SuperMan_Write, insertSql, dbParameters));
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        public void Update(ClienterPushLog clienterPushLog)
        {
            const string updateSql = @"
update  ClienterPushLog
set  OrderId=@OrderId,ClienterIds=@ClienterIds
where  ID=@ID ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ID", clienterPushLog.ID);
            dbParameters.AddWithValue("OrderId", clienterPushLog.OrderId);
            dbParameters.AddWithValue("ClienterIds", clienterPushLog.ClienterIds);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 查询某个订单的推送记录  add by caoheyang  20160104
        /// </summary>
        public ClienterPushLog SelectByOrderId(long orderId)
        {
            const string sql = @"
            select ID,OrderId,ClienterIds,CreateTime,ProcessTime  from ClienterPushLog cpl(nolock) where OrderId=@OrderId ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("OrderId", orderId);
             DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, dbParameters);
             if (!dt.HasData())
             {
                 return null;
             }
             return MapRows<ClienterPushLog>(dt)[0];
        }
       /// <summary>
        /// 新 订单处理后（接单取消订单）推送时间  add by caoheyang  20160104
        /// </summary>
        public void UpdateProcessTime(long id)
        {
            const string updateSql = @"
update ClienterPushLog
		set ProcessTime=getdate() 
where  ID=@ID ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ID", id);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

    }
}
