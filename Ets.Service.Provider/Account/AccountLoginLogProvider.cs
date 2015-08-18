using Ets.Dao.Account;
using Ets.Service.IProvider.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Account
{
    public class AccountLoginLogProvider : IAccountLoginLogProvider
    {
        /// <summary>
        /// 登录日志
        /// 窦海超
        /// 2015年8月18日 12:21:49
        /// </summary>
        /// <param name="model"></param>
        public void Insert(Model.DataModel.Account.AccountLoginLogModel model)
        {
            AccountLoginLogDao dao = new AccountLoginLogDao();
            dao.Insert(model);
        }
    }
}
