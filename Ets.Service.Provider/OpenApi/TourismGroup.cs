using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.OpenApi;
using Ets.Service.Provider.OpenApi;
using ETS.Const;
using ETS.Enums;
using ETS.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.OpenApi
{
    /// <summary>
    /// 首旅对接回调业务 caoheyang  20150527
    /// </summary>
    public class TourismGroup : IGroupProviderOpenApi
    {
        public string app_key = ConfigSettings.Instance.TourismAppkey;
        /// <summary>
        /// 首旅对接同步订单状态 caoheyang  20150527
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
            int status = 0; //第三方订单状态物流状态，1代表已发货，2代表已签收
            string message = "";
            switch (paramodel.fields.status)
            {
                case OrderConst.OrderStatus1:
                    status = 1; //订单已完成(已送达)
                    message = "订单已完成";
                    break;
                case OrderConst.OrderStatus2:
                    status = 2; //订单已接单(已抢单)
                    message = "订单已接单";
                    break;
                case OrderConst.OrderStatus3:
                    status = 4; //订单已取消
                    message = "订单已取消";
                    break;
                default:
                    return OrderApiStatusType.OtherError;
            }

            var paras = new
            {
                method = "takenotify",
                app_key = app_key,
                fields = new object[] {  new
                {
                    order_id = paramodel.fields.OriginalOrderNo,
                    order_status = status,
                    message = message
                }}
            };
            string url = ConfigSettings.Instance.TourismAsyncStatusUrl;
            if (url == null)
            {
                return OrderApiStatusType.SystemError;
            }
            string jsonss = JsonHelper.ToJson(paras);

            string json = new HttpClient().PostAsJsonAsync(url, paras).Result.Content.ReadAsStringAsync().Result;
            if (string.IsNullOrWhiteSpace(json))
            {
                return OrderApiStatusType.ParaError;
            }
            JObject jobject = JObject.Parse(json);
            int result = jobject.Value<int>("Status"); //接口调用状态 区分大小写
            return result == 0 ? OrderApiStatusType.Success : OrderApiStatusType.SystemError;
        }


        /// <summary>
        /// 新增商铺时根据集团id为店铺设置外送费，结算比例等财务相关信息  caoheyang  20150527
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public CreatePM_OpenApi SetCommissonInfo(CreatePM_OpenApi paramodel)
        {
            //paramodel.store_info.delivery_fee = 10;
            //paramodel.store_info.businesscommission = 0;
            return paramodel;
        }
    }
}
