﻿using ETS;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.User
{
    public class UserDao : DaoBase
    {
        public virtual void RegisterToSql(Ets.Model.UserModel user)
        {
            #region 增加demo
            //const string insertSql = @"insert into user(............) values (@userid, @username) select @@identity";

            //IDbParameters dbParameters = DbHelper.CreateDbParameters();
            //dbParameters.AddWithValue("userid", user.UserId);
            //dbParameters.AddWithValue("username", user.UserName);

            //object executeScalar = DbHelper.ExecuteScalar(ConnStringOfETS, insertSql, dbParameters);
            //int a = ParseHelper.ToInt(executeScalar, 0);
            #endregion

            #region 分页demo

            //PageInfo<Ets.Model.UserModel> pinfo = new PageHelper().GetPages<Ets.Model.UserModel>(Config.SuperMan_Read, 1, "1=1", "id", "id", "account", 1, true);
            #endregion

            #region 执行事物demo
            /*
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                //实现调用DAO层方法
                //...
                tran.Complete();
            }
             * */
            #endregion

        }
    }
}
