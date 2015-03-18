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
using System.ComponentModel.DataAnnotations;
using Ets.Model.Common;
using ETS.Enums;
using Ets.Model.ParameterModel.Order;

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
        public ResultModel<dynamic> GetStatus()
        {
            try
            {
                IOrderProvider orderProvider = new OrderProvider();
                return HttpContext.Current.Request.Form["order_no"] == null ?
                     ResultModel<dynamic>.Conclude(OrderApiStatusType.ParaError) :
                     ResultModel<dynamic>.Conclude(OrderApiStatusType.Success, new
                     {
                         status = orderProvider.GetStatus(HttpContext.Current.Request.Form["order_no"])
                     });
            }
            catch {
                return ResultModel<dynamic>.Conclude(OrderApiStatusType.SystemError);       //返回系统错误提示
            }
        }

        // POST: Order Create
        /// <summary>
        /// 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultModel<dynamic> Create(ParaModel<CreatePM_OpenApi> paramodel)
        {
            try
            {
                if (base.ModelState.Count > 0 || paramodel == null)
                    return ResultModel<dynamic>.Conclude(OrderApiStatusType.ParaError);       //返回参数错误提示
                else
                {
                    IOrderProvider orderProvider = new OrderProvider();
                    string orderNo = orderProvider.Create(paramodel.fields);
                    return string.IsNullOrWhiteSpace(orderNo) ? ResultModel<dynamic>.Conclude(OrderApiStatusType.SystemError) :
                        ResultModel<dynamic>.Conclude(OrderApiStatusType.Success, new { order_no = orderNo });
                }
            }
            catch
            {
                return ResultModel<dynamic>.Conclude(OrderApiStatusType.SystemError);       //返回系统错误提示
            }
        }

    }
}