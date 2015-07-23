using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using Ets.Model.Common.YeePay;
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

    }
}
