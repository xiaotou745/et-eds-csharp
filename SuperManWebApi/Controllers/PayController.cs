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
        /// 窦海超
        /// 2015年5月12日 14:35:05
        /// </summary>
        /// <param name="model"></param>
        [HttpGet]
        //[HttpPost]
        public ResultModel<PayResultModel> CreatePay()//PayModel model
        {
            PayModel model = new PayModel()
            {
                orderId = 2114,
                childId = 1,
                payType = 1,
                version = "1.0"
            };
            //return ResultModel<PayResultModel>.Conclude(AliPayStatus, );
            return payProvider.CreatePay(model);
        }

        /// <summary>
        /// 订单回调
        /// 窦海超
        /// 2015年5月12日 14:35:10
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public dynamic ReturnAlipay()
        {
            return payProvider.ReturnAlipay();
        }

        /// <summary>
        /// Alipay自动返回
        /// 窦海超
        /// 2015年5月12日 14:35:15
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [HttpPost]
        public dynamic AlipayResult()
        {
            return payProvider.AlipayResult();
        }

        /// <summary>
        /// 查询支付状态
        /// 窦海超
        /// 2015年5月12日 14:35:19
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public dynamic GetOrderPayStatus(OrderPayModel model)
        {
            return payProvider.GetOrderPayStatus(model);
        }
    }
}
