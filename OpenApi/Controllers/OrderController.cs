using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.IProvider.Clienter;
using Ets.Service.Provider.Clienter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OpenApi.Controllers
{
    /// <summary>
    /// 对外公开接口  订单相关功能  add by caoheyang 20150316
    /// </summary>
 
    //[RoutePrefix("api/order/")]
    public class OrderController : ApiController
    {
        // GET: Order
        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public void GetStatus()
        {
            IOrderProvider orderProvider = new OrderProvider();
            int? status = orderProvider.GetStatus("23150313162254057");

        }

    }
}