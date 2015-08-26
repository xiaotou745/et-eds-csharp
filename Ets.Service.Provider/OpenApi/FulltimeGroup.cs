using Ets.Dao.Common;
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
    /// 全时对接回调业务 add by caoheyang 20150326
    /// </summary>
    public class FulltimeGroup : IGroupProviderOpenApi
    {
        public  string app_key = ConfigSettings.Instance.FulltimeAppkey;
        public const string v = "1.0";
        public  string app_secret = ConfigSettings.Instance.FulltimeAppsecret;
        /// <summary>
        /// 回调万达接口同步订单状态  add by caoheyang 20150326
        /// 茹化肖修改
        /// 2015年8月26日11:09:56
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
                    return OrderApiStatusType.Success;
            }
            string ts = DateTime.Now.ToString();
            string url = ConfigurationManager.AppSettings["FulltimeAsyncStatus"];
            if (url == null)
                return OrderApiStatusType.SystemError;
            ///order_id	int	Y	订单ID ，根据订单ID改变对应的订单物流状态，一个订单只能修改一次，修改过再修改会报错。
            /// status	int	Y	物流状态，1代表已发货，2代表已签收
            ///send_phone	string	Y/N	配送员电话，物流状态传参是（ststus=1）的时候，配送员电话必须写，如果为（ststus=2）的时候可以不写。
            ///send_name	string	Y/N	配送员姓名，物流状态传参是（ststus=1）的时候，配送员姓名必须写，如果为（ststus=2）的时候可以不写。
            var reqpar = "app_key=" + app_key + "&sign=" + GetSign(ts) + "&timestamp=" + ts + "&order_id=" +
                         paramodel.fields.OriginalOrderNo + "&status=" + status + "&v=" + v + "&send_phone=" +
                         paramodel.fields.ClienterPhoneNo
                         + "&send_name=" + paramodel.fields.ClienterTrueName;
            string json = HTTPHelper.HttpPost(url, reqpar);
            HttpModel httpModel = new HttpModel()
            {
                Url = url,
                Htype = HtypeEnum.ReqType.GetHashCode(),
                RequestBody = reqpar,
                ResponseBody = json,
                ReuqestPlatForm = RequestPlatFormEnum.OpenApiPlat.GetHashCode(),
                ReuqestMethod = "Ets.Service.Provider.OpenApi.FulltimeGroup.AsyncStatus",
                Status = 1,
                Remark = "调用全时:同步订单状态"
            };
            new HttpDao().LogThirdPartyInfo(httpModel);
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

        /// <summary>
        /// 新增商铺时根据集团id为店铺设置外送费，结算比例等财务相关信息 add by caoheyang 20150417
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public CreatePM_OpenApi SetCommissonInfo(CreatePM_OpenApi paramodel)
        {
            paramodel.store_info.delivery_fee = 0;//全时目前外送费统一0
            paramodel.store_info.businesscommission = 9;//全时目前结算比例统一9
            return paramodel;
        }

    }
}
