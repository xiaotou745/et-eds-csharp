using Ets.Model.DataModel.Account;
using ETS.Dao;
using ETS.Data.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Account
{
    public class AccountLoginLogDao : DaoBase
    {
        /// <summary>
        /// 登录日志
        /// 窦海超
        /// 2015年8月18日 12:21:49
        /// </summary>
        /// <param name="model"></param>
        public void Insert(AccountLoginLogModel model)
        {
            string sql = @"
insert into dbo.AccountLoginLog ( LoginName, LoginTime, Mac, LoginType, Ip,
                                   Browser, Remark )
values  ( @LoginName, -- LoginName - nvarchar(50)
          getdate(), -- LoginTime - datetime
          @Mac, -- Mac - nvarchar(50)
          @LoginType, -- LoginType - int
          @Ip, -- Ip - varchar(50)
          @Browser, -- Browser - nvarchar(50)
          @Remark  -- Remark - nvarchar(500)
          )";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("LoginName", DbType.String, 50).Value = model.LoginName;
            parm.Add("Mac", DbType.String, 50).Value = model.Mac;
            parm.Add("LoginType", DbType.Int32, 4).Value = model.LoginType;
            parm.Add("Ip", DbType.String, 50).Value = model.Ip;
            parm.Add("Browser", DbType.String, 50).Value = model.Browser;
            parm.Add("Remark", DbType.String, 500).Value = model.Remark;
            DbHelper.ExecuteNonQuery(SuperMan_Write,sql,parm);
        }
    }
}
