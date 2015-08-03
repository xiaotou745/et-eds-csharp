using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Enums;
using ETS.Extension;
using Ets.Model.Common.YeePay;
using Ets.Model.DomainModel.Finance;
using ETS.Util;

namespace Ets.Dao.Common.YeePay
{
    /// <summary>
    /// 易宝记录表 add by caoheyang 20150722
    /// </summary>
    public class YeePayRecordDao:DaoBase
    {
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="yeePayRecord"></param>
        /// <returns></returns>
        public long Insert(YeePayRecord yeePayRecord)
        {
            const string insertSql = @"
insert into YeePayRecord(RequestId,CustomerNumber,HmacKey,Ledgerno,SourceLedgerno,Amount,TransferType,Payer,Code,Hmac,Msg,CallbackUrl,Status,WithdrawId,UserType,Lastno,[Desc])
values(@RequestId,@CustomerNumber,@HmacKey,@Ledgerno,@SourceLedgerno,@Amount,@TransferType,@Payer,@Code,@Hmac,@Msg,@CallbackUrl,@Status,@WithdrawId,@UserType,@Lastno,@Desc)

select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("RequestId", yeePayRecord.RequestId);
            dbParameters.AddWithValue("CustomerNumber", yeePayRecord.CustomerNumber);
            dbParameters.AddWithValue("HmacKey", yeePayRecord.HmacKey);
            dbParameters.AddWithValue("Ledgerno", yeePayRecord.Ledgerno);
            dbParameters.AddWithValue("SourceLedgerno", yeePayRecord.SourceLedgerno);
            dbParameters.AddWithValue("Amount", yeePayRecord.Amount);
            dbParameters.AddWithValue("TransferType", yeePayRecord.TransferType);
            dbParameters.AddWithValue("Payer", yeePayRecord.Payer);
            dbParameters.AddWithValue("Code", yeePayRecord.Code);
            dbParameters.AddWithValue("Hmac", yeePayRecord.Hmac);
            dbParameters.AddWithValue("Msg", yeePayRecord.Msg);
            dbParameters.AddWithValue("CallbackUrl", yeePayRecord.CallbackUrl);
            dbParameters.AddWithValue("Status", yeePayRecord.Status);
            dbParameters.AddWithValue("WithdrawId", yeePayRecord.WithdrawId);
            dbParameters.AddWithValue("UserType", yeePayRecord.UserType);
            dbParameters.AddWithValue("Lastno", yeePayRecord.Lastno);
            dbParameters.AddWithValue("Desc", yeePayRecord.Desc);
            return  ParseHelper.ToLong(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
        }


        /// <summary>
        ///  根据请求号查询易宝提现记录 对应的提现单号等数据  
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public YeePayRecord GetReocordByRequestId(string requestId)
        {
            const string querysql = @"
SELECT Id,RequestId,UserType,WithdrawId  from YeePayRecord where TransferType=1 and RequestId=@RequestId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("RequestId", requestId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return DataTableHelper.ConvertDataTableList<YeePayRecord>(dt)[0];
            }
            return null;
        }
        /// <summary>
        /// 获取活跃易宝账户列表信息
        /// danny-20150729
        /// </summary>
        /// <param name="dateDiff"></param>
        /// <returns></returns>
        public IList<YeePayUser> GetYeePayUserList(int dateDiff=7)
        {
            string sql = @"
SELECT [Id]
      ,[UserId]
      ,[UserType]
      ,[RequestId]
      ,[CustomerNumberr]
      ,[HmacKey]
      ,[BindMobile]
      ,[CustomerType]
      ,[SignedName]
      ,[LinkMan]
      ,[IdCard]
      ,[BusinessLicence]
      ,[LegalPerson]
      ,[MinsettleAmount]
      ,[Riskreserveday]
      ,[BankAccountNumber]
      ,[BankName]
      ,[AccountName]
      ,[BankAccountType]
      ,[BankProvince]
      ,[BankCity]
      ,[ManualSettle]
      ,[Hmac]
      ,[Addtime]
      ,[Ledgerno]
      ,[BalanceRecord]
      ,[UpdateTime]
      ,[YeeBalance]
FROM [YeePayUser] ypu WITH(NOLOCK)
WHERE DATEDIFF(DAY,ypu.UpdateTime,GETDATE()) <=@DateDiff";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@DateDiff", dateDiff);
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            var list = ConvertDataTableList<YeePayUser>(dt);
            return list;
        }
       /// <summary>
        /// 修改本易宝账户余额
        /// danny-20150730
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
        public bool ModifyYeeBalance(YeePayUser model)
        {
            string sql = string.Format(@" 
update YeePayUser
set    YeeBalance=@YeeBalance,
       UpdateTime=getdate()
output Inserted.BalanceRecord
where Ledgerno=@YeepayKey;");
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@YeepayKey", model.YeeBalance);
            parm.AddWithValue("@YeeBalance", model.YeeBalance);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm)>0;
        }
        /// <summary>
        /// 获取金额异常易宝账户列表
        /// danny-20150730
        /// </summary>
        /// <returns></returns>
        public IList<YeePayUser> GetBalanceExceptYeePayUserList()
        {
            string sql = @"
SELECT [Id]
      ,[UserId]
      ,[UserType]
      ,[RequestId]
      ,[CustomerNumberr]
      ,[HmacKey]
      ,[BindMobile]
      ,[CustomerType]
      ,[SignedName]
      ,[LinkMan]
      ,[IdCard]
      ,[BusinessLicence]
      ,[LegalPerson]
      ,[MinsettleAmount]
      ,[Riskreserveday]
      ,[BankAccountNumber]
      ,[BankName]
      ,[AccountName]
      ,[BankAccountType]
      ,[BankProvince]
      ,[BankCity]
      ,[ManualSettle]
      ,[Hmac]
      ,[Addtime]
      ,[Ledgerno]
      ,ISNULL([BalanceRecord],0) BalanceRecord
      ,[UpdateTime]
      ,ISNULL([YeeBalance],0) YeeBalance
FROM [YeePayUser] ypu WITH(NOLOCK)
WHERE ISNULL(BalanceRecord,0)<>ISNULL(YeeBalance,0)";
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql));
            var list = ConvertDataTableList<YeePayUser>(dt);
            return list;
        }
        /// <summary>
        /// 获取预警骑士提现申请单（状态异常和回调超时）
        /// danny-20150730
        /// </summary>
        /// <returns></returns>
        public IList<ClienterWithdrawFormModel> GetWarnClienterWithdrawForm(int dateDiff=3)
        {
            string sql = @"
SELECT   WithwardNo
		,[Status]
		,PayFailedReason
        ,DATEDIFF(DAY,ISNULL(cwf.PayTime,GETDATE()),GETDATE()) DateDiff
FROM dbo.ClienterWithdrawForm cwf WITH(NOLOCK)
WHERE ([Status]=@StatusPaying 
    AND DATEDIFF(DAY,ISNULL(cwf.PayTime,GETDATE()),GETDATE())>@DateDiff) 
    OR [Status]=@StatusExcept;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@StatusPaying", ClienterWithdrawFormStatus.Paying.GetHashCode());
            parm.AddWithValue("@StatusExcept", ClienterWithdrawFormStatus.Except.GetHashCode());
            parm.AddWithValue("@DateDiff",dateDiff);
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            var list = ConvertDataTableList<ClienterWithdrawFormModel>(dt);
            return list;
        }
        /// <summary>
        /// 获取预警商户提现申请单（状态异常和回调超时）
        /// danny-20150730
        /// </summary>
        /// <returns></returns>
        public IList<BusinessWithdrawFormModel> GetWarnBusinessWithdrawForm(int dateDiff = 3)
        {
            string sql = @"
SELECT   bwf.WithwardNo
		,bwf.[Status]
		,bwf.PayFailedReason
        ,DATEDIFF(DAY,ISNULL(bwf.PayTime,GETDATE()),GETDATE()) DateDiff
FROM dbo.BusinessWithdrawForm bwf WITH(NOLOCK)
WHERE (bwf.[Status]=@StatusPaying 
    AND DATEDIFF(DAY,ISNULL(bwf.PayTime,GETDATE()),GETDATE())>@DateDiff) 
    OR bwf.[Status]=@StatusExcept;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@StatusPaying", BusinessWithdrawFormStatus.Paying.GetHashCode());
            parm.AddWithValue("@StatusExcept", BusinessWithdrawFormStatus.Except.GetHashCode());
            parm.AddWithValue("@DateDiff", dateDiff);
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            var list = ConvertDataTableList<BusinessWithdrawFormModel>(dt);
            return list;
        }

    }
}
