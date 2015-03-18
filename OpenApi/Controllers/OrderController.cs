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
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.Common;
using Ets.Model.DomainModel.Group;
using ETS.Security;


namespace OpenApi.Controllers
{
    /// <summary>
    /// 对外公开接口  订单相关功能  add by caoheyang 20150316
    /// </summary>

    //[RoutePrefix("api/order/")]
    public class OrderController : ApiController
    {
        // POSR: Order GetStatus    paramodel 固定 必须是 paramodel
        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi]
        public ResultModel<dynamic> GetStatus(ParaModel<GetStatusPM_OpenApi> paramodel)
        {
            try
            {
                return (new OrderProvider().GetStatus(paramodel.fields.order_no) < 0) ?
                ResultModel<dynamic>.Conclude(OrderApiStatusType.ParaError) :    //订单不存在返回参数错误提示
                ResultModel<dynamic>.Conclude(OrderApiStatusType.Success, new
                {
                    status = new OrderProvider().GetStatus(paramodel.fields.order_no)
                });
            }
            catch
            {
                return ResultModel<dynamic>.Conclude(OrderApiStatusType.SystemError);       //返回系统错误提示
            }
        }
        // POST: Order Create  paramodel 固定 必须是 paramodel  
        /// <summary>
        /// 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi]
        public ResultModel<dynamic> Create(ParaModel<CreatePM_OpenApi> paramodel)
        {
            try
            {
                paramodel.fields.store_info.group = paramodel.group;  //设置集团信息到具体的门店上  在dao层会用到 
                IOrderProvider orderProvider = new OrderProvider();
                string orderNo = orderProvider.Create(paramodel.fields);
                return string.IsNullOrWhiteSpace(orderNo) ? ResultModel<dynamic>.Conclude(OrderApiStatusType.SystemError) :
                    ResultModel<dynamic>.Conclude(OrderApiStatusType.Success, new { order_no = orderNo });
            }
            catch
            {
                return ResultModel<dynamic>.Conclude(OrderApiStatusType.SystemError);       //返回系统错误提示
            }
        }

    }

}