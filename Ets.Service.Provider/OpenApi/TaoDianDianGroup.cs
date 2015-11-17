﻿using System.Configuration;
using ETS.Const;
using Ets.Dao.Common;
using Ets.Dao.Order;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.Common.TaoBao;
using Ets.Model.ParameterModel.Order;
using ETS.Security;
using Ets.Service.IProvider.OpenApi;
using ETS.Util;
using Top.Api.Request;

namespace Ets.Service.Provider.OpenApi
{
    public class TaoDianDianGroup : IGroupProviderOpenApi
    {
        /// <summary>
        /// 回调万达接口同步订单状态  add by caoheyang 20150326
        /// 茹化肖修改
        /// 2015年8月26日11:09:56
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
            string jsonData, url = null;
            switch (paramodel.fields.status)
            {
                case OrderConst.OrderStatus1:  //完成
                     jsonData = Confirm(paramodel.fields);
                     url = ConfigurationManager.AppSettings["TaoBaoConfirmAsyncStatus"];
                     break;
                case OrderConst.OrderStatus2:  //骑士接单
                     jsonData = Ask(paramodel.fields);
                     url = ConfigurationManager.AppSettings["TaoBaoAskAsyncStatus"];
                     break;
                case OrderConst.OrderStatus4:  //订单已取货 配送中
                     jsonData = PickUp(paramodel.fields);
                     url = ConfigurationManager.AppSettings["TaoBaoPickUpAsyncStatus"];
                     break;
                default:
                    return OrderApiStatusType.Success;
            }
            var reqpar = "data=" + AESApp.AesEncrypt(jsonData);
            string json = HTTPHelper.HttpPost(url, reqpar);
            Log(url, reqpar, json); //记录日志
            TaoBaoResponseBase res = JsonHelper.JsonConvertToObject<TaoBaoResponseBase>(json);
            if (paramodel.fields.status == OrderConst.OrderStatus1&&res.is_success)   //骑士接单 记录坐标
            {
                jsonData = Update(paramodel.fields);
                url = ConfigurationManager.AppSettings["TaoBaoUpdateAsyncStatus"];
                reqpar = "data=" + AESApp.AesEncrypt(jsonData);
                json = HTTPHelper.HttpPost(url, reqpar);
                Log(url, reqpar, json); //记录日志
                res = JsonHelper.JsonConvertToObject<TaoBaoResponseBase>(json);
            }
            return res.is_success ? OrderApiStatusType.Success : OrderApiStatusType.SystemError;
        }


        /// <summary>
        /// 记录第三方调用日志
        /// caoheyang  20151117
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void Log(string url,string request, string response)
        {
            new HttpDao().LogThirdPartyInfo(new HttpModel()
            {
                Url = url,
                Htype = HtypeEnum.ReqType.GetHashCode(),
                RequestBody = request,
                ResponseBody = response,
                ReuqestPlatForm = RequestPlatFormEnum.OpenApiPlat.GetHashCode(),
                ReuqestMethod = "Ets.Service.Provider.OpenApi.TaoDianDianGroup.AsyncStatus",
                Status = 1,
                Remark = "调用淘点点:同步订单状态"
            });
        }




        /// <summary>
        /// 确认接单接口(API)  骑士接单
        /// caoheyang  20151116
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        private string Ask(AsyncStatusPM_OpenApi p)
        {

            WaimaiOrderAckRequest temp = new WaimaiOrderAckRequest
            {
                DeliveryOrderNo = ParseHelper.ToLong(p.OriginalOrderNo)
            };
            return JsonHelper.JsonConvertToString(temp);
        }


        /// <summary>
        /// 更新配送员信息接口（API） 
        /// caoheyang  20151116
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        private string Update(AsyncStatusPM_OpenApi p)
        {
            var order = new OrderOtherDao().GetByOrderNo(p.order_no);
            WaimaiDeliveryUpdateRequest temp = new WaimaiDeliveryUpdateRequest
            {
                DeliveryOrderNo = ParseHelper.ToLong(p.OriginalOrderNo),
                DelivererPhone=p.ClienterPhoneNo,
                DelivererName=p.ClienterTrueName,
                Lng = order.GrabLongitude.ToString(),
                Lat = order.GrabLatitude.ToString()
            };
            return JsonHelper.JsonConvertToString(temp);
        }

        /// <summary>
        /// 取件（API）  配送中
        /// caoheyang  20151116
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string PickUp(AsyncStatusPM_OpenApi p)
        {
            var order = new OrderOtherDao().GetByOrderNo(p.order_no);
            WaimaiDeliveryPickupRequest temp = new WaimaiDeliveryPickupRequest
            {
                DeliveryOrderNo = ParseHelper.ToLong(p.OriginalOrderNo),
                Lng = order.TakeLongitude.ToString(),
                Lat = order.TakeLatitude.ToString()
            };
            return JsonHelper.JsonConvertToString(temp);
        }

        /// <summary>
        /// 妥投（API） 订单完成
        /// caoheyang  20151116
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string Confirm(AsyncStatusPM_OpenApi p)
        {
            var order = new OrderOtherDao().GetByOrderNo(p.order_no);
            WaimaiDeliveryConfirmRequest temp = new WaimaiDeliveryConfirmRequest
            {
                DeliveryOrderNo = ParseHelper.ToLong(p.OriginalOrderNo),
                Lng = order.CompleteLongitude.ToString(),
                Lat = order.CompleteLatitude.ToString()
            };
            return JsonHelper.JsonConvertToString(temp);
        }

        /// <summary>
        /// 新增商铺时根据集团id为店铺设置外送费，结算比例等财务相关信息 add by caoheyang 20150417
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public CreatePM_OpenApi SetCommissonInfo(CreatePM_OpenApi paramodel)
        {
            return paramodel;
        }
    }
}
