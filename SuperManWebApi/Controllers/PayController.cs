﻿using System.Web;
using Ets.Model.Common;
using Ets.Model.Common.AliPay;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.AliPay;
using Ets.Model.ParameterModel.Business;
using ETS.Pay.YeePay;
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
using SuperManWebApi.App_Start.Filters;

namespace SuperManWebApi.Controllers
{
    public class PayController : ApiController
    {
        readonly IPayProvider payProvider = new PayProvider();
        /// <summary>
        /// 生成支付宝订单
        /// 窦海超
        /// 2015年5月12日 14:35:05
        /// </summary>
        [Token]
        public ResultModel<PayResultModel> CreatePay(PayModel model)//
        {
            return payProvider.CreatePay(model);
        }
        [HttpGet]
       public ResultModel<PayResultModel> CreatePayTest(int orderId)
        {
            PayModel model = new PayModel()
            {
                productId = 1,
                orderId = orderId,
                childId = 1,
                payType = 2,
                version = "1.0",
                payStyle = 1
            };
           return payProvider.CreatePay(model);
       }

        #region 支付宝

        /// <summary>
        /// 支付宝创建订单  回调
        /// 窦海超
        /// 2015年5月12日 14:35:10
        /// </summary>
        /// <returns></returns>
        public dynamic ReturnAlipay()
        {
            return payProvider.ReturnAlipay();
        }

        /// <summary>
        /// Alipay自动返回,异步处理   回调
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

        /// <summary>
        /// 商家充值
        /// 窦海超
        /// 2015年5月29日 15:09:29
        /// <param name="model"></param>
       [Token]
        public ResultModel<BusinessRechargeResultModel> BusinessRecharge(BusinessRechargeModel model)
        {
            return payProvider.BusinessRecharge(model);
        }

        /// <summary>
        /// 商家充值回调方法回调
        /// 窦海超
        /// 2015年5月29日 15:17:07
        /// </summary>
        /// <returns></returns>
        public void BusinessRechargeNotify()
        {
            payProvider.BusinessRechargeNotify();
        }
        #endregion

        #region 微信

        /// <summary>
        /// 微信支付 回调
        /// 窦海超
        /// 2015年5月13日 15:02:42
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        public dynamic WxNotify()
        {
            return payProvider.WxNotify();
        }
        #endregion

       
        /// <summary>
        /// 易宝转账回调接口  add by caoheyang  20150715
        /// </summary>
        [HttpGet]
        [HttpPost]
        public void YeePayCashTransferCallback()
        {
            string data = HttpContext.Current.Request["data"];
            payProvider.YeePayCashTransferCallback(data);
            HttpContext.Current.Response.Write("SUCCESS");
            HttpContext.Current.Response.End();
        }
    }
}
