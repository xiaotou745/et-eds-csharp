using System.Configuration;
using ETS.Const;
using Ets.Dao.Common;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.Common.TaoBao;
using Ets.Model.ParameterModel.Order;
using ETS.Security;
using Ets.Service.IProvider.OpenApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Util;
using Newtonsoft.Json.Linq;
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
            if (url == null)
            {
                return OrderApiStatusType.SystemError;
            }
            var reqpar = "data=" + AESApp.AesEncrypt(jsonData);
            string json = HTTPHelper.HttpPost(url, reqpar);
            new HttpDao().LogThirdPartyInfo(new HttpModel()
            {
                Url = url,
                Htype = HtypeEnum.ReqType.GetHashCode(),
                RequestBody = reqpar,
                ResponseBody = json,
                ReuqestPlatForm = RequestPlatFormEnum.OpenApiPlat.GetHashCode(),
                ReuqestMethod = "Ets.Service.Provider.OpenApi.TaoDianDianGroup.AsyncStatus",
                Status = 1,
                Remark = "调用全时:同步订单状态"
            });
            if (string.IsNullOrWhiteSpace(json))
            {
                return OrderApiStatusType.ParaError;
            }
            TaoBaoResponseBase res = JsonHelper.JsonConvertToObject<TaoBaoResponseBase>(json);
            return res.is_success ? OrderApiStatusType.Success : OrderApiStatusType.SystemError;
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
        /// 取件（API）  配送中
        /// caoheyang  20151116
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string PickUp(AsyncStatusPM_OpenApi p)
        {
            WaimaiDeliveryPickupRequest temp = new WaimaiDeliveryPickupRequest
            {
                DeliveryOrderNo = ParseHelper.ToLong(p.OriginalOrderNo),
                Lng = p.Longitude.ToString(),
                Lat = p.Latitude.ToString()
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
            WaimaiDeliveryConfirmRequest temp = new WaimaiDeliveryConfirmRequest
            {
                DeliveryOrderNo = ParseHelper.ToLong(p.OriginalOrderNo),
                Lng = p.Longitude.ToString(),
                Lat = p.Latitude.ToString()
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
            paramodel.store_info.delivery_fee = 0;//全时目前外送费统一0
            paramodel.store_info.businesscommission = 9;//全时目前结算比例统一9
            return paramodel;
        }
    }
}
