using Ets.Model.Common;
using Ets.Model.Common.AliPay;
using Ets.Model.ParameterModel.AliPay;
using Ets.Service.IProvider.Pay;
using Ets.Service.Provider.Pay;
using ETS.Enums;
using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SuperManWebApi.Controllers
{
    public class PayController : ApiController
    {
        IPayProvider payProvider = new PayProvider();

        /// <summary>
        /// 生成支付宝订单
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public ResultModel<PayResultModel> CreatePay(PayModel model)
        {
            //return ResultModel<PayResultModel>.Conclude(AliPayStatus, );
            return payProvider.CreatePay(model);
        }

        /// <summary>
        /// 订单回调
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultModel<NotifyResultModel> Notify(NotifyModel model)
        {
            return payProvider.Notify(model);
        }
    }
}
