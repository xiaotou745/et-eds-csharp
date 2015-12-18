using System.Text;
using System.Web;
using Ets.Dao.Common;
using Ets.Model.Common;
using Ets.Model.Common.AliPay;
using Ets.Model.DomainModel.Business;
using Ets.Model.DomainModel.Finance;
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
using ETS.Library.Pay.SSAliPay;

namespace SuperManWebApi.Controllers
{
    public class PayController : ApiController
    {
        readonly IPayProvider payProvider = new PayProvider();

        #region TestMethod
        [HttpGet]
        public ResultModel<PayResultModel> CreatePayTest(int orderId)
        {
            //PayModel model = new PayModel()
            //{
            //    productId = 1,
            //    orderId = orderId,
            //    childId = 1,
            //    payType = 2,
            //    version = "1.0",
            //    payStyle = 1
            //};
            //return payProvider.CreatePay(model);
            //微信支付
            //PayModel model = new PayModel()
            //{
            //    payStyle = 0,
            //    tipAmount = ParseHelper.ToDecimal(0.01),
            //    payType = 2,
            //    orderId = orderId
            //};
            //return payProvider.CreateFlashPay(model);
            //微信退款
            ETS.Library.Pay.SSBWxPay.NativePay nav = new ETS.Library.Pay.SSBWxPay.NativePay();
            bool s = nav.Refund(
                "2125151217163453670",//易代送单号
                "1005451006201512172140602927",//微信单号
                1,//总金额
                1,//退款金额
                "2125151217163453670"//易代送单号
                );
            return null;
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

        #region 骑士代付,扫码支付
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
 
        /// <summary>
        /// 现金支付
        /// wc
        /// </summary>
        [Token]
        public ResultModel<PayResultModel> CashPay(PayModel model)//
        {
            return payProvider.CashPay(model);
        }
        #endregion

        #region 商家充值
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
        /// 商家充值回调方法回调 支付宝
        /// 窦海超
        /// 2015年5月29日 15:17:07
        /// </summary>
        /// <returns></returns>
        public void BusinessRechargeNotify()
        {
            payProvider.BusinessRechargeNotify();
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

        #endregion

        #region 商家冲值 闪送模式
        /// <summary>
        /// 商家充值微信回调方法回调
        /// 窦海超
        /// 2015年5月29日 15:09:29
        /// </summary>
        /// <returns></returns>
        public void SSBusinessRechargeWxNotify()
        {
            payProvider.SSBusinessRechargeWxNotify();
        }
        #endregion

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
        /// 支付宝批量付款接受回调
        /// 茹化肖
        /// 2015年10月19日16:55:31
        /// </summary>
        [HttpPost]
        [HttpGet]
        public void AlipayForBatchCallBack()
        {
            HttpRequest req = HttpContext.Current.Request;
            #region===回调取参
            StringBuilder sb = new StringBuilder();
            AlipayBatchCallBackModel model = new AlipayBatchCallBackModel();
            var notify_time = req["notify_time"];//异步通知时间
            sb.Append(notify_time + "\r\n");
            model.NotifyTime = notify_time;
            var notify_type = req["notify_type"];//异步通知类型
            sb.Append(notify_type + "\r\n");
            model.NotifyType = notify_type;
            var notify_id = req["notify_id"];//异步通知ID
            sb.Append(notify_id + "\r\n\n");
            model.NotifyId = notify_id;
            var sign_type = req["sign_type"];//签名类型
            sb.Append(sign_type + "\r\n\n");
            model.SignType = sign_type;
            var sign = req["sign"];//签名内容
            sb.Append(sign + "\r\n\n");
            model.Sign = sign;
            var batch_no = req["batch_no"];//批次号
            sb.Append(batch_no + "\r\n\n");
            model.BatchNo = batch_no;
            var pay_user_id = req["pay_user_id"];//付款账号ID
            sb.Append(pay_user_id + "\r\n\n");
            model.PayUserId = pay_user_id;
            var pay_user_name = req["pay_user_name"];//付款账户名称
            sb.Append(pay_user_name + "\r\n\n");
            model.PayUserName = pay_user_name;
            var pay_account_no = req["pay_account_no"];//付款账户
            sb.Append(pay_account_no + "\r\n\n");
            model.PayAccountNo = pay_account_no;
            var success_details = req["success_details"];//付款成功列表
            sb.Append(success_details + "\r\n\n");
            model.SuccessDetails = success_details;
            var fail_details = req["fail_details"];//付款失败列表
            sb.Append(fail_details + "\r\n\n");
            model.FailDetails = fail_details;
            LogHelper.LogWriterString(sb.ToString());
            #endregion
            HttpContext.Current.Response.Clear();
            if (payProvider.AlipayTransferCallback(model))
            {
                HttpContext.Current.Response.Write("success");
            }
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

        #region  闪送模式
        /// <summary>
        /// 生成订单 闪送
        /// 胡灵波
        /// 2015年12月8日 11:14:04
        /// </summary>
        //[Token]
        public ResultModel<PayResultModel> CreateFlashPay(PayModel model)
        {
            return payProvider.CreateFlashPay(model);
        }


        /// <summary>
        /// 支付宝回调 闪送模式
        /// 发单商家加小费 抢单商家加小费
        /// 胡灵波
        /// 2015年12月8日 17:20:38
        /// </summary>
        /// <returns></returns>
        public dynamic NotifyTip()
        {
            return payProvider.NotifyTip();
        }


        /// <summary>
        /// 微信回调 闪送模式
        /// 发单商家加小费 抢单商家加小费
        /// 胡灵波
        /// 2015年12月8日 17:20:38
        /// </summary>
        /// <returns></returns>
        public void SSWxNotify()
        {
            payProvider.SSWxNotify();
        }
        #endregion
    }
}
