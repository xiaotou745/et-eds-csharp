using Ets.Model.DataModel.Authority;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Enums;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Account
{
    public class AccountDao : DaoBase
    {
        /// <summary>
        /// 用户登录
        /// danny-20150324
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserLoginResults ValidateUser(string userName, string password)
        {

            var user = GetAccountInfoByLoginName(userName);
            if (user == null)
            {
                return UserLoginResults.UserNotExist;
            }
            if (user.Password != password)
            {
                return UserLoginResults.WrongPassword;
            }
            if (user.AccountType == (int)AccountType.AdminUser)
            {
                //var admin = db.account.Single(i => i.Id == user.Id);
                if (user.Status == 0)
                {
                    return UserLoginResults.AccountClosed;
                }
            }
            return UserLoginResults.Successful;
        }
        /// <summary>
        /// 根据用户名获取用户信息
        /// danny-20150324
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private account GetAccountInfoByLoginName(string name)
        {
            //获取登陆用户时 判断 status = 1   wc改
            string sql = @" SELECT TOP 1 [Id]
                                          ,[Password]
                                          ,[UserName]
                                          ,[LoginName]
                                          ,[Status]
                                          ,[AccountType]
                                          ,[FADateTime]
                                          ,[FAUser]
                                          ,[LCDateTime]
                                          ,[LCUser]
                                          ,[GroupId]
                                          ,[RoleId]
                                      FROM [account] with(nolock) where LoginName=@LoginName AND [Status] = 1";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("LoginName", name);
            var dr = DbHelper.ExecuteReader(SuperMan_Read, sql, dbParameters);
            if (dr.Read())
            {
                return new account
                {
                    Id = ParseHelper.ToInt(dr["Id"]),
                    Password = ParseHelper.ToString(dr["Password"]),
                    UserName = ParseHelper.ToString(dr["UserName"]),
                    LoginName = ParseHelper.ToString(dr["LoginName"]),
                    Status = ParseHelper.ToInt(dr["Status"]),
                    AccountType = ParseHelper.ToInt(dr["AccountType"]),
                    FADateTime = ParseHelper.ToDatetime(dr["FADateTime"]),
                    FAUser = ParseHelper.ToString(dr["FAUser"]),
                    LCDateTime = ParseHelper.ToDatetime(dr["LCDateTime"]),
                    LCUser = ParseHelper.ToString(dr["LCUser"]),
                    GroupId = ParseHelper.ToInt(dr["GroupId"]),
                    RoleId = ParseHelper.ToInt(dr["RoleId"])
                };
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountId"></param>
        public IList<AuthorityMenuModel> GetAuth(int AccountId)
        {
            string sql = @"
SELECT AccoutId,MenuId,ParId FROM dbo.AuthorityAccountMenuSet  aam (nolock)
JOIN AuthorityMenuClass amc(nolock) ON aam.MenuId = amc.Id
WHERE aam.AccoutId=@AccountId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("AccountId", DbType.Int32, 4).Value = AccountId;
            DataTable dt= DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt==null || dt.Rows.Count<=0)
            {
                return null;
            }
            return MapRows<AuthorityMenuModel>(dt);
        }

    }
}
