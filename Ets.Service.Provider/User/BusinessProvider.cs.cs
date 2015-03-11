using Ets.Dao.User;
using Ets.Service.IProvider;
using Ets.Service.IProvider.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.User
{

    /// <summary>
    /// 商户业务逻辑接口实现类  add by caoheyang 20150311
    /// </summary>
    public class BusinessProvider : IBusinessProvider
    {
        /// <summary>
        /// app端商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual IList<int> GetOrdersApp()
        {
            return null;
        }
    }
}