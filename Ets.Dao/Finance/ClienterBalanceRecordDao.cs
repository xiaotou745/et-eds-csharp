#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data;
using Ets.Model.DataModel.Finance;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using ETS.Util;
#endregion

namespace Ets.Dao.Finance
{
    /// <summary>
    /// 数据访问类ClienterBalanceRecordDao。
    /// Generate By: tools.etaoshi.com   caoheyang
    /// Generate Time: 2015-05-09 14:59:36
    /// </summary>
    public class ClienterBalanceRecordDao : DaoBase
    {
        public ClienterBalanceRecordDao()
        {

        }
        #region IClienterBalanceRecordRepos  Members

        /// <summary>
        /// 增加一条记录
        /// </summary>
        public long Insert(ClienterBalanceRecord ClienterBalanceRecord)
        {
            const string INSERT_SQL = @"
insert into ClienterBalanceRecord(ClienterId,Amount,Status,Balance,RecordType,Operator,OperateTime,RelationNo,Remark)
values(@ClienterId,@Amount,@Status,@Balance,@RecordType,@Operator,@OperateTime,@RelationNo,@Remark)
select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", ClienterBalanceRecord.ClienterId);
            dbParameters.AddWithValue("Amount", ClienterBalanceRecord.Amount);
            dbParameters.AddWithValue("Status", ClienterBalanceRecord.Status);
            dbParameters.AddWithValue("Balance", ClienterBalanceRecord.Balance);
            dbParameters.AddWithValue("RecordType", ClienterBalanceRecord.RecordType);
            dbParameters.AddWithValue("Operator", ClienterBalanceRecord.Operator);
            dbParameters.AddWithValue("OperateTime", ClienterBalanceRecord.OperateTime);
            dbParameters.AddWithValue("RelationNo", ClienterBalanceRecord.RelationNo);
            dbParameters.AddWithValue("Remark", ClienterBalanceRecord.Remark);
            object result = DbHelper.ExecuteScalar(SuperMan_Write, INSERT_SQL, dbParameters);
            return ParseHelper.ToLong(result);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        public void Update(ClienterBalanceRecord ClienterBalanceRecord)
        {
            const string UPDATE_SQL = @"
update  ClienterBalanceRecord
set  ClienterId=@ClienterId,Amount=@Amount,Status=@Status,Balance=@Balance,RecordType=@RecordType,Operator=@Operator,OperateTime=@OperateTime,RelationNo=@RelationNo,Remark=@Remark
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", ClienterBalanceRecord.Id);
            dbParameters.AddWithValue("ClienterId", ClienterBalanceRecord.ClienterId);
            dbParameters.AddWithValue("Amount", ClienterBalanceRecord.Amount);
            dbParameters.AddWithValue("Status", ClienterBalanceRecord.Status);
            dbParameters.AddWithValue("Balance", ClienterBalanceRecord.Balance);
            dbParameters.AddWithValue("RecordType", ClienterBalanceRecord.RecordType);
            dbParameters.AddWithValue("Operator", ClienterBalanceRecord.Operator);
            dbParameters.AddWithValue("OperateTime", ClienterBalanceRecord.OperateTime);
            dbParameters.AddWithValue("RelationNo", ClienterBalanceRecord.RelationNo);
            dbParameters.AddWithValue("Remark", ClienterBalanceRecord.Remark);

            DbHelper.ExecuteNonQuery(SuperMan_Write, UPDATE_SQL, dbParameters);
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
        public IList<ClienterBalanceRecord> Query(ClienterBalanceRecord ClienterBalanceRecord)
        {
            string condition = BindQueryCriteria(ClienterBalanceRecord);
            IList<ClienterBalanceRecord> models = new List<ClienterBalanceRecord>();
            string querysql = @"
select  Id,ClienterId,Amount,Status,Balance,RecordType,Operator,OperateTime,RelationNo,Remark
from  ClienterBalanceRecord (nolock)" + condition;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<ClienterBalanceRecord>(dt);
            }
            return models;
        }


        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClienterBalanceRecord GetById(long id)
        {
            ClienterBalanceRecord model = new ClienterBalanceRecord();
            const string querysql = @"
select  Id,ClienterId,Amount,Status,Balance,RecordType,Operator,OperateTime,RelationNo,Remark
from  ClienterBalanceRecord (nolock)
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<ClienterBalanceRecord>(dt)[0];
            }
            return model;
        }

        #endregion

        #region  Other Members

        /// <summary>
        /// 构造查询条件
        /// </summary>
        public static string BindQueryCriteria(ClienterBalanceRecord ClienterBalanceRecord)
        {
            var stringBuilder = new StringBuilder(" where 1=1 ");
            if (ClienterBalanceRecord == null)
            {
                return stringBuilder.ToString();
            }

            //TODO:在此加入查询条件构建代码

            return stringBuilder.ToString();
        }

        #endregion
    }
}

