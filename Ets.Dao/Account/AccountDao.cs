﻿using Ets.Model.DataModel.Authority;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Enums;
using ETS.Util;
using System;
using System.Collections.Generic;
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
                if (user.Status==0)
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
                                      FROM [account] with(nolock) where LoginName=@LoginName ";
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

    }
}