using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
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


    }
}
