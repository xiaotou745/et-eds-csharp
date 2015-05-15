using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Finance
{
    public class ClienterFinanceDao : DaoBase
    {
        /// <summary>
        /// 根据参数获取骑士提现申请单列表
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetClienterWithdrawList<T>(ClienterWithdrawSearchCriteria criteria)
        {
            string columnList = @"  cwf.Id,
                                    cwf.WithwardNo,
                                    c.[TrueName] ClienterName,
                                    c.PhoneNo ClienterPhoneNo,
                                    cwf.BalancePrice,
                                    cwf.AllowWithdrawPrice,
                                    cwf.Amount,
                                    cwf.Balance,
                                    cwf.Status,
                                    cwf.WithdrawTime,
                                    cwf.Auditor,
                                    cwf.AuditTime,
                                    cwf.Payer,
                                    cwf.PayTime,
                                    cwf.AuditFailedReason,
                                    cwf.PayFailedReason ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(criteria.ClienterName))
            {
                sbSqlWhere.AppendFormat(" AND c.[TrueName]='{0}' ", criteria.ClienterName.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.ClienterPhoneNo))
            {
                sbSqlWhere.AppendFormat(" AND c.PhoneNo='{0}' ", criteria.ClienterPhoneNo.Trim());
            }
            if (criteria.WithdrawStatus != 0)
            {
                sbSqlWhere.AppendFormat(" AND cwf.Status={0} ", criteria.WithdrawStatus);
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithwardNo))
            {
                sbSqlWhere.AppendFormat(" AND cwf.WithwardNo='{0}' ", criteria.WithwardNo.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateStart))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),cwf.WithdrawTime,120)>=CONVERT(CHAR(10),'{0}',120) ", criteria.WithdrawDateStart.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateEnd))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),cwf.WithdrawTime,120)<=CONVERT(CHAR(10),'{0}',120) ", criteria.WithdrawDateEnd.Trim());
            }
            string tableList = @" ClienterWithdrawForm cwf with(nolock)
                                  join clienter c with(nolock) on cwf.ClienterId=c.Id";
            string orderByColumn = " cwf.Id DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }

        /// <summary>
        /// 根据申请单Id获取骑士提现申请单
        /// danny-20150513
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ClienterWithdrawFormModel GetClienterWithdrawListById(string withwardId)
        {
            string sql = @"  
select cwf.Id,
       cwf.WithwardNo,
       cwf.ClienterId,
       c.[TrueName] ClienterName,
       c.PhoneNo ClienterPhoneNo,
       c.HasWithdrawPrice,
       cwf.BalancePrice,
       cwf.AllowWithdrawPrice,
       cwf.Amount,
       cwf.Balance,
       cwf.Status,
       cwf.WithdrawTime,
       cwf.Auditor,
       cwf.AuditTime,
       cwf.Payer,
       cwf.PayTime,
       cwf.AuditFailedReason,
       cwf.PayFailedReason,
       cwf.OpenBank,
       cwf.OpenSubBank,
       cwf.TrueName,
       cwf.AccountNo
from ClienterWithdrawForm cwf with(nolock)
  join clienter c with(nolock) on cwf.ClienterId=c.Id and cwf.Id=@Id  ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Id", withwardId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<ClienterWithdrawFormModel>(dt)[0];
        }
        /// <summary>
        /// 获取骑士提款单操作日志
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        public IList<ClienterWithdrawLog> GetClienterWithdrawOptionLog(string withwardId)
        {
            string sql = @"  
SELECT [Id]
      ,[WithwardId]
      ,[Status]
      ,[Remark]
      ,[Operator]
      ,[OperatTime]
FROM ClienterWithdrawLog cwl with(nolock)
WHERE WithwardId=@Id
ORDER BY Id;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Id", withwardId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<ClienterWithdrawLog>(dt);
        }
        /// <summary>
        /// 审核骑士提现申请单
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawAudit(ClienterWithdrawLog model)
        {
            string sql = string.Format(@" 
UPDATE ClienterWithdrawForm
 SET    [Status] = @Status,
		Auditor=@Operator,
		AuditTime=getdate()
OUTPUT
  Inserted.Id,
  Inserted.[Status],
  @Remark,
  @Operator,
  getdate()
INTO ClienterWithdrawLog
  ([WithwardId],
  [Status],
  [Remark],
  [Operator],
  [OperatTime])
 WHERE  Id = @Id");

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@Id", model.WithwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 骑士提现申请单确认打款
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawPayOk(ClienterWithdrawLog model)
        {
            string sql = string.Format(@" 
UPDATE ClienterWithdrawForm
 SET    [Status] = @Status,
		Payer=@Operator,
		PayTime=getdate()
OUTPUT
  Inserted.Id,
  Inserted.[Status],
  @Remark,
  @Operator,
  getdate()
INTO ClienterWithdrawLog
  ([WithwardId],
  [Status],
  [Remark],
  [Operator],
  [OperatTime])
 WHERE  Id = @Id");

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@Id", model.WithwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 骑士提现申请单审核拒绝
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawAuditRefuse(ClienterWithdrawLogModel model)
        {
            string sql = string.Format(@" 
UPDATE ClienterWithdrawForm
 SET    [Status] = @Status,
		Auditor=@Operator,
		AuditTime=getdate(),
        AuditFailedReason=@AuditFailedReason
OUTPUT
  Inserted.Id,
  Inserted.[Status],
  @Remark,
  @Operator,
  getdate()
INTO ClienterWithdrawLog
  ([WithwardId],
  [Status],
  [Remark],
  [Operator],
  [OperatTime])
 WHERE  Id = @Id");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@AuditFailedReason", model.AuditFailedReason);
            parm.AddWithValue("@Id", model.WithwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        /// <summary>
        /// 骑士提现申请单打款失败
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawPayFailed(ClienterWithdrawLogModel model)
        {
            string sql = string.Format(@" 
UPDATE ClienterWithdrawForm
 SET    [Status] = @Status,
		Payer=@Operator,
		PayTime=getdate(),
        PayFailedReason=@PayFailedReason
OUTPUT
  Inserted.Id,
  Inserted.[Status],
  @Remark,
  @Operator,
  getdate()
INTO ClienterWithdrawLog
  ([WithwardId],
  [Status],
  [Remark],
  [Operator],
  [OperatTime])
 WHERE  Id = @Id");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@PayFailedReason", model.PayFailedReason);
            parm.AddWithValue("@Id", model.WithwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 修改骑士提款流水状态
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyClienterBalanceRecordStatus(string withwardId)
        {
            string sql = string.Format(@" 
UPDATE ClienterBalanceRecord
 SET    [Status] = @Status
 WHERE  WithwardId = @WithwardId AND [Status]=2;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@WithwardId", withwardId);
            parm.AddWithValue("@Status", ETS.Enums.ClienterBalanceRecordStatus.Success.GetHashCode());
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        /// <summary>
        /// 骑士提现失败后返现
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawReturn(ClienterWithdrawLogModel model)
        {
            string sql = string.Format(@" 
insert into ClienterBalanceRecord
            ([ClienterId]
           ,[Amount]
           ,[Status]
           ,[Balance]
           ,[RecordType]
           ,[Operator]
           ,[OperateTime]
           ,[WithwardId]
           ,[RelationNo]
           ,[Remark])
select      cbr.[ClienterId]
           ,-ISNULL(cbr.[Amount],0) Amount
           ,@NewStatus [Status]
           ,-ISNULL(cbr.[Amount],0)+ISNULL(c.AccountBalance,0) Balance
           ,@NewRecordType [RecordType]
           ,@Operator
           ,getdate() OperateTime
           ,cbr.[WithwardId]
           ,cbr.[RelationNo]
           ,@Remark
 from ClienterBalanceRecord cbr (nolock)
    join clienter c (nolock) on c.Id=cbr.ClienterId
 where cbr.WithwardId=@WithwardId and cbr.Status=@Status and cbr.RecordType=@RecordType;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@WithwardId", model.WithwardId);
            parm.AddWithValue("@Status", ETS.Enums.ClienterBalanceRecordStatus.Tradeing.GetHashCode());
            parm.AddWithValue("@RecordType", ETS.Enums.ClienterBalanceRecordRecordType.Withdraw.GetHashCode());
            parm.AddWithValue("@NewStatus", ETS.Enums.ClienterBalanceRecordStatus.Success.GetHashCode());
            parm.AddWithValue("@NewRecordType", ETS.Enums.ClienterBalanceRecordRecordType.Return.GetHashCode());
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        /// <summary>
        /// 骑士提现失败后修改骑士表金额
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyClienterAmountInfo(string withwardId)
        {
            string sql = string.Format(@" 
update c
set    c.AccountBalance=ISNULL(c.AccountBalance, 0)+ISNULL(cwf.Amount,0),
       c.AllowWithdrawPrice=ISNULL(c.AllowWithdrawPrice,0)+ISNULL(cwf.Amount,0)
from clienter c
join [ClienterWithdrawForm] cwf on c.Id=cwf.ClienterId
where cwf.Id=@WithwardId;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@WithwardId", withwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 骑士提现打款确认后修改骑士表累计提款金额
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyClienterTotalAmount(string withwardId)
        {
            string sql = string.Format(@" 
update c
set    c.HasWithdrawPrice=ISNULL(c.HasWithdrawPrice, 0)+ISNULL(cwf.Amount,0)
from clienter c
join [ClienterWithdrawForm] cwf on c.Id=cwf.ClienterId
where cwf.Id=@WithwardId;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@WithwardId", withwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 获取骑士提款收支记录列表
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        public IList<ClienterBalanceRecord> GetClienterBalanceRecordList(ClienterBalanceRecordSerchCriteria criteria)
        {
            string sql = @"  
SELECT cbr.[Id]
      ,cbr.[ClienterId]
      ,cbr.[Amount]
      ,cbr.[Status]
      ,cbr.[Balance]
      ,cbr.[RecordType]
      ,cbr.[Operator]
      ,cbr.[OperateTime]
      ,cbr.[WithwardId]
      ,cbr.[RelationNo]
      ,cbr.[Remark]
  FROM [ClienterBalanceRecord] cbr WITH(NOLOCK)
WHERE ClienterId=@ClienterId ";
            if (criteria.RecordType != 0)
            {
                sql += @" AND cbr.[RecordType]=@RecordType";
            }
            if (!string.IsNullOrWhiteSpace(criteria.RelationNo))
            {
                sql += @" AND cbr.[RelationNo]=@RelationNo";
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeStart))
            {
                sql += @" AND CONVERT(CHAR(10),cbr.OperateTime,120)>=CONVERT(CHAR(10),@OperateTimeStart,120)";
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeEnd))
            {
                sql += @" AND CONVERT(CHAR(10),cbr.OperateTime,120)<=CONVERT(CHAR(10),@OperateTimeEnd,120)";
            }
            sql += " ORDER BY cbr.Id DESC";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ClienterId", criteria.ClienterId);
            parm.AddWithValue("@RecordType", criteria.RecordType);
            parm.AddWithValue("@RelationNo", criteria.RelationNo);
            parm.AddWithValue("@OperateTimeStart", criteria.OperateTimeStart);
            parm.AddWithValue("@OperateTimeEnd", criteria.OperateTimeEnd);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<ClienterBalanceRecord>(dt);
        }

        /// <summary>
        /// 获取要导出的骑士提现申请单
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<ClienterWithdrawFormModel> GetClienterWithdrawForExport(ClienterWithdrawSearchCriteria criteria)
        {
            string sql = @"  
select c.[TrueName] ClienterName,
       c.PhoneNo ClienterPhoneNo,
       cwf.OpenBank ,
       cwf.TrueName ,
       cwf.AccountNo ,
       cwf.Amount 
from ClienterWithdrawForm cwf with(nolock)
  join clienter c with(nolock) on cwf.ClienterId=c.Id 
where 1=1";
            if (!string.IsNullOrWhiteSpace(criteria.ClienterName))
            {
                sql += " AND c.[TrueName]=@ClienterName";
            }
            if (!string.IsNullOrWhiteSpace(criteria.ClienterPhoneNo))
            {
                sql += " AND c.PhoneNo=@ClienterPhoneNo";
            }
            if (criteria.WithdrawStatus != 0)
            {
                sql += " AND cwf.Status=@WithdrawStatus";
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithwardNo))
            {
                sql += " AND cwf.WithwardNo=@WithwardNo";
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateStart))
            {
                sql += " AND CONVERT(CHAR(10),cwf.WithdrawTime,120)>=CONVERT(CHAR(10),@WithdrawDateStart,120)";
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateEnd))
            {
                sql += " AND CONVERT(CHAR(10),cwf.WithdrawTime,120)<=CONVERT(CHAR(10),@WithdrawDateEnd,120)";
            }
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ClienterName", criteria.ClienterName);
            parm.AddWithValue("@ClienterPhoneNo", criteria.ClienterPhoneNo);
            parm.AddWithValue("@WithwardNo", criteria.WithwardNo);
            parm.AddWithValue("@WithdrawDateStart", criteria.WithdrawDateStart);
            parm.AddWithValue("@WithdrawDateEnd", criteria.WithdrawDateEnd);
            parm.AddWithValue("@WithdrawStatus", criteria.WithdrawStatus);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<ClienterWithdrawFormModel>(dt);
        }
        /// <summary>
        /// 获取要导出的骑士提款收支记录列表
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        public IList<ClienterBalanceRecordModel> GetClienterBalanceRecordListForExport(ClienterBalanceRecordSerchCriteria criteria)
        {
            string sql = @"  
SELECT cbr.[Id]
      ,cbr.[ClienterId]
      ,cbr.[Amount]
      ,cbr.[Status]
      ,cbr.[Balance]
      ,cbr.[RecordType]
      ,cbr.[Operator]
      ,cbr.[OperateTime]
      ,cbr.[WithwardId]
      ,cbr.[RelationNo]
      ,cbr.[Remark]
      ,cfa.AccountNo
      ,cfa.OpenBank
FROM [ClienterBalanceRecord] cbr WITH(NOLOCK)
LEFT JOIN ClienterFinanceAccount cfa WITH(NOLOCK) ON cbr.ClienterId=cfa.ClienterId
WHERE cbr.ClienterId=@ClienterId ";
            if (criteria.RecordType != 0)
            {
                sql += @" AND cbr.[RecordType]=@RecordType";
            }
            if (!string.IsNullOrWhiteSpace(criteria.RelationNo))
            {
                sql += @" AND cbr.[RelationNo]=@RelationNo";
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeStart))
            {
                sql += @" AND CONVERT(CHAR(10),cbr.OperateTime,120)>=CONVERT(CHAR(10),@OperateTimeStart,120)";
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeEnd))
            {
                sql += @" AND CONVERT(CHAR(10),cbr.OperateTime,120)<=CONVERT(CHAR(10),@OperateTimeEnd,120)";
            }
            sql += " ORDER BY cbr.Id DESC";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ClienterId", criteria.ClienterId);
            parm.AddWithValue("@RecordType", criteria.RecordType);
            parm.AddWithValue("@RelationNo", criteria.RelationNo);
            parm.AddWithValue("@OperateTimeStart", criteria.OperateTimeStart);
            parm.AddWithValue("@OperateTimeEnd", criteria.OperateTimeEnd);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<ClienterBalanceRecordModel>(dt);
        }


        /// <summary>
        /// 骑士提现失败后返现
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertClienterWithdraw(ClienterWithdrawLogModel model)
        {
            string sql = string.Format(@" 
insert into ClienterBalanceRecord
            ([ClienterId]
           ,[Amount]
           ,[Status]
           ,[Balance]
           ,[RecordType]
           ,[Operator]
           ,[OperateTime]
           ,[WithwardId]
           ,[RelationNo]
           ,[Remark])
values(    @ClienterId
           ,@Amount
           ,@NewStatus
           ,@Balance
           ,@NewRecordType
           ,@Operator
           ,getdate()
           ,@WithwardId
           ,@RelationNo
           ,@Remark");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@WithwardId", model.WithwardId);
            parm.AddWithValue("@Status", ETS.Enums.ClienterBalanceRecordStatus.Tradeing.GetHashCode());
            parm.AddWithValue("@RecordType", ETS.Enums.ClienterBalanceRecordRecordType.Withdraw.GetHashCode());
            parm.AddWithValue("@NewStatus", ETS.Enums.ClienterBalanceRecordStatus.Success.GetHashCode());
            parm.AddWithValue("@NewRecordType", ETS.Enums.ClienterBalanceRecordRecordType.Return.GetHashCode());
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
    }
}
