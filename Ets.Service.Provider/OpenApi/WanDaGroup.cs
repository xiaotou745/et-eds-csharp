using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.OpenApi;
using ETS.Const;
using ETS.Enums;
using ETS.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.OpenApi
{

    /// <summary>
    /// 万达集团对接回调业务 add by caoheyang 20150326
    /// </summary>
    public class WanDaGroup : IGroupProviderOpenApi
    {
        /// <summary>
        /// 回调万达接口同步订单状态  add by caoheyang 20150326
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
            string status = null; //第三方订单状态
            string statusDesc = null; //事件状态描述
            switch (paramodel.fields.status)
            {
                case OrderConst.OrderStatus1:
                    status = "complete";
                    statusDesc = "订单已完成";
                    break;
                case OrderConst.OrderStatus2:
                    status = "sending";
                    statusDesc = "谁谁谁在配送，联系电话..";
                    break;
                case OrderConst.OrderStatus3:
                    status = "cancel";
                    statusDesc = "XXX商户取消该订单";
                    break;
                default:
                    break;
            }
            var model = new
            {
                app_key = paramodel.app_key, // app_key
                sign = paramodel.app_key, // sign
                method = "POST", //请求方式 
                ts = TimeHelper.GetTimeStamp(), //时间戳
                orderId = paramodel.fields.order_no, //订单 ID
                staus = status,  //物流变更状态
                statusDesc = statusDesc,  //事件状态描述
                syncTime = DateTime.Now, //同步时间
                operatorId = 0, //操作人ID, 来源系统的账号id
                @operator = "E代送系统"// 操作人姓名
            };
            string url = ConfigurationManager.AppSettings["WanDaAsyncStatus"];
            if (url == null)
                return OrderApiStatusType.SystemError;
            string json = new HttpClient().PostAsJsonAsync(url, model).Result.Content.ReadAsStringAsync().Result;
            JObject jobject = JObject.Parse(json);
            int x = jobject.Value<int>("status"); //接口调用状态 区分大小写
            return x == 200 ? OrderApiStatusType.Success : OrderApiStatusType.SystemError;
        }
    }
}
