using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using Ets.Model.DataModel.Order;

namespace Ets.Dao.Order
{
    /// <summary>
    /// 数据访问类OpenCallBackLogDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2016-02-18 10:40:23
    /// </summary>
    public class OpenCallBackLogDao : DaoBase
    {
        public OpenCallBackLogDao()
        {

        }

        #region IOpenCallBackLogDao  Members

        /// <summary>
        /// 增加一条记录
        /// </summary>
        public int Insert(OpenCallBackLog openCallBackLog)
        {
            const string insertSql = @"
insert into OpenCallBackLog(Url,OrderId,OrderNo,RequestBody,ResponseBody,Status)
values(@Url,@OrderId,@OrderNo,@RequestBody,@ResponseBody,@Status)

select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Url", openCallBackLog.Url);
            dbParameters.AddWithValue("OrderId", openCallBackLog.OrderId);
            dbParameters.AddWithValue("OrderNo", openCallBackLog.OrderNo);
            dbParameters.AddWithValue("RequestBody", openCallBackLog.RequestBody);
            dbParameters.AddWithValue("ResponseBody", openCallBackLog.ResponseBody);
            dbParameters.AddWithValue("Status", openCallBackLog.Status);
            object result = DbHelper.ExecuteScalar(base.SuperMan_Write, insertSql, dbParameters);
            if (result == null)
            {
                return 0;
            }
            return int.Parse(result.ToString());
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        public void Update(OpenCallBackLog openCallBackLog)
        {
            const string updateSql = @"
update  OpenCallBackLog
set  Url=@Url,OrderId=@OrderId,OrderNo=@OrderNo,RequestBody=@RequestBody,ResponseBody=@ResponseBody,Status=@Status,CreateTime=@CreateTime
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", openCallBackLog.Id);
            dbParameters.AddWithValue("Url", openCallBackLog.Url);
            dbParameters.AddWithValue("OrderId", openCallBackLog.OrderId);
            dbParameters.AddWithValue("OrderNo", openCallBackLog.OrderNo);
            dbParameters.AddWithValue("RequestBody", openCallBackLog.RequestBody);
            dbParameters.AddWithValue("ResponseBody", openCallBackLog.ResponseBody);
            dbParameters.AddWithValue("Status", openCallBackLog.Status);
            dbParameters.AddWithValue("CreateTime", openCallBackLog.CreateTime);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        public void Delete(int id)
        {
            const string deleteSql = @"delete from OpenCallBackLog where Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            DbHelper.ExecuteNonQuery(SuperMan_Write, deleteSql, dbParameters);
        }
        #endregion
    }

}
