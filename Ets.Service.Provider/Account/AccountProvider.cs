using Ets.Dao.Account;
using Ets.Service.IProvider.Account;
using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Account
{
    public class AccountProvider:IAccountProvider
    {
        AccountDao accountDao=new AccountDao();
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
    }
}
