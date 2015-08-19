using Ets.Model.DataModel.Authority;
using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Account
{
    public interface IAccountProvider
    {
        /// <summary>
        /// 用户登录
        /// danny-20150324
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserLoginResults ValidateUser(string userName, string password);

        IList<AuthorityMenuModel> GetAuth(int AccountId);
        bool ChcekPassword(int AccountId,string oldpwd);

        bool UpdatePassword(int AccountId, string newpwd);

        int PasswordTime(int AccountId);
    }
}
