using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.User
{
    /// <summary>
    /// 商户业务逻辑接口 add by caoheyang 20150311
    /// </summary>
    public interface IBusinessProvider
    {
        /// <summary>
        /// 商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <returns></returns>
        IList<int> GetOrdersApp();
    }
}
