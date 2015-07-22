using ETS.Enums;
using Ets.Model.DataModel.Business;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ets.Dao.Order;
using Ets.Model.DataModel.Order;
using ETS.Util;

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
            if (criteria.AuthorityCityNameListStr != null && !string.IsNullOrEmpty(criteria.AuthorityCityNameListStr.Trim()) && criteria.UserType != 0)
            {
                sbSqlWhere.AppendFormat("  AND B.City IN({0}) ", criteria.AuthorityCityNameListStr);
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
        /// <param name="withwardId">提款单Id</param>
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
       bwf.AccountNo,
       bwf.OpenProvince,
       bwf.OpenCity,
       bwf.BelongType,
       bwf.IDCard
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
        /// 查询该商户有无审核通过的体提现申请单
        /// wc-20150717
        /// </summary>
        /// <param name="businessId">提款单Id</param>
        /// <returns></returns>
        public int GetBusinessWithdrawByBusinessId(int businessId)
        {
            string sql = @"  
 select count(1)  FROM  dbo.BusinessWithdrawForm a (nolock)
        where a.[Status] in(1,2) and a.BusinessId = @businessId ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@businessId", DbType.Int32).Value = businessId;
            var count = DbHelper.ExecuteScalar(SuperMan_Read, sql, parm); 
            return ParseHelper.ToInt(count, 0);
        }


        /// <summary>
        /// 获取商户提款单操作日志
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
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
            //只更新待审状态的数据
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
 WHERE  Id = @Id and [Status]=1");

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@Id", model.WithwardId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 修改商家提款流水状态
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        public bool ModifyBusinessBalanceRecordStatus(string withwardId)
        {
            string sql = string.Format(@" 
UPDATE BusinessBalanceRecord
 SET    [Status] = @Status
 WHERE  WithwardId = @WithwardId AND [Status]=2;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@WithwardId", withwardId);
            parm.AddWithValue("@Status", BusinessBalanceRecordStatus.Success.GetHashCode());
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
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
select      bbr.[BusinessId]
           ,-ISNULL(bbr.[Amount],0) Amount
           ,@NewStatus [Status]
           ,-ISNULL(bbr.[Amount],0)+ISNULL(b.BalancePrice,0) Balance
           ,@NewRecordType [RecordType]
           ,@Operator
           ,getdate() OperateTime
           ,bbr.[WithwardId]
           ,bbr.[RelationNo]
           ,@Remark
 from BusinessBalanceRecord bbr (nolock)
    join business b (nolock) on b.Id=bbr.BusinessId
 where bbr.WithwardId=@WithwardId and bbr.Status=@Status and bbr.RecordType=@RecordType;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Operator", model.Operator);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@WithwardId", model.WithwardId);
            parm.AddWithValue("@Status",BusinessBalanceRecordStatus.Tradeing);
            parm.AddWithValue("@RecordType",BusinessBalanceRecordRecordType.WithdrawApply);
            parm.AddWithValue("@NewStatus",BusinessBalanceRecordStatus.Success);
            parm.AddWithValue("@NewRecordType", model.Status == BusinessWithdrawFormStatus.TurnDown.GetHashCode() ?BusinessBalanceRecordRecordType.WithdrawRefuse :BusinessBalanceRecordRecordType.PayFailure);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        
        /// <summary>
        /// 商户提现失败后修改商户表金额
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 商户提现打款确认后修改商户表累计提款金额
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId"></param>
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 获取商户提款收支记录列表
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
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
                sql+=@" AND CONVERT(CHAR(10),bbr.OperateTime,120)<=CONVERT(CHAR(10),@OperateTimeEnd,120)";
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

        /// <summary>
        /// 获取要导出的商户提现申请单
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<BusinessWithdrawFormModel> GetBusinessWithdrawForExport(BusinessWithdrawSearchCriteria criteria)
        {
            string sql = @"  
select b.[Name] BusinessName,
       b.PhoneNo BusinessPhoneNo,
       bwf.OpenBank ,
       bwf.TrueName ,
       bwf.AccountNo ,
       bwf.Amount 
from BusinessWithdrawForm bwf with(nolock)
  join business b with(nolock) on bwf.BusinessId=b.Id 
where 1=1";
            if (!string.IsNullOrWhiteSpace(criteria.BusinessName))
            {
                sql+=" AND b.[Name]=@BusinessName";
            }
            if (!string.IsNullOrWhiteSpace(criteria.BusinessPhoneNo))
            {
                sql+=" AND b.PhoneNo=@BusinessPhoneNo";
            }
            if (!string.IsNullOrWhiteSpace(criteria.BusinessCity))
            {
                sql+=" AND b.City=@BusinessCity";
            }
            if (criteria.WithdrawStatus != 0)
            {
                sql+=" AND bwf.Status=@WithdrawStatus";
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithwardNo))
            {
                sql+=" AND bwf.WithwardNo=@WithwardNo";
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateStart))
            {
                sql+=" AND CONVERT(CHAR(10),bwf.WithdrawTime,120)>=CONVERT(CHAR(10),@WithdrawDateStart,120)";
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateEnd))
            {
                sql+=" AND CONVERT(CHAR(10),bwf.WithdrawTime,120)<=CONVERT(CHAR(10),@WithdrawDateEnd,120)";
            }
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessName", criteria.BusinessName);
            parm.AddWithValue("@BusinessPhoneNo", criteria.BusinessPhoneNo);
            parm.AddWithValue("@WithwardNo", criteria.WithwardNo);
            parm.AddWithValue("@WithdrawDateStart", criteria.WithdrawDateStart);
            parm.AddWithValue("@WithdrawDateEnd", criteria.WithdrawDateEnd);
            parm.AddWithValue("@WithdrawStatus", criteria.WithdrawStatus);
            parm.AddWithValue("@BusinessCity", criteria.BusinessCity);
            DataTable dt =DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<BusinessWithdrawFormModel>(dt);
        }
        /// <summary>
        /// 获取要导出的商户提款收支记录列表
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<BusinessBalanceRecordModel> GetBusinessBalanceRecordListForExport(BusinessBalanceRecordSerchCriteria criteria)
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
      ,bfa.AccountNo
      ,bfa.OpenBank
FROM [BusinessBalanceRecord] bbr WITH(NOLOCK)
LEFT JOIN BusinessFinanceAccount bfa WITH(NOLOCK) ON bbr.BusinessId=bfa.BusinessId
WHERE bbr.BusinessId=@BusinessId ";
            if (criteria.RecordType != 0)
            {
                sql += @" AND bbr.[RecordType]=@RecordType";
            }
            if (!string.IsNullOrWhiteSpace(criteria.RelationNo))
            {
                sql += @" AND bbr.[RelationNo]=@RelationNo";
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeStart))
            {
                sql += @" AND CONVERT(CHAR(10),bbr.OperateTime,120)>=CONVERT(CHAR(10),@OperateTimeStart,120)";
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeEnd))
            {
                sql += @" AND CONVERT(CHAR(10),bbr.OperateTime,120)<=CONVERT(CHAR(10),@OperateTimeEnd,120)";
            }
            sql += " ORDER BY bbr.Id DESC";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", criteria.BusinessId);
            parm.AddWithValue("@RecordType", criteria.RecordType);
            parm.AddWithValue("@RelationNo", criteria.RelationNo);
            parm.AddWithValue("@OperateTimeStart", criteria.OperateTimeStart);
            parm.AddWithValue("@OperateTimeEnd", criteria.OperateTimeEnd);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<BusinessBalanceRecordModel>(dt);
        }


        public decimal GetBusiBalance(string orderNo)
        {
            OrderDao orderDao = new OrderDao();
             
            return 0m;
        }
        /// <summary>
        /// 商户充值增加商家余额可提现和插入商家余额流水
        /// danny-20150526
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BusinessRecharge(BusinessOptionLog model)
        {
            string sql = string.Format(@" 
update b
set    b.BalancePrice=ISNULL(b.BalancePrice, 0)+@Amount,
       b.AllowWithdrawPrice=ISNULL(b.AllowWithdrawPrice,0)+@Amount
OUTPUT
  Inserted.Id,
  @Amount,
  @Status,
  Inserted.BalancePrice,
  @RecordType,
  @Operator,
  getdate(),
  @Remark
INTO BusinessBalanceRecord
  ( [BusinessId]
   ,[Amount]
   ,[Status]
   ,[Balance]
   ,[RecordType]
   ,[Operator]
   ,[OperateTime]
   ,[Remark])
from business b WITH ( ROWLOCK )
where b.Id=@BusinessId;");
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Amount", model.RechargeAmount);
            parm.AddWithValue("@Status", BusinessBalanceRecordStatus.Success);
            parm.AddWithValue("@RecordType", BusinessBalanceRecordRecordType.Recharge);
            parm.AddWithValue("@Operator", model.OptName);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@BusinessId", model.BusinessId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 获取商户提款收支记录列表分页版
        /// danny-20150604
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetBusinessBalanceRecordListOfPaging<T>(BusinessBalanceRecordSerchCriteria criteria)
        {
            string columnList = @"   bbr.[Id]
                                    ,bbr.[BusinessId]
                                    ,bbr.[Amount]
                                    ,bbr.[Status]
                                    ,bbr.[Balance]
                                    ,bbr.[RecordType]
                                    ,bbr.[Operator]
                                    ,bbr.[OperateTime]
                                    ,bbr.[WithwardId]
                                    ,bbr.[RelationNo]
                                    ,bbr.[Remark]";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (criteria.BusinessId != 0)
            {
                sbSqlWhere.AppendFormat("AND bbr.[BusinessId]={0}", criteria.BusinessId);
            }
            if (criteria.RecordType != 0)
            {
                sbSqlWhere.AppendFormat("AND bbr.[RecordType]={0}", criteria.RecordType);
            }
            if (!string.IsNullOrWhiteSpace(criteria.RelationNo))
            {
                sbSqlWhere.AppendFormat("AND bbr.[RelationNo]='{0}'", criteria.RelationNo);
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeStart))
            {
                sbSqlWhere.AppendFormat("AND CONVERT(CHAR(10),bbr.OperateTime,120)>=CONVERT(CHAR(10),'{0}',120)", criteria.OperateTimeStart.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.OperateTimeEnd))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),bbr.OperateTime,120)<=CONVERT(CHAR(10),'{0}',120)", criteria.OperateTimeEnd.Trim());
            }
            string tableList = @" [BusinessBalanceRecord] bbr WITH(NOLOCK)";
            string orderByColumn = " bbr.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }
        /// <summary>
        /// 根据单号查询充值详情
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150624</UpdateTime>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public BusinessRechargeDetail GetBusinessRechargeDetailByNo(string orderNo)
        {
            string sql = @"
                        SELECT  b.BusinessId ,
                                a.Name ,
                                c.PayTime ,
                                b.Amount ,
                                b.Balance ,
                                c.OrderNo ,
                                c.PayType ,
                                c.PayStatus
                        FROM    dbo.business a ( NOLOCK )
                                JOIN BusinessBalanceRecord b ( NOLOCK ) ON a.id = b.BusinessId
                                JOIN BusinessRecharge c ( NOLOCK ) ON a.id = c.BusinessId AND b.RelationNo=c.OrderNo 
                        WHERE   c.OrderNo=@OrderNo
                ";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OrderNo", orderNo);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<BusinessRechargeDetail>(dt)[0];
        }
        /// <summary>
        /// 根据申请单Id获取商家金融账号信息
        /// danny-20150716
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        public BusinessFinanceAccountModel GetBusinessFinanceAccount(string withwardId)
        {
            string sql = @"  
SELECT bfa.[Id]
      ,bfa.[BusinessId]
      ,bfa.[TrueName]
      ,bfa.[AccountNo]
      ,bfa.[IsEnable]
      ,bfa.[AccountType]
      ,bfa.[BelongType]
      ,bfa.[OpenBank]
      ,bfa.[OpenSubBank]
      ,bfa.[CreateBy]
      ,bfa.[CreateTime]
      ,bfa.[UpdateBy]
      ,bfa.[UpdateTime]
      ,bfa.[IDCard]
      ,bfa.[OpenProvince]
      ,bfa.[OpenCity]
      ,bfa.[YeepayKey]
      ,bfa.[YeepayStatus]
      ,b.IDCard BusiIDCard
      ,bwf.Amount
      ,bwf.HandChargeThreshold
      ,bwf.HandCharge
      ,bwf.HandChargeOutlay
      ,WithdrawTime
  FROM [BusinessFinanceAccount] bfa with(nolock)
  join BusinessWithdrawForm bwf with(nolock) on bwf.BusinessId=bfa.BusinessId and bwf.Id=@withwardId 
  join business b with(nolock) on b.id=bfa.BusinessId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@withwardId", withwardId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<BusinessFinanceAccountModel>(dt)[0];
        }
    }
       
}
