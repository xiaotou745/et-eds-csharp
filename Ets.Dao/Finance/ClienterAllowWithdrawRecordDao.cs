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
using Ets.Model.DomainModel.Finance;
using ETS.Util;
using Ets.Model.ParameterModel.Finance;
#endregion

namespace Ets.Dao.Finance
{
    /// <summary>
    /// 数据访问类ClienterAllowWithdrawRecordDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-08-03 15:34:49
    /// </summary>
    public class ClienterAllowWithdrawRecordDao : DaoBase
    {
        /// <summary>
        /// 增加一条记录
        /// </summary>
        public long Insert(ClienterAllowWithdrawRecord clienterAllowWithdrawRecord)
        {
            const string insertSql = @"
insert into ClienterAllowWithdrawRecord(ClienterId,Amount,Status,Balance,RecordType,Operator,WithwardId,RelationNo,Remark)
values(@ClienterId,@Amount,@Status,@Balance,@RecordType,@Operator,@WithwardId,@RelationNo,@Remark)

select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterAllowWithdrawRecord.ClienterId);
            dbParameters.AddWithValue("Amount", clienterAllowWithdrawRecord.Amount);
            dbParameters.AddWithValue("Status", clienterAllowWithdrawRecord.Status);
            dbParameters.AddWithValue("Balance", clienterAllowWithdrawRecord.Balance);
            dbParameters.AddWithValue("RecordType", clienterAllowWithdrawRecord.RecordType);
            dbParameters.AddWithValue("Operator", clienterAllowWithdrawRecord.Operator);
            dbParameters.AddWithValue("WithwardId", clienterAllowWithdrawRecord.WithwardId);
            dbParameters.AddWithValue("RelationNo", clienterAllowWithdrawRecord.RelationNo);
            dbParameters.AddWithValue("Remark", clienterAllowWithdrawRecord.Remark);

            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToLong(result);
        }

    }
}

