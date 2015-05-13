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
        [HttpGet]
        public ResultModel<PayResultModel> CreatePay()//PayModel model
        {
            PayModel model = new PayModel()
            {
                orderId = 2120,
                childId = 4,
                payType = 2,
                version = "1.0"
            };
            return payProvider.CreatePay(model);
        }

        #region 支付宝

        /// <summary>
        /// 支付宝创建订单
        /// 窦海超
        /// 2015年5月12日 14:35:10
        /// </summary>
        /// <returns></returns>
        public dynamic ReturnAlipay()
        {
            return payProvider.ReturnAlipay();
        }

        /// <summary>
        /// Alipay自动返回,异步处理
        /// 窦海超
        /// 2015年5月12日 14:35:15
        /// </summary>
        /// <returns></returns>
        public dynamic Notify()
        {
            return payProvider.Notify();
        }

        /// <summary>
        /// 查询支付状态
        /// 窦海超
        /// 2015年5月12日 14:35:19
        /// </summary>
        /// <returns></returns>
        public dynamic GetOrderPayStatus(OrderPayModel model)
        {
            //OrderPayModel model = new OrderPayModel()
            //{
            //    childId = 1,
            //    orderId = 2114,
            //    payType = 1,
            //    payStyle = 1
            //};
            return payProvider.GetOrderPayStatus(model);
        }

        #endregion

        #region 微信

        /// <summary>
        /// 微信支付
        /// 窦海超
        /// 2015年5月13日 15:02:42
        /// </summary>
        /// <returns></returns>
        public dynamic ReturnWxpay()
        {
            return payProvider.ReturnWxpay();
        }
        #endregion

    }
}
