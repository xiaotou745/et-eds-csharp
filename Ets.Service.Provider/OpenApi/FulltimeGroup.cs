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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.OpenApi
{

    /// <summary>
    /// 全时对接回调业务 add by caoheyang 20150326
    /// </summary>
    public class FulltimeGroup : IGroupProviderOpenApi
    {
        public const string app_key = "fulltime";
        public const string v = "1.0";
        public const string app_secret = "E7A1C84E8F47404FB8C1CDC1FA48A912";
        /// <summary>
        /// 回调万达接口同步订单状态  add by caoheyang 20150326
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
            int status = 0; //第三方订单状态物流状态，1代表已发货，2代表已签收
            switch (paramodel.fields.status)
            {
                case OrderConst.OrderStatus1:
                    status = 2; //已签收
                    break;
                case OrderConst.OrderStatus2:
                    status = 1; //已发货
                    break;
                default:
                    break;
            }
            string ts = DateTime.Now.ToString();
            string url = ConfigurationManager.AppSettings["FulltimeAsyncStatus"];
            if (url == null)
                return OrderApiStatusType.SystemError;
            string json = HTTPHelper.HttpPost(url, "app_key=" + app_key + "&sign=" + GetSign(ts) + "&timestamp=" + ts + "&order_id=" + paramodel.fields.OriginalOrderNo + "&status=" + status+"&v="+v);
            if (string.IsNullOrWhiteSpace(json))
                return OrderApiStatusType.ParaError;
            else if (json == "null")
                return OrderApiStatusType.SystemError;
            JObject jobject = JObject.Parse(json);
            int x = jobject.Value<int>("status"); //接口调用状态 区分大小写
            return x == 1 ? OrderApiStatusType.Success : OrderApiStatusType.SystemError;
        }

        /// <summary>
        /// 获取当前集团请求时的sign信息  add by caoheyang 20150407
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        private string GetSign(string time)
        {
            string signStr = app_secret + "app_key" + app_key + "timestamp" + time + "v" + v + app_secret;
            return ETS.Security.MD5.Encrypt(signStr);
        }


    }
}
