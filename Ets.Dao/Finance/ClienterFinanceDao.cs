﻿using ETS.Enums;
using Ets.Model.Common.YeePay;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ets.Model.DataModel.Clienter;
using ETS.Util;

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
                                    cwf.PayFailedReason,
                                    cwf.HandCharge,
                                    cwf.HandChargeOutlay ";
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
        /// <param name="withwardId">提款单Id</param>
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
        /// <param name="withwardId">提款单Id</param>
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
 WHERE  Id = @Id and [Status]=1");

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@Id", model.WithwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
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
        PayFailedReason=ISNULL(PayFailedReason,'')+@PayFailedReason
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 修改骑士提款流水状态
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
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
            parm.AddWithValue("@Status", ClienterBalanceRecordStatus.Tradeing.GetHashCode());
            parm.AddWithValue("@RecordType", ClienterBalanceRecordRecordType.WithdrawApply.GetHashCode());
            parm.AddWithValue("@NewStatus", ClienterBalanceRecordStatus.Success.GetHashCode());
            parm.AddWithValue("@NewRecordType", model.Status == ClienterWithdrawFormStatus.TurnDown.GetHashCode() ? ClienterBalanceRecordRecordType.WithdrawRefuse : ClienterBalanceRecordRecordType.PayFailure);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

        /// <summary>
        /// 骑士提现失败后修改骑士表金额
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 骑士提现打款确认后修改骑士表累计提款金额
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 获取骑士提款收支记录列表
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
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
        /// 获取骑士提款收支记录列表分页版
        /// danny-20150604
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetClienterBalanceRecordListOfPaging<T>(ClienterBalanceRecordSerchCriteria criteria)
        {
            string columnList = @"   cbr.[Id]
                                    ,cbr.[ClienterId]
                                    ,cbr.[Amount]
                                    ,cbr.[Status]
                                    ,cbr.[Balance]
                                    ,cbr.[RecordType]
                                    ,cbr.[Operator]
                                    ,cbr.[OperateTime]
                                    ,cbr.[WithwardId]
                                    ,cbr.[RelationNo]
                                    ,cbr.[Remark]";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (criteria.RecordType != 0)
            {
                sbSqlWhere.AppendFormat("AND cbr.[RecordType]={0}", criteria.RecordType);
            }
            if (!string.IsNullOrWhiteSpace(criteria.RelationNo))
            {
                sbSqlWhere.AppendFormat("AND cbr.[RelationNo]='{0}'", criteria.RelationNo);
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeStart))
            {
                sbSqlWhere.AppendFormat("AND CONVERT(CHAR(10),cbr.OperateTime,120)>=CONVERT(CHAR(10),'{0}',120)", criteria.OperateTimeStart.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeEnd))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),cbr.OperateTime,120)<=CONVERT(CHAR(10),'{0}',120)", criteria.OperateTimeEnd.Trim());
            }
            if (criteria.ClienterId>0)
            {
                sbSqlWhere.AppendFormat(" AND ClienterId={0}", criteria.ClienterId);
            }
            string tableList = @" [ClienterBalanceRecord] cbr WITH(NOLOCK)";
            string orderByColumn = " cbr.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
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
       cwf.Amount,
       cwf.WithdrawTime WithdrawDateStart
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
        /// <param name="criteria"></param>
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
        /// 骑士 余额可提现和插入骑士余额流水     
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterRecharge(ClienterOptionLog model)
        {
            string sql = string.Format(@" 
update b
set    b.AccountBalance=ISNULL(b.AccountBalance, 0)+@Amount,
       b.AllowWithdrawPrice=ISNULL(b.AllowWithdrawPrice,0)+@Amount
OUTPUT
  Inserted.Id,
  @Amount,
  @Status,
  Inserted.AccountBalance,
  @RecordType,
  @Operator,
  getdate(),
  '',
  @Remark
INTO ClienterBalanceRecord
  ( [ClienterId]
   ,[Amount]
   ,[Status]
   ,[Balance]
   ,[RecordType]
   ,[Operator]
   ,[OperateTime]
   ,[WithwardId]
   ,[Remark])
from clienter b WITH ( ROWLOCK )
where b.Id=@ClienterId;");
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Amount", model.RechargeAmount);
            parm.AddWithValue("@Status", ClienterBalanceRecordStatus.Success);
            parm.AddWithValue("@RecordType", ClienterBalanceRecordRecordType.BalanceAdjustment);
            parm.AddWithValue("@Operator", model.OptName);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@ClienterId", model.ClienterId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
//         /// <summary>
//        /// 根据申请单Id获取商家金融账号信息
//        /// danny-20150716
//        /// </summary>
//        /// <param name="withwardId">提款单Id</param>
//        /// <returns></returns>
//        public ClienterFinanceAccountModel GetClienterFinanceAccount(string withwardId)
//        {
//            string sql = @"  
//SELECT cfa.[Id]
//      ,cfa.[ClienterId]
//      ,cfa.[TrueName]
//      ,cfa.[AccountNo]
//      ,cfa.[IsEnable]
//      ,cfa.[AccountType]
//      ,cfa.[BelongType]
//      ,cfa.[OpenBank]
//      ,cfa.[OpenSubBank]
//      ,cfa.[CreateBy]
//      ,cfa.[CreateTime]
//      ,cfa.[UpdateBy]
//      ,cfa.[UpdateTime]
//      ,cfa.[IDCard]
//      ,cfa.[OpenProvince]
//      ,cfa.[OpenCity]
//      ,cfa.[YeepayKey]
//      ,cfa.[YeepayStatus]
//      ,cwf.IDCard CliIDCard
//      ,cwf.Amount
//      ,cwf.HandChargeThreshold
//      ,cwf.HandCharge
//      ,cwf.HandChargeOutlay
//      ,cwf.WithdrawTime
//      ,c.PhoneNo
//  FROM [ClienterFinanceAccount] cfa with(nolock)
//  join ClienterWithdrawForm cwf with(nolock) on cwf.ClienterId=cfa.ClienterId and cwf.Id=@withwardId
//  join clienter c with(nolock) on c.Id=cfa.ClienterId ";
//            IDbParameters parm = DbHelper.CreateDbParameters();
//            parm.AddWithValue("@withwardId", withwardId);
//            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
//            if (dt == null || dt.Rows.Count <= 0)
//            {
//                return null;
//            }
//            return MapRows<ClienterFinanceAccountModel>(dt)[0];
//        }
        /// <summary>
        /// 根据提现申请单Id获取商家金融账号信息
        /// danny-20150716
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        public ClienterFinanceAccountModel GetClienterFinanceAccount(string withwardId)
        {
            string sql = @"  
SELECT cwf.[ClienterId]
      ,cwf.[TrueName]
      ,cwf.[AccountNo]
      ,cwf.[AccountType]
      ,cwf.[BelongType]
      ,cwf.[OpenBank]
      ,cwf.[OpenSubBank]
      ,cwf.[IDCard]
      ,cwf.[OpenProvince]
      ,cwf.[OpenCity]
	  ,ypu.Ledgerno YeepayKey
	  ,cfa.YeepayStatus
      ,cwf.Amount
      ,cwf.HandChargeThreshold
      ,cwf.HandCharge
      ,cwf.HandChargeOutlay
      ,cwf.WithdrawTime
      ,cwf.PhoneNo
	  ,ISNULL(ypu.BalanceRecord,0) BalanceRecord
	  ,ISNULL(ypu.YeeBalance,0) YeeBalance
      ,cfa.Id
  FROM ClienterWithdrawForm cwf with(nolock)
  JOIN dbo.ClienterFinanceAccount cfa WITH(NOLOCK) ON cfa.ClienterId=cwf.ClienterId AND cwf.Id=@withwardId
  LEFT JOIN ( SELECT tblypu.UserId,tblypu.Ledgerno,tblypu.BankName,tblypu.BankAccountNumber,tblypu.BalanceRecord,tblypu.YeeBalance
			  FROM(
			      SELECT UserId,BankName,BankAccountNumber,MAX(Addtime) Addtime
			      FROM YeePayUser(NOLOCK) 
			      GROUP BY UserId,BankName,BankAccountNumber) tbl
			  JOIN YeePayUser tblypu (NOLOCK) 
                ON  tblypu.Addtime=tbl.Addtime 
                AND tblypu.UserId = tbl.UserId 
                AND tblypu.BankName=tbl.BankName 
                AND tblypu.BankAccountNumber=tbl.BankAccountNumber) ypu  
	   ON  ypu.UserId=cwf.ClienterId  
       AND ypu.BankAccountNumber=cwf.AccountNo 
       AND ypu.BankName=cwf.OpenBank ;
";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@withwardId", withwardId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<ClienterFinanceAccountModel>(dt)[0];
        }
        /// <summary>
        /// 添加易宝用户账户流水记录
        /// danny-20150728
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddYeePayUserBalanceRecord(YeePayUserBalanceRecord model)
        {
            string sql = string.Format(@" 
INSERT INTO [YeePayUserBalanceRecord]
           ([LedgerNo]
           ,[WithwardId]
           ,[Amount]
           ,[Balance]
           ,[RecordType]
           ,[Operator]
           ,[Remark])
     VALUES
           (@LedgerNo  
           ,@WithwardId
           ,@Amount
           ,@Balance
           ,@RecordType
           ,@Operator
           ,@Remark);");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@LedgerNo", model.LedgerNo);
            parm.AddWithValue("@WithwardId", model.WithwardId);
            parm.AddWithValue("@Amount", model.Amount);
            parm.AddWithValue("@Balance", model.Balance);
            parm.AddWithValue("@RecordType", model.RecordType);
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 修改本系统易宝余额
        /// danny-20150729
        /// </summary>
        /// <param name="yeepayKey">易宝账号</param>
        /// <param name="amount">交易金额</param>
        /// <returns></returns>
        public int ModifyYeeBalanceRecord(string yeepayKey,decimal amount)
        {
            string sql = string.Format(@" 
update YeePayUser
set    BalanceRecord=BalanceRecord+@Amount,
       UpdateTime=getdate()
output Inserted.BalanceRecord
where Ledgerno=@YeepayKey;");
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@YeepayKey", yeepayKey);
            parm.AddWithValue("@Amount", amount);
            return  ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm));
        }
    }
}
