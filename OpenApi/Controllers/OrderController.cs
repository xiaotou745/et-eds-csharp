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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using ETS.Util;
using System.Configuration;
using ETS.Const;
using Ets.Service.Provider.OpenApi;
using Ets.Model.DataModel.Order;
using Ets.Service.IProvider.OpenApi;

namespace OpenApi.Controllers
{
    /// <summary>
    /// 对外公开接口  订单相关功能  add by caoheyang 20150316
    /// </summary>
    public class OrderController : ApiController
    {
        // POST: Order GetStatus    paramodel 固定 必须是 paramodel
        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi] //sign验证过滤器 设计参数验证，sign验证 add by caoheyang 20150316
        [OpenApiActionError] //异常过滤器 add by caoheyang 一旦发生异常，客户端返回系统内部错误提示
        public ResultModel<object> GetStatus(ParaModel<GetStatusPM_OpenApi> paramodel)
        {
            int status = new OrderProvider().GetStatus(paramodel.fields.order_no, paramodel.group);
            return status < 0 ?
            ResultModel<object>.Conclude(OrderApiStatusType.ParaError) :    //订单不存在返回参数错误提示
            ResultModel<object>.Conclude(OrderApiStatusType.Success, new { order_status = status });
        }

        // POST: Order Create   paramodel 固定 必须是 paramodel  
        /// <summary>
        /// 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi]
        [OpenApiActionError] 
        public ResultModel<object> Create(ParaModel<CreatePM_OpenApi> paramodel)
        {
            paramodel.fields.store_info.group = paramodel.group;  //设置集团信息到具体的门店上  在dao层会用到
            paramodel.fields.orderfrom = paramodel.group; //设置订单来源 TODO 目前与订单一致
            IOrderProvider orderProvider = new OrderProvider();
            return orderProvider.Create(paramodel.fields);
        }

        // POST: Order OrderDetail   paramodel 固定 必须是 paramodel  
        /// <summary>
        /// 查看订单详情接口  add by caoheyang 20150325
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi]
        [OpenApiActionError]
        public ResultModel<object> OrderDetail(ParaModel<OrderDetailPM_OpenApi> paramodel)
        {
            paramodel.fields.GroupId = paramodel.group;  //设置集团信息到具体的门店上  在dao层会用到
            IOrderProvider orderProvider = new OrderProvider();
            return orderProvider.OrderDetail(paramodel.fields);
        }

        // POST: Order AsyncStatus   paramodel 固定 必须是 paramodel  
        /// <summary>
        /// 第三方订单状态同步   add by caoheyang 20150326  
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi]
        [OpenApiActionError]
        public ResultModel<object> AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
            IGroupProviderOpenApi groupProvider = OpenApiGroupFactory.Create(paramodel.group);
            if (groupProvider == null)
                ResultModel<object>.Conclude(OrderApiStatusType.Success);  //无集团信息，不需要同步返回成功，实际应该不会该情况
            OrderApiStatusType statusType = groupProvider.AsyncStatus(paramodel);
            return ResultModel<object>.Conclude(statusType);
        }


        // POST: Order ChangeStatus   paramodel 固定 必须是 paramodel  
        /// <summary>
        /// 第三方更新E代送订单状态   add by caoheyang 20150421  
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi]
        [OpenApiActionError]
        public ResultModel<object> ChangeStatus(ParaModel<ChangeStatusPM_OpenApi> paramodel)
        {
            paramodel.fields.groupid = paramodel.group;
            return new OrderProvider().UpdateOrderStatus_Other(paramodel.fields);
        }
        /// <summary>
        /// 获取订单的日志信息
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        //[HttpPost]
        //[SignOpenApi]
        //[OpenApiActionError]
        //public ResultModel<object> GetOrderRecords(ParaModel<GetStatusPM_OpenApi> paramodel)
        //{
        //    int orderRecords =  new OrderProvider().GetStatus(paramodel.fields.order_no, paramodel.group);

        //    return 
        //}
    }
}