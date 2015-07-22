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
    /// 易宝用户表 add by caoheyang 20150722
    /// </summary>
    public class YeePayUserDao : DaoBase
    {

        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="yeePayUser"></param>
        /// <returns></returns>
        public long Insert(YeePayUser yeePayUser)
        {
            const string insertSql = @"
insert into YeePayUser(UserId,UserType,RequestId,CustomerNumberr,HmacKey,BindMobile,CustomerType,SignedName,LinkMan,IdCard,BusinessLicence,LegalPerson,MinsettleAmount,Riskreserveday,BankAccountNumber,BankName,AccountName,BankAccountType,BankProvince,BankCity,ManualSettle,Hmac,Addtime,Ledgerno,BalanceRecord)
values(@UserId,@UserType,@RequestId,@CustomerNumberr,@HmacKey,@BindMobile,@CustomerType,@SignedName,@LinkMan,@IdCard,@BusinessLicence,@LegalPerson,@MinsettleAmount,@Riskreserveday,@BankAccountNumber,@BankName,@AccountName,@BankAccountType,@BankProvince,@BankCity,@ManualSettle,@Hmac,@Addtime,@Ledgerno,@BalanceRecord)

select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("UserId", yeePayUser.UserId);
            dbParameters.AddWithValue("UserType", yeePayUser.UserType);
            dbParameters.AddWithValue("RequestId", yeePayUser.RequestId);
            dbParameters.AddWithValue("CustomerNumberr", yeePayUser.CustomerNumberr);
            dbParameters.AddWithValue("HmacKey", yeePayUser.HmacKey);
            dbParameters.AddWithValue("BindMobile", yeePayUser.BindMobile);
            dbParameters.AddWithValue("CustomerType", yeePayUser.CustomerType);
            dbParameters.AddWithValue("SignedName", yeePayUser.SignedName);
            dbParameters.AddWithValue("LinkMan", yeePayUser.LinkMan);
            dbParameters.AddWithValue("IdCard", yeePayUser.IdCard);
            dbParameters.AddWithValue("BusinessLicence", yeePayUser.BusinessLicence);
            dbParameters.AddWithValue("LegalPerson", yeePayUser.LegalPerson);
            dbParameters.AddWithValue("MinsettleAmount", yeePayUser.MinsettleAmount);
            dbParameters.AddWithValue("Riskreserveday", yeePayUser.Riskreserveday);
            dbParameters.AddWithValue("BankAccountNumber", yeePayUser.BankAccountNumber);
            dbParameters.AddWithValue("BankName", yeePayUser.BankName);
            dbParameters.AddWithValue("AccountName", yeePayUser.AccountName);
            dbParameters.AddWithValue("BankAccountType", yeePayUser.BankAccountType);
            dbParameters.AddWithValue("BankProvince", yeePayUser.BankProvince);
            dbParameters.AddWithValue("BankCity", yeePayUser.BankCity);
            dbParameters.AddWithValue("ManualSettle", yeePayUser.ManualSettle);
            dbParameters.AddWithValue("Hmac", yeePayUser.Hmac);
            dbParameters.AddWithValue("Addtime", yeePayUser.Addtime);
            dbParameters.AddWithValue("Ledgerno", yeePayUser.Ledgerno);
            dbParameters.AddWithValue("BalanceRecord", yeePayUser.BalanceRecord);

            return ParseHelper.ToLong(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));

        }

    }
}
