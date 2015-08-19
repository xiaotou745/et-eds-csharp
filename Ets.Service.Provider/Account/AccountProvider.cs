using Ets.Dao.Account;
using Ets.Model.DataModel.Authority;
using Ets.Service.IProvider.Account;
using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Util;

namespace Ets.Service.Provider.Account
{
    public class AccountProvider:IAccountProvider
    {
        readonly AccountDao accountDao = new AccountDao();
        /// <summary>
        /// 用户登录
        /// danny-20150324
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserLoginResults ValidateUser(string userName, string password)
        {
            return accountDao.ValidateUser(userName,password);
        }

        public IList<AuthorityMenuModel> GetAuth(int AccountId)
        {
            return accountDao.GetAuth(AccountId);
        }

        /// <summary>
        /// 验证旧密码
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="oldpwd"></param>
        /// <returns></returns>
        public bool ChcekPassword(int AccountId, string oldpwd)
        {
            oldpwd = MD5Helper.MD5(oldpwd);
            return accountDao.ChcekPassword(AccountId, oldpwd);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="newpwd"></param>
        /// <returns></returns>
        public bool UpdatePassword(int AccountId, string newpwd)
        {
            newpwd = MD5Helper.MD5(newpwd);
            return accountDao.UpdatePassword(AccountId, newpwd);
        }

        /// <summary>
        /// 密码是否过期
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public int PasswordTime(int AccountId)
        {
            return accountDao.PasswordTime(AccountId);
        }
    }
}
