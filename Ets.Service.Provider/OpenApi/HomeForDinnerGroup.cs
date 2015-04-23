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
    /// 回家吃饭对接回调业务
    /// 徐鹏程
    /// 2015-04-23
    /// </summary>
    public class HomeForDinnerGroup:IGroupProviderOpenApi
    {
        public string app_key = ConfigSettings.Instance.HomeForDinnerAppkey;
        public string app_secret = ConfigSettings.Instance.HomeForDinnerAppsecret;
        /// <summary>
        /// 回调回家吃饭同步订单状态
        /// 徐鹏程
        /// 2015-04-23
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {

            int status = 0; //第三方订单状态物流状态，1代表已发货，2代表已签收
            switch (paramodel.fields.status)
            {
                case OrderConst.OrderStatus1:
                    status = 2; //订单已完成
                    break;
                case OrderConst.OrderStatus2:
                    status = 1; //订单已接单
                    break;
                case OrderConst.OrderStatus3:
                    status = 3; //订单已取消
                    break;
                default:
                    return OrderApiStatusType.OtherError;
            }
            string ts = DateTime.Now.ToString();
            string url = ConfigurationManager.AppSettings["HomeForDinnerAsyncStatus"];
            if (url == null)
                return OrderApiStatusType.SystemError;
            ///order_id	string	Y	订单ID ，根据订单ID改变对应的订单物流状态，一个订单只能修改一次，修改过再修改会报错。
            ///status	int	Y	物流状态，1代表已发货，2代表已签收
            ///send_phone	string	Y/N	配送员电话，物流状态传参是（ststus=1）的时候，配送员电话必须写，如果为（ststus=2）的时候可以不写。
            ///send_name	string	Y/N	配送员姓名，物流状态传参是（ststus=1）的时候，配送员姓名必须写，如果为（ststus=2）的时候可以不写。
            string json = HTTPHelper.HttpPost(url, "app_key=" + app_key + "&sign=" + GetSign(ts) + "&updatetime=" + ts + "&order_id=" + paramodel.fields.OriginalOrderNo + "&status=" + status + "&dm_name=" + paramodel.fields.ClienterTrueName
                + "&dm_mobile=" + paramodel.fields.ClienterPhoneNo);
            
            if (string.IsNullOrWhiteSpace(json))
                return OrderApiStatusType.ParaError;
            else if (json == "null")
                return OrderApiStatusType.SystemError;
            JObject jobject = JObject.Parse(json);
            int result = jobject.Value<int>("status"); //接口调用状态 区分大小写
            return result == 1 ? OrderApiStatusType.Success : OrderApiStatusType.SystemError;
        }

        /// <summary>
        /// 获取当前集团请求时的sign信息
        /// 徐鹏程
        /// 2015-04-23
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        private string GetSign(string time)
        {
            string signStr = app_secret + "app_key" + app_key + "updatetime" + time + app_secret;
            return ETS.Security.MD5.Encrypt(signStr);
        }
        /// <summary>
        /// 新增商铺时根据集团id为店铺设置外送费，结算比例等财务相关信息
        /// 徐鹏程
        /// 2015-04-23
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public CreatePM_OpenApi SetCommissonInfo(CreatePM_OpenApi paramodel)
        {
            paramodel.store_info.delivery_fee = 5;//全时目前外送费统一5
            paramodel.store_info.businesscommission = 0;//万达目前结算比例统一0
            return paramodel;
        }
    }
}
