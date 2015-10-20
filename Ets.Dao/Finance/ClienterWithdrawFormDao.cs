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
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
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
TrueName,AccountNo,AccountType,BelongType,OpenBank,OpenSubBank,OpenProvince,OpenCity,OpenProvinceCode,OpenCityCode,IDCard,HandChargeThreshold,HandCharge,HandChargeOutlay,PhoneNo) values(@WithwardNo,@ClienterId,@BalancePrice,@AllowWithdrawPrice,@Status,@Amount,@Balance,
@TrueName,@AccountNo,@AccountType,@BelongType,@OpenBank,@OpenSubBank,@OpenProvince,@OpenCity,@OpenProvinceCode,@OpenCityCode,@IDCard,@HandChargeThreshold,@HandCharge,@HandChargeOutlay,@PhoneNo) ;select @@IDENTITY ";
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
            dbParameters.Add("PhoneNo", DbType.String).Value = clienterWithdrawForm.PhoneNo; //手机号
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
            var dbParameters = DbHelper.CreateDbParameters();
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
 where a.[Status] in(1,2,20) and a.ClienterId=@clienterId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@clienterId", DbType.Int32).Value = clienterId;
            var count = DbHelper.ExecuteScalar(SuperMan_Read, querysql, dbParameters);
            return ParseHelper.ToInt(count, 0);
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
            if (clienterWithdrawFormPm.Status>-100 )
            {
                stringBuilder.Append(" and Status=" + clienterWithdrawFormPm.Status);
            }
            //TODO:在此加入查询条件构建代码
            return stringBuilder.ToString();
        }
        #endregion

        /// <summary>
        /// 获取提现中的骑士提现单数据 
        /// 窦海超 
        /// 2015年8月26日 18:58:39
        /// </summary>
        /// <returns></returns>
        public IList<Ets.Model.DomainModel.Finance.ClienterWithdrawFormModel> GetClienterWithdrawing()
        {
            string sql = @"
SELECT ypr.RequestId,cwf.Id,cwf.Amount,
  ypr.Ledgerno,CustomerNumber
   FROM 
 dbo.ClienterWithdrawForm cwf (nolock)
 join dbo.YeePayRecord ypr(nolock) on cwf.Id=ypr.WithdrawId and TransferType=1 and ypr.UserType=0 and cwf.AccountType=@AccountType
  where cwf.Status=20 ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@AccountType", ClienterFinanceAccountType.WangYin.GetHashCode());//账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）
            DataTable dt= DbHelper.ExecuteDataTable(SuperMan_Read,sql);
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<Ets.Model.DomainModel.Finance.ClienterWithdrawFormModel>(dt);
        }

        /// <summary>
        /// 胡灵波
        /// 临时修复数据使用
        /// 2015年9月14日 16:14:11
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable Query(int status)
        {
            string querysql = @"
select  Id,WithwardNo,ClienterId,BalancePrice,AllowWithdrawPrice,Status,Amount,Balance,WithdrawTime,Auditor,AuditTime,AuditFailedReason,Payer,PayTime,PayFailedReason
from  ClienterWithdrawForm (nolock) where Status=" + status;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql));
            return dt;
        }

        /// <summary>
        /// 支付宝批量付款到账(写串)
        /// 茹化肖
        /// 2015年10月20日09:19:06
        /// </summary>
        /// <param name="type">1根据提现单ID进行打款.2 将已有批次号再次提交</param>
        /// <param name="data">type=1:以英文逗号分隔的提现单ID序列 type=2:已存在的批次号</param>
        /// <returns></returns>
        public List<AlipayClienterWithdrawModel> GetWithdrawListForAlipay(AlipayBatchPM pm)
        {
            List<AlipayClienterWithdrawModel> list = null;
            StringBuilder querystr =new StringBuilder(@"
--查询提现单信息
SELECT  Id,--提现单ID
        WithwardNo,--提现单号
        Amount ,--提现金额
        TrueName ,--真实姓名
        AccountNo ,--支付宝账户
        PaidAmount ,--实付金额
        HandCharge,--手续费
        AlipayBatchNo
FROM    dbo.ClienterWithdrawForm AS cwf
WHERE   1 = 1  ");
            string where = "";
            if (pm.Type == 1) //拼接id
            {
                 where = string.Format(@"AND cwf.Status = 2 --审核通过
                                               AND AccountType = 2 --支付宝账户
                                               AND ISNULL(cwf.AlipayBatchNo, '') = ''
                                               AND cwf.id IN ({0})", pm.Data);
            }
            else //拼接批次号
            {
                where = string.Format(@"AND cwf.Status = 20 --打款中
                                        AND AccountType = 2 --支付宝账户
                                        AND ISNULL(cwf.AlipayBatchNo, '') = '{0}'", pm.Data);
            }
            querystr.Append(where);
            DataTable dt= DbHelper.ExecuteDataTable(SuperMan_Write,querystr.ToString());
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<AlipayClienterWithdrawModel>(dt).ToList();
        }

        /// <summary>
        /// 添加批次号且修改状态为打款中
        /// 茹化肖
        /// 2015年10月20日11:40:03
        /// </summary>
        /// <returns>影响行数</returns>
        public int AddAlipayBatchNo(long id,string batchNo)
        {
            string updatestr = @"UPDATE ClienterWithdrawForm SET AlipayBatchNo=@AlipayBatchNo,Status=@Status WHERE id=@ID";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("AlipayBatchNo", DbType.String).Value = batchNo;
            dbParameters.Add("Status", DbType.Int32).Value = ClienterWithdrawFormStatus.Paying.GetHashCode();
            dbParameters.Add("ID", DbType.Int32).Value = id;
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updatestr, dbParameters);
        }
        /// <summary>
        /// 插入支付宝批次号
        /// 茹化肖
        /// 2015年10月20日13:06:11
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertAlipayBatch(AlipayBatchModel model)
        {
            string updatestr = @"  INSERT INTO dbo.AlipayBatch
              ( BatchNo ,
                TotalWithdraw ,
                OptTimes ,
                WithdrawNos ,
                WithdrawIds ,
                CreateBy ,
                LastOptUser ,
                Remarks
              )
      VALUES  ( 
				@BatchNo ,
                @TotalWithdraw ,
                @OptTimes ,
                @WithdrawNos ,
                @WithdrawIds ,
                @CreateBy ,
                @LastOptUser ,
                @Remarks
              )";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("BatchNo", DbType.String).Value = model.BatchNo;
            dbParameters.Add("TotalWithdraw", DbType.Decimal).Value = model.TotalWithdraw;
            dbParameters.Add("OptTimes", DbType.Int32).Value = model.OptTimes;
            dbParameters.Add("WithdrawNos", DbType.String).Value = model.WithdrawNos;
            dbParameters.Add("WithdrawIds", DbType.String).Value = model.WithdrawIds;
            dbParameters.Add("CreateBy", DbType.String).Value = model.CreateBy;
            dbParameters.Add("LastOptUser", DbType.String).Value = model.LastOptUser;
            dbParameters.Add("Remarks", DbType.String).Value = model.Remarks;
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updatestr, dbParameters);
        }

        /// <summary>
        /// 插入支付宝批次号
        /// 茹化肖
        /// 2015年10月20日13:06:11
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateAlipayBatch(AlipayBatchModel model)
        {
            string updatestr = @" UPDATE  dbo.AlipayBatch
SET     SuccessTimes = @SuccessTimes ,
        FailTimes = @FailTimes ,
        Status = 1 ,
        CallbackTime = GETDATE()
WHERE   BatchNo = @BatchNo";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("BatchNo", DbType.String).Value = model.BatchNo;
            dbParameters.Add("FailTimes", DbType.Int32).Value = model.FailTimes;
            dbParameters.Add("SuccessTimes", DbType.Int32).Value = model.SuccessTimes;
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updatestr, dbParameters);
        }

    }
}
