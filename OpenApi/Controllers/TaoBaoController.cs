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
using ETS.Util;
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
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [HttpPost]
        public ResultModel<long> OrderDispatch(ParamModel pm)
        {
            string json = AESApp.AesDecrypt(pm.data.Replace(' ', '+')/*TODO 暂时用Replace*/);
            OrderDispatch p = ParseHelper.Deserialize<OrderDispatch>(json);
            p.itemsList = ParseHelper.Deserialize<List<Commodity>>(p.items);
            if (p.store_name.IndexOf("代送") > 0)
            {
                return ResultModel<long>.Conclude(taoDianDianGroup.TaoBaoPushOrder(p), p.delivery_order_no);
            }
            return ResultModel<long>.Conclude(TaoBaoPushOrder.Error);

        }

        //[HttpPost]
        //public ResultModel<object> OrderDispatch(OrderDispatch p)
        //{
        //    try
        //    {
        //        p.itemsList = ParseHelper.Deserialize<List<Commodity>>(p.items);
        //    }
        //    catch (Exception err)
        //    {
        //        string str = err.Message;
        //    }
        //    return ResultModel<object>.Conclude(taoDianDianGroup.TaoBaoPushOrder(p));
        //}
    }
}
