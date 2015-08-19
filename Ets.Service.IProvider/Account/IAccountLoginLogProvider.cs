using Ets.Model.DataModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Account
{
    public interface IAccountLoginLogProvider
    {
        /// <summary>
        /// 登录日志
        /// 窦海超
        /// 2015年8月18日 12:21:49
        /// </summary>
        /// <param name="model"></param>
        void Insert(AccountLoginLogModel model);
    }
}
