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
using System.Web;

namespace OpenApi.Controllers
{
    /// <summary>
    /// 对外公开接口  订单相关功能  add by caoheyang 20150316
    /// </summary>
 
    //[RoutePrefix("api/order/")]
    public class OrderController : ApiController
    {
        // POSR: Order GetStatus
        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void GetStatus()
        {
            IOrderProvider orderProvider = new OrderProvider();
            int status = orderProvider.GetStatus(HttpContext.Current.Request.Form["order_no"]);  //todo  缺少非空验证
        }

        // POSR: Order Create
        /// <summary>
        /// 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void Create(Ets.Model.ParameterModel.Order.CreatePM_OpenApi paramodel)
        {
            IOrderProvider orderProvider = new OrderProvider(); 
        }

    }
}