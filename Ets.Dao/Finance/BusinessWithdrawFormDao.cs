using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Util;

namespace Ets.Dao.Finance
{
    /// <summary>
    ///  商户提现表 数据访问类BusinessWithdrawFormDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 16:00:11
    /// </summary>

    public class BusinessWithdrawFormDao : DaoBase
    {
        public BusinessWithdrawFormDao()
        { }
        #region IBusinessWithdrawFormRepos  Members

        /// <summary>
        /// 增加一条记录
        /// <param name="businessWithdrawForm">参数实体</param>
        /// </summary>
        public long Insert(BusinessWithdrawForm businessWithdrawForm)
        {
            const string insertSql = @"
insert into BusinessWithdrawForm(WithwardNo,BusinessId,BalancePrice,AllowWithdrawPrice,Status,Amount,Balance,
TrueName,AccountNo,AccountType,BelongType,OpenBank,OpenSubBank,OpenProvince,OpenCity,OpenProvinceCode,OpenCityCode,IDCard,HandChargeThreshold,HandCharge,HandChargeOutlay,PhoneNo) 
values(@WithwardNo,@BusinessId,@BalancePrice,@AllowWithdrawPrice,@Status,@Amount,@Balance,
@TrueName,@AccountNo,@AccountType,@BelongType,@OpenBank,@OpenSubBank,@OpenProvince,@OpenCity,@OpenProvinceCode,@OpenCityCode,@IDCard,@HandChargeThreshold,@HandCharge,@HandChargeOutlay,@PhoneNo);
select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("WithwardNo", DbType.String).Value = businessWithdrawForm.WithwardNo; //提现单号
            dbParameters.AddWithValue("BusinessId", DbType.Int32).Value = businessWithdrawForm.BusinessId;  //商户ID(clienter表)
            dbParameters.AddWithValue("BalancePrice", DbType.Decimal).Value = businessWithdrawForm.BalancePrice;  //提现前商户余额
            dbParameters.AddWithValue("AllowWithdrawPrice", DbType.Decimal).Value = businessWithdrawForm.AllowWithdrawPrice;  //提现前商户可提现金额
            dbParameters.AddWithValue("Status", DbType.Int32).Value = businessWithdrawForm.Status;  //提现状态(1待审核 2 审核通过 3打款完成 -1审核拒绝 -2 打款失败)
            dbParameters.AddWithValue("Amount", DbType.Decimal).Value = businessWithdrawForm.Amount;  //提现金额
            dbParameters.AddWithValue("Balance", DbType.Decimal).Value = businessWithdrawForm.Balance;  //提现后余额 
            dbParameters.AddWithValue("TrueName", DbType.String).Value = businessWithdrawForm.TrueName; //商户收款户名
            dbParameters.AddWithValue("AccountNo", DbType.String).Value = businessWithdrawForm.AccountNo; //卡号(DES加密)
            dbParameters.AddWithValue("AccountType", DbType.Int32).Value = businessWithdrawForm.AccountType;//账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）
            dbParameters.AddWithValue("BelongType", DbType.Int32).Value = businessWithdrawForm.BelongType;//账号类别  0 个人账户 1 公司账户  
            dbParameters.AddWithValue("OpenBank", DbType.String).Value = businessWithdrawForm.OpenBank ?? ""; //开户行
            dbParameters.AddWithValue("OpenSubBank", DbType.String).Value = businessWithdrawForm.OpenSubBank ?? "";//开户支行
            dbParameters.Add("OpenProvince", DbType.String).Value = businessWithdrawForm.OpenProvince??""; //易宝省份
            dbParameters.Add("OpenCity", DbType.String).Value = businessWithdrawForm.OpenCity ?? "";//易宝城市
            dbParameters.Add("OpenProvinceCode", DbType.String).Value = businessWithdrawForm.OpenProvinceCode;//易宝省份代码  
            dbParameters.Add("OpenCityCode", DbType.String).Value = businessWithdrawForm.OpenCityCode; //易宝城市代码
            dbParameters.Add("IDCard", DbType.String).Value = businessWithdrawForm.IDCard;//身份证号
            dbParameters.Add("HandChargeThreshold", DbType.Decimal).Value = businessWithdrawForm.HandChargeThreshold;//手续费阈值  
            dbParameters.Add("HandCharge", DbType.Decimal).Value = businessWithdrawForm.HandCharge; //手续费
            dbParameters.Add("HandChargeOutlay", DbType.Int32).Value = businessWithdrawForm.HandChargeOutlay.GetHashCode();//手续费支付方
            dbParameters.Add("PhoneNo", DbType.String).Value = businessWithdrawForm.PhoneNo; //手机号
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters); //提现单号
            return ParseHelper.ToLong(result);
        }

        /// <summary>
        /// 更新一条记录
        /// <param name="businessWithdrawForm">参数实体</param>
        /// </summary>
        public void Update(BusinessWithdrawForm businessWithdrawForm)
        {
            const string updateSql = @"
update  BusinessWithdrawForm
set  Status=@Status,Auditor=@Auditor,AuditTime=@AuditTime,AuditFailedReason=@AuditFailedReason,Payer=@Payer,PayTime=@PayTime,PayFailedReason=@PayFailedReason
where  Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", businessWithdrawForm.Id);
            dbParameters.AddWithValue("Status", businessWithdrawForm.Status);
            dbParameters.AddWithValue("Auditor", businessWithdrawForm.Auditor);
            dbParameters.AddWithValue("AuditTime", businessWithdrawForm.AuditTime);
            dbParameters.AddWithValue("AuditFailedReason", businessWithdrawForm.AuditFailedReason);
            dbParameters.AddWithValue("Payer", businessWithdrawForm.Payer);
            dbParameters.AddWithValue("PayTime", businessWithdrawForm.PayTime);
            dbParameters.AddWithValue("PayFailedReason", businessWithdrawForm.PayFailedReason);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }



        /// <summary>
        /// 根据ID获取对象
        /// <param name="id">id</param>
        /// </summary>
        public BusinessWithdrawForm GetById(long id)
        {
            BusinessWithdrawForm model = new BusinessWithdrawForm();
            const string querysql = @"
select  Id,WithwardNo,BusinessId,BalancePrice,AllowWithdrawPrice,Status,Amount,Balance,
WithdrawTime,Auditor,AuditTime,AuditFailedReason,Payer,PayTime,PayFailedReason
,HandChargeThreshold,HandCharge,HandChargeOutlay 
from  BusinessWithdrawForm (nolock)
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<BusinessWithdrawForm>(dt)[0];
            }
            return model;
        }

        #endregion
        /// <summary>
        /// 获取提现中的商家提现单数据 
        /// 窦海超 
        /// 2015年8月26日 18:58:39
        /// </summary>
        /// <returns></returns>
        public IList<Ets.Model.DomainModel.Finance.BusinessWithdrawFormModel> GetBusinessWithdrawing()
        {
            string sql = @"
 SELECT ypr.RequestId,bwf.Id,bwf.Amount,
  ypr.Ledgerno,ypr.CustomerNumber
   FROM 
 dbo.BusinessWithdrawForm bwf (nolock)
 join dbo.YeePayRecord ypr(nolock) on bwf.Id=ypr.WithdrawId and TransferType=1 and ypr.UserType=1 --商家 
  where bwf.Status=20";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<Ets.Model.DomainModel.Finance.BusinessWithdrawFormModel>(dt);
        }
    }
}
