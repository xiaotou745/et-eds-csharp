using System.Text;
using System.Web;
using Ets.Dao.Common;
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
using ETS.Util;
using SuperManWebApi.App_Start.Filters;

namespace SuperManWebApi.Controllers
{
    public class PayController : ApiController
    {
        readonly IPayProvider payProvider = new PayProvider();
        #region TestMethod
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

        [HttpGet]
        public ResultModel<BusinessRechargeResultModel> CreateRechargeTest()
        {
            BusinessRechargeModel model = new BusinessRechargeModel()
            {
                Businessid = 1987,
                payAmount = ETS.Util.ParseHelper.ToDecimal(0.01),
                PayType = 2,
                Version = "1.1"
            };
            ResultModel<BusinessRechargeResultModel> result = payProvider.BusinessRecharge(model);
            return result;
        }


        #endregion
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


        /// <summary>
        /// 现金支付
        /// wc
        /// </summary>
        [Token]
        public ResultModel<PayResultModel> CashPay(PayModel model)//
        {
            return payProvider.CashPay(model);
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
        /// 支付宝批量付款接受回调
        /// 茹化肖
        /// 2015年10月19日16:55:31
        /// </summary>
        [HttpGet]
        [HttpPost]
        public void AlipayForBatch()
        {
            //
            HttpRequest req = HttpContext.Current.Request;
            StringBuilder sb = new StringBuilder();
            var notify_time = req.Form["notify_time"];
            sb.Append(notify_time + "\r\n");
            var notify_type = req.Form["notify_type"];
            sb.Append(notify_type + "\r\n");
            var notify_id = req.Form["notify_id"];
            sb.Append(notify_id + "\r\n\n");
            var sign_type = req.Form["sign_type"];
            sb.Append(sign_type + "\r\n\n");
            var sign = req.Form["sign"];
            sb.Append(sign + "\r\n\n");
            var batch_no = req.Form["batch_no"];
            sb.Append(batch_no + "\r\n\n");
            var pay_user_id = req.Form["pay_user_id"];
            sb.Append(pay_user_id + "\r\n\n");
            var pay_user_name = req.Form["pay_user_name"];
            sb.Append(pay_user_name + "\r\n\n");
            var pay_account_no = req.Form["pay_account_no"];
            sb.Append(pay_account_no + "\r\n\n");
            var success_details = req.Form["success_details"];
            sb.Append(success_details + "\r\n\n");
            var fail_details = req.Form["fail_details"];
            sb.Append(fail_details + "\r\n\n");
            LogHelper.LogWriterString(sb.ToString());
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("success");
            HttpContext.Current.Response.End();
            
         
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
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Token]
        public ResultModel<BusinessRechargeResultModel> BusinessRecharge(BusinessRechargeModel model)
        {
            return payProvider.BusinessRecharge(model);
        }

        /// <summary>
        /// 商家充值微信回调方法回调
        /// 窦海超
        /// 2015年5月29日 15:09:29
        /// </summary>
        /// <returns></returns>
        public void BusinessRechargeWxNotify()
        {
            payProvider.BusinessRechargeWxNotify();
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
        public void WxNotify()
        {
            payProvider.WxNotify();
        }
        #endregion


        /// <summary>
        /// 易宝转账回调接口  add by caoheyang  20150715
        /// 茹化肖修改
        /// 2015年8月26日13:23:10
        /// </summary>
        [HttpGet]
        [HttpPost]
        public void YeePayCashTransferCallback()
        {
            string data = HttpContext.Current.Request["data"];
            ETS.Util.LogHelper.LogWriter(DateTime.Now + "易宝回调：" + data);
            //if (payProvider.YeePayCashTransferCallback(data))//如果返回值是成功
            //{
            //    HttpContext.Current.Response.Write("SUCCESS");
            //    HttpContext.Current.Response.End();
            //}
        }
    }
}
