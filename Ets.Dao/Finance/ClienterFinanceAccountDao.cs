﻿using System;
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
    /// 骑士金融账号表 数据访问类ClienterFinanceAccountDao。
    /// Generate By: tools.etaoshi.com   caoheyang
    /// Generate Time: 2015-05-09 14:59:36
    /// </summary>
    public class ClienterFinanceAccountDao : DaoBase
    {
        #region IClienterFinanceAccountRepos  Members
        public ClienterFinanceAccountDao()
        {

        }
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="clienterFinanceAccount">参数实体</param>
        /// <returns></returns>
        public int Insert(ClienterFinanceAccount clienterFinanceAccount)
        {
            const string insertSql = @"
insert into ClienterFinanceAccount(ClienterId,TrueName,AccountNo,IsEnable,AccountType,BelongType,OpenBank,OpenSubBank,CreateBy,UpdateBy)
values(@ClienterId,@TrueName,@AccountNo,@IsEnable,@AccountType,@BelongType,@OpenBank,@OpenSubBank,@CreateBy,@UpdateBy)
select @@IDENTITY";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterFinanceAccount.ClienterId); //骑士ID
            dbParameters.AddWithValue("TrueName", clienterFinanceAccount.TrueName); //户名
            dbParameters.AddWithValue("AccountNo", clienterFinanceAccount.AccountNo); //卡号(DES加密)
            dbParameters.AddWithValue("IsEnable", clienterFinanceAccount.IsEnable);  //是否有效(1：有效 0：无效）
            dbParameters.AddWithValue("AccountType", clienterFinanceAccount.AccountType); //账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）
            dbParameters.AddWithValue("BelongType", clienterFinanceAccount.BelongType); //账号类别  0 个人账户 1 公司账户  
            dbParameters.AddWithValue("OpenBank", clienterFinanceAccount.OpenBank); //开户行
            dbParameters.AddWithValue("OpenSubBank", clienterFinanceAccount.OpenSubBank); //开户支行
            dbParameters.AddWithValue("CreateBy", clienterFinanceAccount.CreateBy); //添加人
            dbParameters.AddWithValue("UpdateBy", clienterFinanceAccount.UpdateBy); //最后更新人
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToInt(result);
        }

        /// <summary>
        /// 更新一条记录
        /// <param name="clienterFinanceAccount"> 参数实体</param>
        /// </summary>
        public void Update(ClienterFinanceAccount clienterFinanceAccount)
        {
            const string updateSql = @"
update  ClienterFinanceAccount
set  TrueName=@TrueName,AccountNo=@AccountNo,BelongType=@BelongType,OpenBank=@OpenBank,
OpenSubBank=@OpenSubBank,UpdateBy=@UpdateBy
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", clienterFinanceAccount.Id);
            dbParameters.AddWithValue("TrueName", clienterFinanceAccount.TrueName);
            dbParameters.AddWithValue("AccountNo", clienterFinanceAccount.AccountNo);
            dbParameters.AddWithValue("BelongType", clienterFinanceAccount.BelongType); //账号类别  0 个人账户 1 公司账户  
            dbParameters.AddWithValue("OpenBank", clienterFinanceAccount.OpenBank);
            dbParameters.AddWithValue("OpenSubBank", clienterFinanceAccount.OpenSubBank);
            dbParameters.AddWithValue("UpdateBy", clienterFinanceAccount.UpdateBy);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        public void Delete(int id)
        {

        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="clienterFinanceAccountPm">参数实体</param>
        /// <returns></returns>
        public IList<ClienterFinanceAccount> Query(ClienterFinanceAccountPM clienterFinanceAccountPm)
        {
            IList<ClienterFinanceAccount> models = new List<ClienterFinanceAccount>();
            string condition = BindQueryCriteria(clienterFinanceAccountPm);
            string querysql = @"
select  Id,ClienterId,TrueName,AccountNo,IsEnable,AccountType,OpenBank,OpenSubBank,CreateBy,CreateTime,UpdateBy,UpdateTime
from  ClienterFinanceAccount (nolock)" + condition;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<ClienterFinanceAccount>(dt);
            }
            return models;
        }

        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        public ClienterFinanceAccount GetById(int id)
        {
            ClienterFinanceAccount model = null;
            const string querysql = @"
select  Id,ClienterId,TrueName,AccountNo,IsEnable,AccountType,BelongType,OpenBank,OpenSubBank,CreateBy,CreateTime,UpdateBy,UpdateTime
from  ClienterFinanceAccount(nolock)  
where  Id=@Id  and IsEnable=1";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<ClienterFinanceAccount>(dt)[0];
            }
            return model;
        }


        /// <summary>
        /// 根据骑士ID获取当前骑士的所有有效金融账户 add by caoheyang 20150511
        /// </summary>
        /// <param name="clienterId">骑士ID</param>
        /// <returns></returns>
        public IList<ClienterFinanceAccount> GetByClienterId(int clienterId)
        {
            IList<ClienterFinanceAccount> models = new List<ClienterFinanceAccount>();
            const string querysql = @"
select  Id,ClienterId,TrueName,AccountNo,IsEnable,AccountType,BelongType,OpenBank,OpenSubBank,CreateBy,CreateTime,UpdateBy,UpdateTime
from  ClienterFinanceAccount  
where  ClienterId=@ClienterId and IsEnable=1";  //事物内不加锁
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<ClienterFinanceAccount>(dt);
            }
            return models;
        }

        /// <summary>
        /// 根据骑士ID获取当前骑士的所有有效金融账户数量 add by caoheyang 20150511
        /// </summary>
        /// <param name="clienterId">骑士ID</param>
        /// <returns></returns>
        public int GetCountByClienterId(int clienterId)
        {
            IList<ClienterFinanceAccount> models = new List<ClienterFinanceAccount>();
            const string querysql = @"
select  Count(Id)
from  ClienterFinanceAccount  
where  ClienterId=@ClienterId and IsEnable=1";  //事物内不加锁
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterId);
           return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, querysql, dbParameters));     
        }

        #endregion

        #region  Other Members

        /// <summary>
        /// 构造查询条件
        /// <param name="clienterFinanceAccountPm">参数实体</param>
        /// </summary>
        public static string BindQueryCriteria(ClienterFinanceAccountPM clienterFinanceAccountPm)
        {
            var stringBuilder = new StringBuilder(" where 1=1 ");
            if (clienterFinanceAccountPm == null)
            {
                return stringBuilder.ToString();
            }
            //TODO:在此加入查询条件构建代码
            return stringBuilder.ToString();
        }
        #endregion
    }
}
