using ETS;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.User
{
    public class UserDao : DaoBase
    {
        public virtual void RegisterToSql(Ets.Model.UserModel user)
        {
            //DbHelper.ExecuteNonQuery();
            //const string insertSql = @"insert into user(............) values (@userid, @username) select @@identity";

            //IDbParameters dbParameters = DbHelper.CreateDbParameters();
            //dbParameters.AddWithValue("userid", user.UserId);
            //dbParameters.AddWithValue("username", user.UserName);

            //object executeScalar = DbHelper.ExecuteScalar(ConnStringOfETS, insertSql, dbParameters);
            //int a = ParseHelper.ToInt(executeScalar, 0);
            //DbHelper.QueryWithRowMapper
            PageInfo pinfo = new PageHelper().GetPages(Config.SuperMan_Read, 1, "1=1", "id", "id", "account", 1, true);

        }
    }
}
