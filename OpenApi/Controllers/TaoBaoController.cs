using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Common;
using Ets.Model.ParameterModel.Order;
using ETS.Security;
using Ets.Service.Provider.Order;
using Newtonsoft.Json.Linq;
using OpenApi;
using Ets.Model.Common.TaoBao;
using Ets.Service.IProvider.OpenApi;
using Ets.Service.Provider.OpenApi;
namespace OpenApi.Controllers
{

    /// <summary>
    /// 淘宝处理器  
    /// caoheyang 2015118
    /// </summary>
    [OpenApiActionError] 
    public class TaoBaoController : ApiController
    {

        TaoDianDianGroup taoDianDianGroup = new TaoDianDianGroup();
        /// <summary>
        /// 淘宝取消订单
        /// caoheyang 2015118
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [HttpPost]
        public ResultModel<object> CancelOrder(ParamModel p)
        {
            string json = AESApp.AesDecrypt(p.data);
            JObject jobject = JObject.Parse(json);
            string delivery_order_no = jobject.Value<string>("delivery_order_no"); //接口调用状态 区分大小写
            return ResultModel<object>.Conclude(new OrderProvider().TaoBaoCancelOrder(delivery_order_no));
        }

        /// <summary>
        /// 淘宝发布订单
        /// caoheyang 2015118
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [HttpPost]
        public ResultModel<object> OrderDispatch(OrderDispatch p)
        {
            
            //string json = AESApp.AesDecrypt(p.data);
            //JObject jobject = JObject.Parse(json);
            //string delivery_order_no = jobject.Value<string>("delivery_order_no"); //接口调用状态 区分大小写
            //return ResultModel<object>.Conclude(new OrderProvider().TaoBaoCancelOrder(delivery_order_no));
            taoDianDianGroup.TaoBaoPushOrder(p);
            //调用确认订单接口
            return ResultModel<object>.Conclude(null);
        }
    }
}
