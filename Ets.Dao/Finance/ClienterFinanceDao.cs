﻿using System;
using ETS.Enums;
using Ets.Model.Common.YeePay;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using System.Collections.Generic;
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
                                    cwf.HandChargeOutlay,
                                    cwf.AccountType   ";
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
            if (!string.IsNullOrEmpty(criteria.businessCity))
            {
                sbSqlWhere.AppendFormat(" AND C.City='{0}' ", criteria.businessCity.Trim());
            }
            if (criteria.AccountType!=0)
            {
                sbSqlWhere.AppendFormat(" AND cwf.AccountType={0} ", criteria.AccountType);
            }
            if (criteria.ClientWithdrawDate > 0)
            {
                if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateStart))
                {
                    sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),cwf.{0},120)>=CONVERT(CHAR(10),'{1}',120) ", (((ClientWithdrawType)criteria.ClientWithdrawDate).ToString()), criteria.WithdrawDateStart.Trim());
                }
                if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateEnd))
                {
                    sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),cwf.{0},120)<=CONVERT(CHAR(10),'{1}',120) ", (((ClientWithdrawType)criteria.ClientWithdrawDate).ToString()), criteria.WithdrawDateEnd.Trim());
                }
            }

            if (!string.IsNullOrEmpty(criteria.AuthorityCityNameListStr) && criteria.UserType != 0)
            {
                sbSqlWhere.AppendFormat(" AND C.City IN ({0}) ", criteria.AuthorityCityNameListStr.Trim());
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
       cwf.AccountNo,
       cwf.AccountType
from ClienterWithdrawForm cwf with(nolock)
  join clienter c with(nolock) on cwf.ClienterId=c.Id and cwf.Id=@Id  ";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Id", withwardId);
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
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
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Id", withwardId);
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
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
            string sql = @" 
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
 WHERE  Id = @Id and [Status]=1";
            var parm = DbHelper.CreateDbParameters();
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
            #region 添加易宝回调处理
            var sqlAppend = "";
            if (model.IsCallBack > 0)
            {
                sqlAppend += ",CallBackTime=getdate(),CallBackRequestId=@CallBackRequestId ";
            }
            #endregion
            string sql = string.Format(@" 
UPDATE ClienterWithdrawForm
 SET    [Status] = @Status,
		Payer=@Operator,
		PayTime=getdate()
        {0}
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
 WHERE  Id = @Id AND [Status]=@OldStatus", sqlAppend);
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@OldStatus", model.OldStatus);
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@Id", model.WithwardId);
            parm.AddWithValue("@CallBackRequestId", model.CallBackRequestId);
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
            string sql = @" 
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
 WHERE  Id = @Id and [Status]=1";
            var parm = DbHelper.CreateDbParameters();
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
            #region 添加易宝回调处理
            var sqlAppend = "";
            if (model.IsCallBack > 0)
            {
                sqlAppend += ",CallBackTime=getdate(),CallBackRequestId=@CallBackRequestId ";
            }
            #endregion
            string sql = string.Format(@" 
UPDATE ClienterWithdrawForm
 SET    [Status] = @Status,
        PayFailedReason=ISNULL(PayFailedReason,'')+@PayFailedReason+' '
        {0}
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
 WHERE  Id = @Id AND [Status] IN(2,20,4)", sqlAppend);
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@PayFailedReason", model.PayFailedReason);
            parm.AddWithValue("@Id", model.WithwardId);
            parm.AddWithValue("@CallBackRequestId", model.CallBackRequestId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

        /// <summary>
        /// 修改骑士提现申请单打款失败原因
        /// danny-20150804
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyClienterWithdrawPayFailedReason(ClienterWithdrawLogModel model)
        {
            string sql = @" 
UPDATE ClienterWithdrawForm
 SET    PayFailedReason=ISNULL(PayFailedReason,'')+@PayFailedReason+' '
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
 WHERE  Id = @Id ";
            var parm = DbHelper.CreateDbParameters();
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
            string sql = @" 
UPDATE ClienterBalanceRecord
 SET    [Status] = @Status
 WHERE  WithwardId = @WithwardId AND [Status]=2;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@WithwardId", withwardId);
            parm.AddWithValue("@Status", ClienterBalanceRecordStatus.Success.GetHashCode());
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

        /// <summary>
        /// 骑士提现申请单确认打款处理成功
        /// danny-20150805
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawPayDealOk(ClienterWithdrawLog model)
        {
            string sql = @" 
UPDATE ClienterWithdrawForm
 SET    [DealStatus] = @DealStatus,
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
 WHERE  Id = @Id ";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@DealStatus", model.DealStatus);
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@Id", model.WithwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

        /// <summary>
        /// 骑士提现失败后返现==增加骑士余额流水记录
        /// danny-20150513
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterWithdrawReturn(ClienterWithdrawLogModel model)
        {
            #region 临时注释
            //            string sql = @" 
            //insert into ClienterBalanceRecord
            //            ([ClienterId]
            //           ,[Amount]
            //           ,[Status]
            //           ,[Balance]
            //           ,[RecordType]
            //           ,[Operator]
            //           ,[OperateTime]
            //           ,[WithwardId]
            //           ,[RelationNo]
            //           ,[Remark])
            //select      cbr.[ClienterId]
            //           ,-ISNULL(cbr.[Amount],0) Amount
            //           ,@NewStatus [Status]
            //           ,-ISNULL(cbr.[Amount],0)+ISNULL(c.AccountBalance,0) Balance
            //           ,@NewRecordType [RecordType]
            //           ,@Operator
            //           ,getdate() OperateTime
            //           ,cbr.[WithwardId]
            //           ,cbr.[RelationNo]
            //           ,@Remark
            // from ClienterBalanceRecord cbr (nolock)
            //    join clienter c (nolock) on c.Id=cbr.ClienterId
                        // where cbr.WithwardId=@WithwardId and cbr.Status=@Status and cbr.RecordType=@RecordType;";
            #endregion
            string sql = @" 
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
select      cwf.[ClienterId]
           ,ISNULL(cwf.[Amount],0) Amount
           ,@NewStatus [Status]
           ,ISNULL(cwf.[Amount],0)+ISNULL(c.AccountBalance,0) Balance
           ,@NewRecordType [RecordType]
           ,@Operator
           ,getdate() OperateTime
           ,cwf.Id WithwardId
           ,cwf.WithwardNo RelationNo
           ,@Remark
 from ClienterWithdrawForm cwf(nolock)
    join clienter c (nolock) on c.Id=cwf.ClienterId
 where cwf.Id=@WithwardId ";
            
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@WithwardId", model.WithwardId);
            //parm.AddWithValue("@Status", ClienterBalanceRecordStatus.Tradeing.GetHashCode());
            //parm.AddWithValue("@RecordType", ClienterBalanceRecordRecordType.WithdrawApply.GetHashCode());
            parm.AddWithValue("@NewStatus", ClienterBalanceRecordStatus.Success.GetHashCode());
            parm.AddWithValue("@NewRecordType", model.Status == ClienterWithdrawFormStatus.TurnDown.GetHashCode() ? ClienterBalanceRecordRecordType.WithdrawRefuse : ClienterBalanceRecordRecordType.PayFailure);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 骑士提现失败 
        /// 可提现余额流水
        /// danny-20150820
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterClienterAllowWithdrawRecordReturn(ClienterWithdrawLogModel model)
        {
            #region 临时注释
//            string sql = string.Format(@" 
//            insert into ClienterAllowWithdrawRecord
//                        ([ClienterId]
//                       ,[Amount]
//                       ,[Status]
//                       ,[Balance]
//                       ,[RecordType]
//                       ,[Operator]
//                       ,[OperateTime]
//                       ,[WithwardId]
//                       ,[RelationNo]
//                       ,[Remark])
//            select      cbr.[ClienterId]
//                       ,-ISNULL(cbr.[Amount],0) Amount
//                       ,@NewStatus [Status]
//                       ,-ISNULL(cbr.[Amount],0)+ISNULL(c.AllowWithdrawPrice,0) Balance
//                       ,@NewRecordType [RecordType]
//                       ,@Operator
//                       ,getdate() OperateTime
//                       ,cbr.[WithwardId]
//                       ,cbr.[RelationNo]
//                       ,@Remark
//             from ClienterBalanceRecord cbr (nolock)
//                join clienter c (nolock) on c.Id=cbr.ClienterId
//             where cbr.WithwardId=@WithwardId and cbr.Status=@Status and cbr.RecordType=@RecordType;");
            #endregion

            string sql = string.Format(@" 
            insert into ClienterAllowWithdrawRecord
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
            SELECT 
                cwf.ClienterId,
                ISNULL(cwf.[Amount],0) Amount,
                @NewStatus [Status],
                ISNULL(cwf.[Amount],0)+ISNULL(c2.AllowWithdrawPrice,0) Balance,
                @NewRecordType [RecordType],
                @Operator Operator,
                getdate() OperateTime,
                cwf.Id WithwardId,
                cwf.WithwardNo RelationNo,
                @Remark Remark
            FROM dbo.ClienterWithdrawForm cwf(nolock)
                join dbo.clienter c2(nolock) on cwf.ClienterId=c2.Id
            where cwf.Id=@WithwardId");
            
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@WithwardId", model.WithwardId);
            //parm.AddWithValue("@Status", ClienterAllowWithdrawRecordStatus.Success.GetHashCode());
            //parm.AddWithValue("@RecordType", ClienterAllowWithdrawRecordType.WithdrawApply.GetHashCode());
            parm.AddWithValue("@NewStatus", ClienterAllowWithdrawRecordStatus.Success.GetHashCode());
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
            string sql = @" 
update c
set    c.AccountBalance=ISNULL(c.AccountBalance, 0)+ISNULL(cwf.Amount,0),
       c.AllowWithdrawPrice=ISNULL(c.AllowWithdrawPrice,0)+ISNULL(cwf.Amount,0)
from clienter c
join [ClienterWithdrawForm] cwf on c.Id=cwf.ClienterId
where cwf.Id=@WithwardId;";
            var parm = DbHelper.CreateDbParameters();
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
            string sql = @" 
update c
set    c.HasWithdrawPrice=ISNULL(c.HasWithdrawPrice, 0)+ISNULL(cwf.Amount,0)
from clienter c
join [ClienterWithdrawForm] cwf on c.Id=cwf.ClienterId
where cwf.Id=@WithwardId;";
            var parm = DbHelper.CreateDbParameters();
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
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ClienterId", criteria.ClienterId);
            parm.AddWithValue("@RecordType", criteria.RecordType);
            parm.AddWithValue("@RelationNo", criteria.RelationNo);
            parm.AddWithValue("@OperateTimeStart", criteria.OperateTimeStart);
            parm.AddWithValue("@OperateTimeEnd", criteria.OperateTimeEnd);
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
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
            if (criteria.ClienterId > 0)
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
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ClienterName", criteria.ClienterName);
            parm.AddWithValue("@ClienterPhoneNo", criteria.ClienterPhoneNo);
            parm.AddWithValue("@WithwardNo", criteria.WithwardNo);
            parm.AddWithValue("@WithdrawDateStart", criteria.WithdrawDateStart);
            parm.AddWithValue("@WithdrawDateEnd", criteria.WithdrawDateEnd);
            parm.AddWithValue("@WithdrawStatus", criteria.WithdrawStatus);
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
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
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ClienterId", criteria.ClienterId);
            parm.AddWithValue("@RecordType", criteria.RecordType);
            parm.AddWithValue("@RelationNo", criteria.RelationNo);
            parm.AddWithValue("@OperateTimeStart", criteria.OperateTimeStart);
            parm.AddWithValue("@OperateTimeEnd", criteria.OperateTimeEnd);
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<ClienterBalanceRecordModel>(dt);
        }

        /// <summary>
        /// 骑士 余额可提现和插入骑士余额流水     
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ClienterRecharge(ClienterOptionLog model)
        {
            string sql = @" 
update b
set    b.AccountBalance=ISNULL(b.AccountBalance, 0)+@Amount,
       b.AllowWithdrawPrice=ISNULL(b.AllowWithdrawPrice,0)+@Amount
from clienter b WITH ( ROWLOCK )
where b.Id=@ClienterId;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Amount", model.RechargeAmount);
            parm.AddWithValue("@ClienterId", model.ClienterId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

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
      ,cwf.Status WithdrawStatus
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
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@withwardId", withwardId);
            var dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, parm);
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
            string sql = @" 
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
           ,@Remark);";
            var parm = DbHelper.CreateDbParameters();
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
        public int ModifyYeeBalanceRecord(string yeepayKey, decimal amount)
        {
            string sql = @" 
update YeePayUser
set    BalanceRecord=BalanceRecord+@Amount,
       UpdateTime=getdate()
output Inserted.BalanceRecord
where Ledgerno=@YeepayKey;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@YeepayKey", yeepayKey);
            parm.AddWithValue("@Amount", amount);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm));
        }

        /// <summary>
        /// 修改本系统易宝余额
        /// danny-20150804
        /// </summary>
        /// <param name="yeepayKey">易宝账号</param>
        /// <param name="amount">交易金额</param>
        /// <returns></returns>
        public bool UpdateYeeBalanceRecord(string yeepayKey, decimal amount)
        {
            string sql = @" 
update YeePayUser
set    BalanceRecord=BalanceRecord+@Amount,
       UpdateTime=getdate()
where Ledgerno=@YeepayKey;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@YeepayKey", yeepayKey);
            parm.AddWithValue("@Amount", amount);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

        /// <summary>
        /// 获取状态为打款中的骑士提款申请单（待自动服务处理）
        /// danny-20150804
        /// 茹化肖修改只查账户类型为网银的 2015年10月21日11:24:25
        /// </summary>
        /// <returns></returns>
        public IList<ClienterFinanceAccountModel> GetClienterFinanceAccountList()
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
      ,cwf.DealStatus
      ,cwf.DealCount
      ,cwf.Id WithwardId
      ,cwf.WithwardNo WithwardNo
      ,cwf.PayFailedReason
      ,cwf.PaidAmount
  FROM ClienterWithdrawForm cwf with(nolock)
  JOIN dbo.ClienterFinanceAccount cfa WITH(NOLOCK) ON cfa.ClienterId=cwf.ClienterId and cwf.Status=@cwfStatus and cwf.DealStatus=@DealStatus and cwf.AccountType=cfa.AccountType AND cwf.AccountType=1
  LEFT JOIN ( SELECT tblypu.UserId,tblypu.Ledgerno,tblypu.BankName,tblypu.BankAccountNumber,tblypu.BalanceRecord,tblypu.YeeBalance
			  FROM(
			      SELECT UserId,BankName,BankAccountNumber,MAX(Addtime) Addtime
			      FROM YeePayUser(NOLOCK) 
                  WHERE UserType=0
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
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@cwfStatus", ClienterWithdrawFormStatus.Paying.GetHashCode());
            parm.AddWithValue("@DealStatus", ClienterWithdrawFormDealStatus.Dealing.GetHashCode());
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<ClienterFinanceAccountModel>(dt);
        }

        /// <summary>
        ///  修改骑士提现单处理次数
        /// danny-20150805
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        public int ModifyClienterWithdrawDealCount(long withwardId)
        {
            string sql = @" 
update ClienterWithdrawForm
set    DealCount=DealCount+1
output Inserted.DealCount
where Id=@WithwardId;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@WithwardId", withwardId);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm));
        }

        /// <summary>
        ///  获取骑士体现中未打款的金额
        /// 茹化肖
        /// 2015年8月12日17:13:21
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        public decimal GetClienterWithdrawingAmount(int cId)
        {
            string sql = @" 
SELECT ISNULL(SUM(Amount),0) AS Amount FROM ClienterWithdrawForm ( NOLOCK) WHERE ClienterId=@CID AND (Status=1 OR Status=2)";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@CID", cId);
            var obj = DbHelper.ExecuteScalar(SuperMan_Write, sql, parm);
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }

    }
}
