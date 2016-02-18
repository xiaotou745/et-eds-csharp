using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using Ets.Dao.Order;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.OpenApi;
using ETS.Util;

namespace Ets.Service.Provider.OpenApi
{
    /// <summary>
    /// 根据回调地址E代送内部订单流转时通知第三方
    /// add by caoheyang  20150217
    /// </summary>
    public class DefaultGroup : IGroupProviderOpenApi
    {
        /// <summary>
        /// 状态流转回调通知第三方
        /// caoheyang 20150127
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel)
        {
            var p = new
            {
                app_key = paramodel.app_key,
                timestamp = paramodel.timestamp,
                v = paramodel.v,
                sign = paramodel.sign,
                fields = new
                {
                    order_no = paramodel.fields.OriginalOrderNo,  //第三方平台订单号
                    status = paramodel.fields.status, //E代送订单状态
                    clientername = paramodel.fields.ClienterTrueName, //配送员姓名
                    clienterphone = paramodel.fields.ClienterPhoneNo, //配送员电话
                    businessName = paramodel.fields.BusinessName, //商户名
                    cancelreason = paramodel.fields.OtherCancelReason //订单取消原因
                }
            };
            string json = new HttpClient().PostAsJsonAsync(paramodel.fields.ReturnUrl, paramodel).Result.Content.ReadAsStringAsync().Result;
            int r = new OpenCallBackLogDao().Insert(new OpenCallBackLog()
             {
                 Url = paramodel.fields.ReturnUrl,
                 OrderId = 0,
                 OrderNo = paramodel.fields.order_no,
                 RequestBody = JsonHelper.ToJson(p),
                 ResponseBody = json,
                 Status = paramodel.fields.status
             });

            if (!string.IsNullOrWhiteSpace(json) && json == "success")
            {
                return OrderApiStatusType.Success;
            }
            return OrderApiStatusType.SystemError;
        }
    }
}
