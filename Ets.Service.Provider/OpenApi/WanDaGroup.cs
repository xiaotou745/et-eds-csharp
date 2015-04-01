﻿using Ets.Model.Common;
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
    /// 万达集团对接回调业务 add by caoheyang 20150326
    /// </summary>
    public class WanDaGroup : IGroupProviderOpenApi
    {
        public const string app_key = "6e61eb575d2b22223551af81b2812ec2";
        public const string app_secret = "9ed652ad50274a24f4b648f0e7dad167";
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
                    status = "agree";
                    statusDesc = string.Format("{0}已确认可配送", paramodel.fields.ClienterTrueName);
                    break;
                case OrderConst.OrderStatus3:
                    status = "refused";
                    statusDesc = string.Format("{0}拒绝配送该订单", paramodel.fields.BusinessName);
                    break;
                default:
                    break;
            }
            string ts = TimeHelper.GetTimeStamp(false);
            string url = ConfigurationManager.AppSettings["WanDaAsyncStatus"];
            if (url == null)
                return OrderApiStatusType.SystemError;
            string  json= HTTPHelper.HttpPost(url, "app_key=" + app_key + "&sign=" + GetSign(ts) + "&method=POST&ts=" + ts + "&orderId=" + paramodel.fields.OriginalOrderNo + "&status=" + status + "&statusDesc=" + statusDesc +
                "&syncTime=" + ts + "&operatorId=9999&operator=E代送系统&logisticsNo=" +
                 paramodel.fields.order_no + "&action=takeoutsync");
            if (string.IsNullOrWhiteSpace(json))
                return OrderApiStatusType.ParaError;
            else if (json == "null")
                return OrderApiStatusType.SystemError;
            JObject jobject = JObject.Parse(json);
            int x = jobject.Value<int>("status"); //接口调用状态 区分大小写
            return x == 200 ? OrderApiStatusType.Success : OrderApiStatusType.SystemError;
        }

        /// <summary>
        /// 获取当前集团请求时的sign信息  add by caoheyang 20150330
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        private string GetSign(string time)
        {
            //签名信息
            string method = "POST";
            List<string> @params = new List<string>() { "app_key="+ app_key,
            "app_secret="+ app_secret,"method="+ method,"ts="+ time};
            @params.Sort();
            string signStr = string.Join("&", @params);
            return ETS.Security.MD5.Encrypt(signStr);
        }


    }
}
