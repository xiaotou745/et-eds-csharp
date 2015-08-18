using Ets.Dao.Account;
using Ets.Model.DataModel.Account;
using Ets.Service.IProvider.Account;
using ETS.Util;
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
            model.Browser = System.Web.HttpContext.Current.Request.UserAgent;
            model.Ip=DnsUtils.HostIp;
            model.Mac = DnsUtils.GetMacString;
            AccountLoginLogDao dao = new AccountLoginLogDao();
            dao.Insert(model);
        }
    }
}
