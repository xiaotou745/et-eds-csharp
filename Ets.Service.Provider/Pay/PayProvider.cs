using Ets.Service.IProvider.Pay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common.AliPay;
using ETS.Enums;
using Ets.Model.Common;
using ETS.Expand;
using ETS.Util;
using ETS.AliPay;
using System.Xml;
using Ets.Dao.Order;
using Ets.Model.DataModel.Order;

namespace Ets.Service.Provider.Pay
{
    public class PayProvider : IPayProvider
    {
        AlipayIntegrate alipayIntegrate = new AlipayIntegrate();
        OrderChildDao orderChildDao = new OrderChildDao();
        #region 生成支付宝订单

        /// <summary>
        /// 生成支付宝订单
        /// 窦海超
        /// 2015年5月12日 14:35:05
        /// </summary>
        /// <param name="model"></param>
        [ActionStatus(typeof(AliPayStatus))]
        public ResultModel<PayResultModel> CreatePay(Model.ParameterModel.AliPay.PayModel model)
        {
            LogHelper.LogWriter("=============支付请求数据：", model);
            if (model.payType == 1)
            {
                LogHelper.LogWriter("=============支付支付宝支付：");
                ////支付宝支付
                //数据库里查询订单信息
                OrderChild orderChildModel = orderChildDao.GetOrderChildInfo(model.orderId, model.childId);
                return QRCodeAdd(model.orderId, model.childId, orderChildModel.TotalPrice);
            }
            if (model.payType == 2)
            {
                //微信支付
            }
            return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
        }

        /// <summary>
        /// 生成支付宝二维码
        /// 窦海超
        /// 2015年5月12日 14:36:37
        /// </summary>
        /// <param name="orderNumber">订单号</param>
        /// <param name="payAmount">支付金额</param>
        /// <returns></returns>
        private ResultModel<PayResultModel> QRCodeAdd(int orderId, int childId, decimal payAmount)
        {

            PayResultModel resultModel = new PayResultModel();
            var qrcodeUrl = alipayIntegrate.GetQRCodeUrl(orderId, childId, payAmount);
            if (string.IsNullOrEmpty(qrcodeUrl))
            {
                return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
            }
            resultModel.aliQRCode = qrcodeUrl;
            resultModel.orderId = orderId;
            resultModel.childId = childId;
            resultModel.payAmount = payAmount;
            resultModel.payType = 1;
            return ResultModel<PayResultModel>.Conclude(AliPayStatus.success, resultModel);
        }

        #endregion

        /// <summary>
        /// 订单回调
        /// 窦海超
        /// 2015年5月12日 14:36:42
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        
        public dynamic ReturnAlipay()
        {
            var request = System.Web.HttpContext.Current.Request;
            try
            {
                //int orderId = ParseHelper.ToInt(request["orderId"], 0);
                //int orderChildId = ParseHelper.ToInt(request["orderChildId"], 0);
                string goods_id = request["goods_id"];
                //if (string.IsNullOrEmpty(goods_id) || !goods_id.Contains("_"))
                //{
                //    LogHelper.LogWriter("订单编号为null");
                //    return new { is_success = "F", error_code = "PARAM_ILLEGAL" };
                //}
                //int orderId = ParseHelper.ToInt(goods_id.Split('_')[0], 0);
                //int orderChildId = ParseHelper.ToInt(goods_id.Split('_')[1], 0);
                //if (orderId <= 0 || orderChildId <= 0)
                //{
                //    LogHelper.LogWriter("订单编号为null");
                //    return new { is_success = "F", error_code = "PARAM_ILLEGAL" };
                //}
                //更新订单状态
                //if (orderChildDao.FinishStatus(orderId, orderChildId))
                //{
                //    return new { is_success = "T", out_trade_no = orderId + "_" + orderChildId };
                //}
                //else
                //{
                //return new { is_success = "F", error_code = "PARAM_ILLEGAL" };

                //return new { is_success = "T", out_trade_no = orderId + "_" + orderChildId };

                //return new { is_success = "T", out_trade_no = goods_id };
                return new { is_success = "T", out_trade_no = goods_id };
                //}

            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "alipay自动回调异常");
                return new { is_success = "F" };
            }
        }

        /// <summary>
        /// 订单回调
        /// 窦海超
        /// 2015年5月12日 14:36:48
        /// </summary>
        /// <returns></returns>
        public dynamic AlipayResult()
        {
            try
            {
                var request = System.Web.HttpContext.Current.Request;
                string sign = request["sign"];
                string sign_type = request["sign_type"];
                string notify_data = request["notify_data"];
                XmlDocument xmlDoc = new XmlDocument();
                AlipayNotifyData notify = new AlipayNotifyData();
                xmlDoc.LoadXml(notify_data);
                notify.buyer_email = xmlDoc.SelectSingleNode("notify/buyer_email").InnerText;
                notify.trade_status = xmlDoc.SelectSingleNode("notify/trade_status").InnerText;
                notify.out_trade_no = xmlDoc.SelectSingleNode("notify/out_trade_no").InnerText;
                notify.trade_no = xmlDoc.SelectSingleNode("notify/trade_no").InnerText;
                if (string.IsNullOrEmpty(notify.trade_status) || notify.trade_status != "TRADE_SUCCESS")
                {
                    string fail = string.Concat("错误啦trade_status：", notify.trade_status, "。sign:", sign, "。notify_data:", notify_data);
                    LogHelper.LogWriter(fail);
                    return "fail";
                }
                string orderNo = notify.out_trade_no;
                if (string.IsNullOrEmpty(orderNo) || !orderNo.Contains("_"))
                {
                    string fail = string.Concat("错误啦orderNo：", orderNo);
                    LogHelper.LogWriter(fail);
                    return "fail";
                }
                int orderId = ParseHelper.ToInt(orderNo.Split('_')[0]);
                int orderChildId = ParseHelper.ToInt(orderNo.Split('_')[1]);
                if (orderId <= 0 || orderChildId <= 0)
                {
                    string fail = string.Concat("错误啦orderId：", orderId, ",orderChildId:", orderChildId);
                    LogHelper.LogWriter(fail);
                    return "fail";
                }
                if (orderChildDao.FinishStatus(orderId, orderChildId))
                {
                    //jpush
                    string success = string.Concat("成功，当前订单OrderId:", orderId, ",OrderChild:", orderChildId);
                    LogHelper.LogWriter(success);
                    return "success";
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "Alipay自动返回异常");
                return "fail";
            }
            return "fail";
        }

        /// <summary>
        /// 查询支付状态
        /// 窦海超
        /// 2015年5月12日 14:36:55
        /// </summary>
        /// <returns></returns>
        public dynamic GetOrderPayStatus(OrderPayModel model)
        {
            try
            {
                OrderChild orderChildModel = orderChildDao.GetOrderChildInfo(model.orderId, model.childId);
                if (orderChildModel == null)
                {
                    return new { status_code = -1, status_message = "order_id:" + model.orderId + "_" + model.childId + "错误" };
                }
                if (orderChildModel != null || orderChildModel.PayStatus == 2)
                {
                    return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 1 } };
                }
                //支付宝
                if (model.payType == 1)
                {
                    string orderNo = model.orderId + "_" + model.childId;
                    return alipayIntegrate.GetOrder(orderNo);
                }
                //微信
                if (model.payType == 2)
                {
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "查询支付状态异常");
                return new { status_code = -1, status_message = string.Empty };
            }
            return null;
        }
    }
}
