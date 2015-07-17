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
    ///  骑士提现表 数据访问类ClienterWithdrawFormDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 16:00:11
    /// </summary>

    public class ClienterWithdrawFormDao : DaoBase
    {
        public ClienterWithdrawFormDao()
        { }
        #region IClienterWithdrawFormRepos  Members

        /// <summary>
        /// 增加一条记录
        /// <param name="clienterWithdrawForm">参数实体</param>
        /// </summary>
        public long Insert(ClienterWithdrawForm clienterWithdrawForm)
        {
            //TODO 修改ADD
            const string insertSql = @"
insert into ClienterWithdrawForm(WithwardNo,ClienterId,BalancePrice,AllowWithdrawPrice,Status,Amount,Balance,
TrueName,AccountNo,AccountType,BelongType,OpenBank,OpenSubBank,OpenProvince,OpenCity,OpenProvinceCode,OpenCityCode,IDCard,HandChargeThreshold,HandCharge,HandChargeOutlay)
values(@WithwardNo,@ClienterId,@BalancePrice,@AllowWithdrawPrice,@Status,@Amount,@Balance,
@TrueName,@AccountNo,@AccountType,@BelongType,@OpenBank,@OpenSubBank,@OpenProvince,@OpenCity,@OpenProvinceCode,@OpenCityCode,@IDCard,@HandChargeThreshold,@HandCharge,@HandChargeOutlay)

select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("WithwardNo", clienterWithdrawForm.WithwardNo); //提现单号
            dbParameters.AddWithValue("ClienterId", clienterWithdrawForm.ClienterId);  //骑士ID(clienter表)
            dbParameters.AddWithValue("BalancePrice", clienterWithdrawForm.BalancePrice);  //提现前骑士余额
            dbParameters.AddWithValue("AllowWithdrawPrice", clienterWithdrawForm.AllowWithdrawPrice);  //提现前骑士可提现金额
            dbParameters.AddWithValue("Status", clienterWithdrawForm.Status);  //提现状态(1待审核 2 审核通过 3打款完成 -1审核拒绝 -2 打款失败)
            dbParameters.AddWithValue("Amount", clienterWithdrawForm.Amount);  //提现金额
            dbParameters.AddWithValue("Balance", clienterWithdrawForm.Balance);  //提现后余额 
            dbParameters.AddWithValue("TrueName", clienterWithdrawForm.TrueName); //骑士收款户名
            dbParameters.AddWithValue("AccountNo", clienterWithdrawForm.AccountNo); //卡号(DES加密)
            dbParameters.AddWithValue("AccountType", clienterWithdrawForm.AccountType);//账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）
            dbParameters.AddWithValue("BelongType", clienterWithdrawForm.BelongType);//账号类别  0 个人账户 1 公司账户  
            dbParameters.AddWithValue("OpenBank", clienterWithdrawForm.OpenBank); //开户行
            dbParameters.AddWithValue("OpenSubBank", clienterWithdrawForm.OpenSubBank);//开户支行
            dbParameters.AddWithValue("OpenProvince", clienterWithdrawForm.OpenProvince); //易宝省份
            dbParameters.AddWithValue("OpenCity", clienterWithdrawForm.OpenCity);//易宝城市
            dbParameters.AddWithValue("OpenProvinceCode", clienterWithdrawForm.OpenProvinceCode);//易宝省份代码  
            dbParameters.AddWithValue("OpenCityCode", clienterWithdrawForm.OpenCityCode); //易宝城市代码
            dbParameters.AddWithValue("IDCard", clienterWithdrawForm.IDCard);//身份证号
            dbParameters.AddWithValue("HandChargeThreshold", clienterWithdrawForm.HandChargeThreshold);//手续费阈值  
            dbParameters.AddWithValue("HandCharge", clienterWithdrawForm.HandCharge); //手续费
            dbParameters.AddWithValue("HandChargeOutlay", (object)clienterWithdrawForm.HandChargeOutlay);//手续费支付方
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters); //提现单号
            return ParseHelper.ToLong(result);
        }

        /// <summary>
        /// 更新一条记录
        /// <param name="clienterWithdrawForm">参数实体</param>
        /// </summary>
        public void Update(ClienterWithdrawForm clienterWithdrawForm)
        {
            const string updateSql = @"
update  ClienterWithdrawForm
set  Status=@Status,Auditor=@Auditor,AuditTime=@AuditTime,AuditFailedReason=@AuditFailedReason,Payer=@Payer,PayTime=@PayTime,PayFailedReason=@PayFailedReason
where  Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", clienterWithdrawForm.Id);
            dbParameters.AddWithValue("Status", clienterWithdrawForm.Status);
            dbParameters.AddWithValue("Auditor", clienterWithdrawForm.Auditor);
            dbParameters.AddWithValue("AuditTime", clienterWithdrawForm.AuditTime);
            dbParameters.AddWithValue("AuditFailedReason", clienterWithdrawForm.AuditFailedReason);
            dbParameters.AddWithValue("Payer", clienterWithdrawForm.Payer);
            dbParameters.AddWithValue("PayTime", clienterWithdrawForm.PayTime);
            dbParameters.AddWithValue("PayFailedReason", clienterWithdrawForm.PayFailedReason);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 删除一条记录
        /// <param name="id">id</param>
        /// </summary>
        public void Delete(long id)
        {

        }

        /// <summary>
        /// 查询对象
        /// <param name="clienterWithdrawFormPm"> 参数实体</param>
        /// </summary>
        public IList<ClienterWithdrawForm> Query(ClienterWithdrawFormPM clienterWithdrawFormPm)
        {
            IList<ClienterWithdrawForm> models = new List<ClienterWithdrawForm>();
            string condition = BindQueryCriteria(clienterWithdrawFormPm);
            string querysql = @"
select  Id,WithwardNo,ClienterId,BalancePrice,AllowWithdrawPrice,Status,Amount,Balance,WithdrawTime,Auditor,AuditTime,AuditFailedReason,Payer,PayTime,PayFailedReason
from  ClienterWithdrawForm (nolock)" + condition;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<ClienterWithdrawForm>(dt);
            }
            return models;
        }


        /// <summary>
        /// 根据ID获取对象
        /// <param name="id">id</param>
        /// </summary>
        public ClienterWithdrawForm GetById(long id)
        {
            ClienterWithdrawForm model = new ClienterWithdrawForm();
            const string querysql = @"
select  Id,WithwardNo,ClienterId,BalancePrice,AllowWithdrawPrice,Status,Amount,Balance,WithdrawTime,
Auditor,AuditTime,AuditFailedReason,Payer,PayTime,PayFailedReason,HandChargeThreshold,HandCharge,HandChargeOutlay 
from  ClienterWithdrawForm (nolock)
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<ClienterWithdrawForm>(dt)[0];
            }
            return model;
        }

        /// <summary>
        /// 判断该骑士是否有未完成的体现申请单
        /// <param name="clienterId">id</param>
        /// </summary>
        public int GetByClienterId(int clienterId)
        {
            const string querysql = @" select count(1) FROM dbo.ClienterWithdrawForm(nolock) a
 where a.[Status]=2 and a.ClienterId=@clienterId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@clienterId", DbType.Int32).Value = clienterId;
            var count = DbHelper.ExecuteScalar(SuperMan_Read, querysql, dbParameters); 
            return ParseHelper.ToInt(count,0);
        }


        #endregion
        #region  Other Members

        /// <summary>
        /// 构造查询条件
        /// <param name="clienterWithdrawFormPm"> 参数实体</param>
        /// </summary>
        public static string BindQueryCriteria(ClienterWithdrawFormPM clienterWithdrawFormPm)
        {
            var stringBuilder = new StringBuilder(" where 1=1 ");
            if (clienterWithdrawFormPm == null)
            {
                return stringBuilder.ToString();
            }
            //TODO:在此加入查询条件构建代码
            return stringBuilder.ToString();
        }
        #endregion
    }
}
