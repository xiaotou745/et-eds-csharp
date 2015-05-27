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
using ETS.Pay.AliPay;
using System.Xml;
using Ets.Dao.Order;
using Ets.Model.DataModel.Order;
using ETS.Pay.WxPay;
using Ets.Model.DomainModel.Bussiness;

namespace Ets.Service.Provider.Pay
{
    public class PayProvider : IPayProvider
    {
        AlipayIntegrate alipayIntegrate = new AlipayIntegrate();
        OrderChildDao orderChildDao = new OrderChildDao();
        #region 生成支付宝、微信二维码订单

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
            PayStatusModel payStatusModel = orderChildDao.GetPayStatus(model.orderId, model.childId);
            if (payStatusModel == null)
            {
                string err = string.Concat("订单不存在,主订单号：", model.orderId, ",子订单号:", model.childId);
                LogHelper.LogWriter(err);
                return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
            }
            //所属产品_主订单号_子订单号_支付方式
            string orderNo = string.Concat(model.productId, "_", model.orderId, "_", model.childId, "_", model.payStyle);
            if (model.payType == PayTypeEnum.ZhiFuBao.GetHashCode())
            {
                LogHelper.LogWriter("=============支付宝支付：");
                ////支付宝支付
                //数据库里查询订单信息
                //if (payStatusModel.PayStatus == PayStatusEnum.WaitPay.GetHashCode())//待支付
                //{
                return CreateAliPayOrder(orderNo, payStatusModel.TotalPrice, model.orderId);
                //}
            }
            if (model.payType == PayTypeEnum.WeiXin.GetHashCode())
            {
                //微信支付
                LogHelper.LogWriter("=============微信支付：");
                return CreateWxPayOrder(orderNo, payStatusModel.TotalPrice, payStatusModel.WxCodeUrl);
            }
            return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
        }

        /// <summary>
        /// 完成订单后发送jpush消息 
        /// 窦海超
        /// 2015年5月13日 15:23:16
        /// </summary>
        /// <param name="model"></param>
        private static void FinishOrderPushMessage(OrderChildFinishModel model)
        {
            OrderChildPayModel orderChildPayModel = new OrderDao().GetOrderById(model.orderId);
            if (orderChildPayModel == null)
            {
                return;
            }
            JPushModel jpushModel = new JPushModel()
            {
                Alert = "订单支付完成！",
                City = string.Empty,
                Content = string.Concat(model.orderId, "_", model.orderChildId, "_", orderChildPayModel.PayStatus),
                RegistrationId = orderChildPayModel.clienterId.ToString(),//通过订单ID获取要发送的骑士ID
                TagId = 0,
                Title = "订单提醒"
            };
            //Ets.Service.Provider.MyPush.Push.PushMessage(0, "订单提醒", "有订单完成了！", "有订单完成了！", string.Concat(model.orderId, "_", model.orderChildId), string.Empty);
            Ets.Service.Provider.MyPush.Push.PushMessage(jpushModel);
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
                PayStatusModel orderChildModel = orderChildDao.GetPayStatus(model.orderId, model.childId);
                if (orderChildModel == null)
                {
                    return new { status_code = -1, status_message = "order_id:" + model.orderId + "_" + model.childId + "错误" };
                }
                if (orderChildModel.PayStatus == PayStatusEnum.HadPay.GetHashCode())//如果数据库里返回已完成直接返回
                {
                    return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 1 } };
                }
                string orderNo = string.Concat(model.productId, "_", model.orderId, "_", model.childId + "_", model.payStyle);
                //支付宝
                if (model.payType == PayTypeEnum.ZhiFuBao.GetHashCode())
                {
                    return alipayIntegrate.GetOrder(orderNo, model.orderId, model.childId);
                }
                //微信
                if (model.payType == PayTypeEnum.WeiXin.GetHashCode())
                {
                    WXpayService wxpay = new WXpayService();
                    return wxpay.GetOrder(orderNo);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "查询支付状态异常");
                return new { status_code = -1, status_message = string.Empty };
            }
            return null;
        }
        #endregion

        #region 支付宝相关


        /// <summary>
        /// 生成支付宝二维码
        /// 窦海超
        /// 2015年5月12日 14:36:37
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="payAmount">支付金额</param>
        /// <param name="orderId">订单ID，用于查询商家信息用</param>
        /// <returns></returns>
        private ResultModel<PayResultModel> CreateAliPayOrder(string orderNo, decimal payAmount, int orderId)
        {
            #region 通过订单ID，用于查询商家信息用
            BusinessDM businessModel = new Ets.Dao.User.BusinessDao().GetByOrderId(orderId);
            string businessName = string.Empty;
            if (businessModel == null || string.IsNullOrEmpty(businessModel.Name))
            {
                businessName = "e代送收款";
            }
            else
            {
                businessName = businessModel.Name;
            }
            #endregion

            PayResultModel resultModel = new PayResultModel();
            var qrcodeUrl = alipayIntegrate.GetQRCodeUrl(orderNo, payAmount, businessName);
            if (string.IsNullOrEmpty(qrcodeUrl))
            {
                return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
            }
            resultModel.aliQRCode = qrcodeUrl;
            resultModel.orderNo = orderNo;
            resultModel.payAmount = payAmount;
            resultModel.payType = PayTypeEnum.ZhiFuBao.GetHashCode();
            resultModel.notifyUrl = ETS.Config.NotifyUrl;
            return ResultModel<PayResultModel>.Conclude(AliPayStatus.success, resultModel);
        }

        /// <summary>
        /// 支付宝创建订单
        /// 窦海超
        /// 2015年5月12日 14:36:42
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic ReturnAlipay()
        {
            var error = new { is_success = "F", error_code = "PARAM_ILLEGAL" };
            try
            {
                var request = System.Web.HttpContext.Current.Request;
                string goods_id = request["goods_id"];

                if (string.IsNullOrEmpty(goods_id) || !goods_id.Contains("_"))
                {
                    LogHelper.LogWriter("订单编号为null");
                    return error;
                }
                int productId = ParseHelper.ToInt(goods_id.Split('_')[0], 0);//产品ID
                int orderId = ParseHelper.ToInt(goods_id.Split('_')[1], 0);//主订单号
                int orderChildId = ParseHelper.ToInt(goods_id.Split('_')[2], 0);//子订单号
                if (orderId <= 0 || orderChildId <= 0)
                {
                    LogHelper.LogWriter("订单号或子订单号为零");
                    return error;
                }
                PayStatusModel payStatusModel = orderChildDao.GetPayStatus(orderId, orderChildId);
                if (payStatusModel == null || payStatusModel.PayStatus == PayStatusEnum.HadPay.GetHashCode())//判断当前订单号是否存在，是否为已完成 
                {
                    return error;
                }
                //变更为支付中
                if (orderChildDao.ZhuFuZhongPayStatus(orderId, orderChildId))
                {
                    return new { is_success = "T", out_trade_no = goods_id };
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "alipay自动回调异常");
            }
            return error;
        }

        /// <summary>
        /// 订单回调
        /// 窦海超
        /// 2015年5月12日 14:36:48
        /// </summary>
        /// <returns></returns>
        public dynamic Notify()
        {
            try
            {
                #region 参数绑定

                var request = System.Web.HttpContext.Current.Request;
                string sign = request["sign"];
                string sign_type = request["sign_type"];
                string notify_data = request["notify_data"];
                AlipayNotifyData notify = new AlipayNotifyData();
                if (!string.IsNullOrEmpty(notify_data))
                {
                    //如果是二维码支付
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(notify_data);
                    notify.buyer_email = xmlDoc.SelectSingleNode("notify/buyer_email").InnerText;
                    notify.trade_status = xmlDoc.SelectSingleNode("notify/trade_status").InnerText;
                    notify.out_trade_no = xmlDoc.SelectSingleNode("notify/out_trade_no").InnerText;
                    notify.trade_no = xmlDoc.SelectSingleNode("notify/trade_no").InnerText;
                }
                else
                {
                    //否则是骑士代付
                    notify.buyer_email = request["buyer_email"];
                    notify.trade_status = request["trade_status"];
                    notify.out_trade_no = request["out_trade_no"];
                    notify.trade_no = request["trade_no"];
                }
                #endregion
                //如果状态为空或状态不等于同步成功和异步成功就认为是错误
                if (string.IsNullOrEmpty(notify.trade_status))
                {
                    string fail = string.Concat("错误啦trade_status：", notify.trade_status, "。sign:", sign, "。notify_data:", notify_data);
                    LogHelper.LogWriter(fail);
                    return "fail";
                }
                #region 回调完成状态
                if (notify.trade_status == "TRADE_SUCCESS" || notify.trade_status == "TRADE_FINISHED")
                {
                    string orderNo = notify.out_trade_no;
                    if (string.IsNullOrEmpty(orderNo) || !orderNo.Contains("_"))
                    {
                        string fail = string.Concat("错误啦orderNo：", orderNo);
                        LogHelper.LogWriter(fail);
                        return "fail";
                    }
                    int productId = ParseHelper.ToInt(orderNo.Split('_')[0]);//产品编号
                    int orderId = ParseHelper.ToInt(orderNo.Split('_')[1]);//主订单号
                    int orderChildId = ParseHelper.ToInt(orderNo.Split('_')[2]);//子订单号
                    int payStyle = ParseHelper.ToInt(orderNo.Split('_')[3]);//支付方式(1 用户支付 2 骑士代付)
                    if (orderId <= 0 || orderChildId <= 0)
                    {
                        string fail = string.Concat("错误啦orderId：", orderId, ",orderChildId:", orderChildId);
                        LogHelper.LogWriter(fail);
                        return "fail";
                    }

                    OrderChildFinishModel model = new OrderChildFinishModel()
                    {
                        orderChildId = orderChildId,
                        orderId = orderId,
                        payBy = notify.buyer_email,
                        payStyle = payStyle,
                        payType = PayTypeEnum.ZhiFuBao.GetHashCode(),
                        originalOrderNo = notify.trade_no,
                    };

                    if (orderChildDao.FinishPayStatus(model))
                    {
                        //jpush
                        //Ets.Service.Provider.MyPush.Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", myorder.businessId.ToString(), string.Empty);
                        FinishOrderPushMessage(model);//完成后发送jpush消息
                        string success = string.Concat("成功，当前订单OrderId:", orderId, ",OrderChild:", orderChildId);
                        LogHelper.LogWriter(success);
                        return "success";
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "Alipay自动返回异常");
                return "fail";
            }
            return "fail";
        }



        #endregion

        #region 微信相关

        /// <summary>
        /// 生成微信二维码订单
        /// 窦海超
        /// 2015年5月13日 14:57:38
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="WxCodeUrl">微信地址</param>
        /// <param name="TotalPrice">总金额，注意:微信要乘以100=最后支付的金额，这里传值前不要乘以100</param>
        /// <returns></returns>
        public ResultModel<PayResultModel> CreateWxPayOrder(string orderNo, decimal totalPrice, string wxCodeUrl)
        {
            //支付方式-主订单ID-子订单ID
            PayResultModel resultModel = new PayResultModel();
            string code_url = wxCodeUrl;
            if (string.IsNullOrEmpty(code_url))//先查一下库是否存在二维码地址，不存在去微信生成
            {
                string wx_nonceStr = RequestHandler.getNoncestr();
                WXpayService wxpay = new WXpayService("127.0.0.1", orderNo, "e代送", wx_nonceStr, (Convert.ToInt32(totalPrice * 100)).ToString());//传给微信的金额
                code_url = wxpay.CreateNativeApi();
                if (string.IsNullOrEmpty(code_url))
                {
                    return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
                }
                int productId = ParseHelper.ToInt(orderNo.Split('_')[0]);
                int orderId = ParseHelper.ToInt(orderNo.Split('_')[1]);
                int childId = ParseHelper.ToInt(orderNo.Split('_')[2]);
                if (productId == ProductEnum.OrderChildPay.GetHashCode())
                {
                    //如果是子订单支付 
                    orderChildDao.UpdateWxCodeUrl(orderId, childId, code_url);//把获取到的支付宝地址更新到子订单下
                }
            }

            resultModel.aliQRCode = code_url;//微信地址
            resultModel.orderNo = orderNo;//订单号
            resultModel.payAmount = totalPrice;//总金额，没乘以100的值
            resultModel.payType = PayTypeEnum.WeiXin.GetHashCode();//微信
            resultModel.notifyUrl = ETS.Config.WXNotifyUrl;//回调地址
            return ResultModel<PayResultModel>.Conclude(AliPayStatus.success, resultModel);
        }

        /// <summary>
        /// 微信支付回调方法 
        /// 窦海超
        /// 2015年5月13日 15:03:45
        /// </summary>
        /// <returns></returns>
        public dynamic ReturnWxpay()
        {

            ResponseHandler resHandler = new ResponseHandler(System.Web.HttpContext.Current);
            try
            {
                string return_code = resHandler.getParameter("return_code");
                string return_msg = resHandler.getParameter("return_msg");
                string out_trade_no = resHandler.getParameter("out_trade_no");
                //微信支付订单号
                string transaction_id = resHandler.getParameter("transaction_id");
                string openid = resHandler.getParameter("openid");
                if (string.IsNullOrEmpty(out_trade_no) || !out_trade_no.Contains("_"))
                {
                    LogHelper.LogWriter("订单号异常,微信单号为：" + transaction_id);
                    return new { return_code = "FAIL" };
                }
                if (!string.IsNullOrEmpty(return_code) && return_code == "SUCCESS")
                {
                    int productId = ParseHelper.ToInt(out_trade_no.Split('_')[0], 0);
                    int orderId = ParseHelper.ToInt(out_trade_no.Split('_')[1], 0);
                    int orderChildId = ParseHelper.ToInt(out_trade_no.Split('_')[2], 0);
                    int payStyle = ParseHelper.ToInt(out_trade_no.Split('_')[3], 0);
                    OrderChildFinishModel model = new OrderChildFinishModel()
                      {
                          orderChildId = orderChildId,
                          orderId = orderId,
                          payBy = openid,
                          payStyle = payStyle,
                          payType = PayTypeEnum.WeiXin.GetHashCode(),
                          originalOrderNo = transaction_id
                      };
                    if (orderChildDao.FinishPayStatus(model))
                    {
                        //业务处理
                        return new { return_code = "SUCCESS" };
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "微信支付回调异常");
            }
            return new { return_code = "FAIL" };
        }

        #endregion
    }
}
