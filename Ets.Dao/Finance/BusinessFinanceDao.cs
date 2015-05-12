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
    public class BusinessFinanceDao : DaoBase
    {
        /// <summary>
        /// 根据参数获取商家提现申请单列表
        /// danny-20150509
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetBusinessWithdrawList<T>(BusinessWithdrawSearchCriteria criteria)
        {
            string columnList = @"  bwf.Id,
                                    bwf.WithwardNo,
                                    b.[Name] BusinessName,
                                    b.PhoneNo BusinessPhoneNo,
                                    bwf.BalancePrice,
                                    bwf.AllowWithdrawPrice,
                                    bwf.Amount,
                                    bwf.Balance,
                                    bwf.Status,
                                    bwf.WithdrawTime,
                                    bwf.Auditor,
                                    bwf.AuditTime,
                                    bwf.Payer,
                                    bwf.PayTime,
                                    bwf.AuditFailedReason,
                                    bwf.PayFailedReason ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(criteria.BusinessName))
            {
                sbSqlWhere.AppendFormat(" AND b.[Name]='{0}' ", criteria.BusinessName.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.BusinessPhoneNo))
            {
                sbSqlWhere.AppendFormat(" AND b.PhoneNo='{0}' ", criteria.BusinessPhoneNo.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.BusinessCity))
            {
                sbSqlWhere.AppendFormat(" AND b.City='{0}' ", criteria.BusinessCity.Trim());
            }
            if (criteria.WithdrawStatus != 0)
            {
                sbSqlWhere.AppendFormat(" AND bwf.Status={0} ", criteria.WithdrawStatus);
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithwardNo))
            {
                sbSqlWhere.AppendFormat(" AND bwf.WithwardNo='{0}' ", criteria.WithwardNo.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateStart))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),bwf.WithdrawTime,120)>=CONVERT(CHAR(10),'{0}',120) ", criteria.WithdrawDateStart.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateEnd))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),bwf.WithdrawTime,120)<=CONVERT(CHAR(10),'{0}',120) ", criteria.WithdrawDateEnd.Trim());
            }
            string tableList = @" BusinessWithdrawForm bwf with(nolock)
                                  join business b with(nolock) on bwf.BusinessId=b.Id";
            string orderByColumn = " bwf.Id DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }

        /// <summary>
        /// 根据申请单Id获取商家提现申请单
        /// danny-20150511
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public BusinessWithdrawFormModel GetBusinessWithdrawListById(string withwardId)
        {
            string sql = @"  
select bwf.Id,
       bwf.WithwardNo,
       bwf.BusinessId,
       b.[Name] BusinessName,
       b.PhoneNo BusinessPhoneNo,
       b.HasWithdrawPrice,
       bwf.BalancePrice,
       bwf.AllowWithdrawPrice,
       bwf.Amount,
       bwf.Balance,
       bwf.Status,
       bwf.WithdrawTime,
       bwf.Auditor,
       bwf.AuditTime,
       bwf.Payer,
       bwf.PayTime,
       bwf.AuditFailedReason,
       bwf.PayFailedReason,
       bwf.OpenBank,
       bwf.OpenSubBank,
       bwf.TrueName,
       bwf.AccountNo
from BusinessWithdrawForm bwf with(nolock)
  join business b with(nolock) on bwf.BusinessId=b.Id and bwf.Id=@Id  ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Id", withwardId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<BusinessWithdrawFormModel>(dt)[0];
        }
        /// <summary>
        /// 获取商户提款单操作日志
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        public IList<BusinessWithdrawLog> GetBusinessWithdrawOptionLog(string withwardId)
        {
            string sql = @"  
SELECT [Id]
      ,[WithwardId]
      ,[Status]
      ,[Remark]
      ,[Operator]
      ,[OperatTime]
FROM BusinessWithdrawLog bwl with(nolock)
WHERE WithwardId=@Id
ORDER BY Id;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Id", withwardId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<BusinessWithdrawLog>(dt);
        }
        /// <summary>
        /// 审核商户提现申请单
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessWithdrawAudit(BusinessWithdrawLog model)
        {
            string sql = string.Format(@" 
UPDATE BusinessWithdrawForm
 SET    [Status] = @Status,
		Auditor=@Operator,
		AuditTime=getdate()
OUTPUT
  Inserted.Id,
  Inserted.[Status],
  @Remark,
  @Operator,
  getdate()
INTO BusinessWithdrawLog
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
        /// 商户提现申请单确认打款
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessWithdrawPayOk(BusinessWithdrawLog model)
        {
            string sql = string.Format(@" 
UPDATE BusinessWithdrawForm
 SET    [Status] = @Status,
		Payer=@Operator,
		PayTime=getdate()
OUTPUT
  Inserted.Id,
  Inserted.[Status],
  @Remark,
  @Operator,
  getdate()
INTO BusinessWithdrawLog
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
        /// 商户提现申请单审核拒绝
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessWithdrawAuditRefuse(BusinessWithdrawLogModel model)
        {
            string sql = string.Format(@" 
UPDATE BusinessWithdrawForm
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
INTO BusinessWithdrawLog
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
        /// 商户提现申请单打款失败
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessWithdrawPayFailed(BusinessWithdrawLogModel model)
        {
            string sql = string.Format(@" 
UPDATE BusinessWithdrawForm
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
INTO BusinessWithdrawLog
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
        /// 修改商家提款流水状态
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyBusinessBalanceRecordStatus(string withwardId)
        {
            string sql = string.Format(@" 
UPDATE BusinessBalanceRecord
 SET    [Status] = 1
 WHERE  WithwardId = @WithwardId AND [Status]=2;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@WithwardId", withwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        /// <summary>
        /// 商户提现失败后返现
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessWithdrawReturn(BusinessWithdrawLogModel model)
        {
            string sql = string.Format(@" 
insert into BusinessBalanceRecord
            ([BusinessId]
           ,[Amount]
           ,[Status]
           ,[Balance]
           ,[RecordType]
           ,[Operator]
           ,[OperateTime]
           ,[WithwardId]
           ,[RelationNo]
           ,[Remark])
select      [BusinessId]
           ,-ISNULL([Amount],0) Amount
           ,1 [Status]
           ,-ISNULL([Amount],0)+ISNULL([Balance],0) Balance
           ,5 [RecordType]
           ,@Operator
           ,getdate() OperateTime
           ,[WithwardId]
           ,[RelationNo]
           ,@Remark
 from BusinessBalanceRecord (nolock)
 where WithwardId=@WithwardId and Status=2 and RecordType=3;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@WithwardId", model.WithwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        
        /// <summary>
        /// 商户提现失败后修改商户表金额
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyBusinessAmountInfo(string withwardId)
        {
            string sql = string.Format(@" 
update b
set    b.BalancePrice=ISNULL(b.BalancePrice, 0)+ISNULL(bwf.Amount,0),
       b.AllowWithdrawPrice=ISNULL(b.AllowWithdrawPrice,0)+ISNULL(bwf.Amount,0)
from business b
join [BusinessWithdrawForm] bwf on b.Id=bwf.BusinessId
where bwf.Id=@WithwardId;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@WithwardId", withwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 商户提现打款确认后修改商户表累计提款金额
        /// danny-20150511
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyBusinessTotalAmount(string withwardId)
        {
            string sql = string.Format(@" 
update b
set    b.HasWithdrawPrice=ISNULL(b.HasWithdrawPrice, 0)+ISNULL(bwf.Amount,0)
from business b
join [BusinessWithdrawForm] bwf on b.Id=bwf.BusinessId
where bwf.Id=@WithwardId;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@WithwardId", withwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 获取商户提款收支记录列表
        /// danny-20150512
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        public IList<BusinessBalanceRecord> GetBusinessBalanceRecordList(BusinessBalanceRecordSerchCriteria criteria)
        {
            string sql = @"  
SELECT bbr.[Id]
      ,bbr.[BusinessId]
      ,bbr.[Amount]
      ,bbr.[Status]
      ,bbr.[Balance]
      ,bbr.[RecordType]
      ,bbr.[Operator]
      ,bbr.[OperateTime]
      ,bbr.[WithwardId]
      ,bbr.[RelationNo]
      ,bbr.[Remark]
  FROM [BusinessBalanceRecord] bbr WITH(NOLOCK)
WHERE BusinessId=@BusinessId ";
            if (criteria.RecordType!=0)
            {
                sql+=@" AND bbr.[RecordType]=@RecordType";
            }
            if (!string.IsNullOrWhiteSpace(criteria.RelationNo))
            {
                sql+=@" AND bbr.[RelationNo]=@RelationNo";
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeStart))
            {
                sql+=@" AND CONVERT(CHAR(10),bbr.OperateTime,120)>=CONVERT(CHAR(10),@OperateTimeStart,120)";
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeEnd))
            {
                sql+=@" AND CONVERT(CHAR(10),bbr.OperateTime,120)>=CONVERT(CHAR(10),@OperateTimeEnd,120)";
            }
            sql+=" ORDER BY bbr.Id DESC";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", criteria.BusinessId);
            parm.AddWithValue("@RecordType", criteria.RecordType);
            parm.AddWithValue("@RelationNo", criteria.RelationNo);
            parm.AddWithValue("@OperateTimeStart", criteria.OperateTimeStart);
            parm.AddWithValue("@OperateTimeEnd", criteria.OperateTimeEnd);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<BusinessBalanceRecord>(dt);
        }
    }
       
}
