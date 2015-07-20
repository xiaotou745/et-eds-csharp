using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using ETS.Util;

namespace Ets.Dao.Finance
{
    /// <summary>
    /// 商户金融账号表 数据访问类BusinessFinanceAccountDao。
    /// Generate By: tools.etaoshi.com   caoheyang
    /// Generate Time: 2015-05-09 14:59:36
    /// </summary>
    public class BusinessFinanceAccountDao : DaoBase
    {
        #region IBusinessFinanceAccountRepos  Members
        public BusinessFinanceAccountDao()
        {

        }
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="businessFinanceAccount">参数实体</param>
        /// <returns></returns>
        public int Insert(BusinessFinanceAccount businessFinanceAccount)
        {
            const string insertSql = @"
insert into BusinessFinanceAccount(BusinessId,TrueName,AccountNo,IsEnable,AccountType,BelongType,OpenBank,OpenSubBank,OpenProvince,OpenCity,IDCard,CreateBy,UpdateBy)
values(@BusinessId,@TrueName,@AccountNo,@IsEnable,@AccountType,@BelongType,@OpenBank,@OpenSubBank,@OpenProvince,@OpenCity,@IDCard,@CreateBy,@UpdateBy)
select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", businessFinanceAccount.BusinessId); //商户ID
            dbParameters.AddWithValue("TrueName", businessFinanceAccount.TrueName); //户名
            dbParameters.AddWithValue("AccountNo", businessFinanceAccount.AccountNo); //卡号(DES加密)
            dbParameters.AddWithValue("IsEnable", businessFinanceAccount.IsEnable);  //是否有效(1：有效 0：无效）
            dbParameters.AddWithValue("AccountType", businessFinanceAccount.AccountType); //账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）
            dbParameters.AddWithValue("BelongType", businessFinanceAccount.BelongType); //账号类别  0 个人账户 1 公司账户
            dbParameters.AddWithValue("OpenBank", businessFinanceAccount.OpenBank); //开户行
            dbParameters.AddWithValue("OpenSubBank", businessFinanceAccount.OpenSubBank); //开户支行
            dbParameters.AddWithValue("CreateBy", businessFinanceAccount.CreateBy); //添加人
            dbParameters.AddWithValue("UpdateBy", businessFinanceAccount.UpdateBy); //最后更新人
            dbParameters.AddWithValue("IDCard", businessFinanceAccount.IDCard); //身份证
            dbParameters.AddWithValue("OpenProvince", businessFinanceAccount.OpenProvince); //开户省
            dbParameters.AddWithValue("OpenCity", businessFinanceAccount.OpenCity); //开户市
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToInt(result);
        }

        /// <summary>
        /// 更新一条记录
        /// <param name="businessFinanceAccount"> 参数实体</param>
        /// </summary>
        public void Update(BusinessFinanceAccount businessFinanceAccount)
        {
            const string updateSql = @"
update  BusinessFinanceAccount
set  TrueName=@TrueName,AccountNo=@AccountNo,BelongType=@BelongType,OpenBank=@OpenBank,
OpenSubBank=@OpenSubBank,UpdateBy=@UpdateBy,OpenProvince=@OpenProvince,OpenCity=@OpenCity,IDCard=@IDCard 
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", businessFinanceAccount.Id);
            dbParameters.AddWithValue("TrueName", businessFinanceAccount.TrueName);
            dbParameters.AddWithValue("AccountNo", businessFinanceAccount.AccountNo);
            dbParameters.AddWithValue("BelongType", businessFinanceAccount.BelongType); //账号类别  0 个人账户 1 公司账户
            dbParameters.AddWithValue("OpenBank", businessFinanceAccount.OpenBank);
            dbParameters.AddWithValue("OpenSubBank", businessFinanceAccount.OpenSubBank);
            dbParameters.AddWithValue("UpdateBy", businessFinanceAccount.UpdateBy);
            dbParameters.Add("OpenProvince", DbType.String).Value = businessFinanceAccount.OpenProvince;
            dbParameters.Add("OpenCity", DbType.String).Value = businessFinanceAccount.OpenCity;
            dbParameters.Add("IDCard", DbType.String).Value = businessFinanceAccount.IDCard;
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 更新易宝信息通过Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <param name="yeepayKey">易宝Key</param>
        /// <param name="yeepayStatus">易宝账户状态  0正常 1失败</param>
        public void UpdateYeepayInfoById(int id, string yeepayKey, byte yeepayStatus)
        {
            const string updateSql = @"
update  BusinessFinanceAccount
set  YeepayKey=@YeepayKey,YeepayStatus=@YeepayStatus
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("YeepayKey", yeepayKey);
            dbParameters.AddWithValue("YeepayStatus", yeepayStatus);
            dbParameters.AddWithValue("Id", id);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }
        /// <summary>
        /// 更新易宝信息通过Id（带返回值）
        /// danny-20150716
        /// </summary>
        /// <param name="id"></param>
        /// <param name="yeepayKey"></param>
        /// <param name="yeepayStatus"></param>
        /// <returns></returns>
        public bool ModifyYeepayInfoById(int id, string yeepayKey, byte yeepayStatus)
        {
            const string updateSql = @"
update  BusinessFinanceAccount
set  YeepayKey=@YeepayKey,YeepayStatus=@YeepayStatus
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("YeepayKey", yeepayKey);
            dbParameters.AddWithValue("YeepayStatus", yeepayStatus);
            dbParameters.AddWithValue("Id", id);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters) > 0;
        }

        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        public BusinessFinanceAccount GetById(int id)
        {
            BusinessFinanceAccount model = null;
            const string querysql = @"
 select  a.Id,a.BusinessId,a.TrueName,a.AccountNo,a.IsEnable,a.AccountType,a.BelongType,a.OpenBank,a.OpenSubBank,a.CreateBy,a.CreateTime,a.UpdateBy,a.UpdateTime,a.OpenProvince,a.OpenCity,a.IDCard,b.PhoneNo
from  dbo.BusinessFinanceAccount (nolock) a join dbo.business b (nolock) on a.BusinessId = b.Id
where  a.Id=@Id and a.IsEnable=1 ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<BusinessFinanceAccount>(dt)[0];
            }
            return model;
        }


        /// <summary>
        /// 根据商户ID获取当前商户的所有有效金融账户 add by caoheyang 20150511
        /// </summary>
        /// <param name="businessId">商户ID</param>
        /// <returns></returns>
        public IList<BusinessFinanceAccount> GetByBusinessId(int businessId)
        {
            IList<BusinessFinanceAccount> models = new List<BusinessFinanceAccount>();
            const string querysql = @"
select  Id,BusinessId,TrueName,AccountNo,IsEnable,AccountType,BelongType,OpenBank,OpenSubBank,CreateBy,CreateTime,UpdateBy,UpdateTime
from  BusinessFinanceAccount  (nolock)
where  BusinessId=@BusinessId and IsEnable=1";  //事物内不加锁
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", businessId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<BusinessFinanceAccount>(dt);
            }
            return models;
        }

        /// <summary>
        /// 根据商户ID获取当前商户的所有有效金融账户数量 add by caoheyang 20150511
        /// </summary>
        /// <param name="businessId">商户ID</param>
        /// <returns></returns>
        public int GetCountByBusinessId(int businessId)
        {
            IList<BusinessFinanceAccount> models = new List<BusinessFinanceAccount>();
            const string querysql = @"
select  Count(Id)
from  BusinessFinanceAccount  
where  BusinessId=@BusinessId and IsEnable=1";  //事物内不加锁
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", businessId);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, querysql, dbParameters));
        }

        /// <summary>
        /// 获取商家账号Id
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150626</UpdateTime>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetBFinanceAccountId(int businessId)
        {
            string querysql = @"  
select id from BusinessFinanceAccount nolock
where BusinessId=@businessId";

            var dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@businessId", businessId);

            object obj = DbHelper.ExecuteScalar(SuperMan_Read, querysql, dbParameters);
            return ParseHelper.ToInt(obj, 0);
        }
        #endregion

        public BusinessFinanceAccount GetFinanceAccountByBusiId(int busiId)
        {
            string selSql = @"select top 1 b.Id ,
        b.BusinessId ,
        b.TrueName ,
        b.AccountNo ,
        b.AccountType ,
        b.BelongType ,
        b.OpenBank ,
        b.OpenSubBank ,
        b.IDCard ,
        b.OpenProvince ,
        b.OpenCity ,
        b.YeepayKey ,
        b.YeepayStatus FROM dbo.BusinessFinanceAccount b (nolock)
        where b.IsEnable = 1 and b.BusinessId = @BusinessId
        order by b.Id desc";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", busiId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return DataTableHelper.ConvertDataTableList<BusinessFinanceAccount>(dt)[0];
            }
            return null;
        }
    }
}
