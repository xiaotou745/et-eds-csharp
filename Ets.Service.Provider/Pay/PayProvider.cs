using ETS.Const;
using Ets.Dao.Common.YeePay;
using Ets.Model.Common.YeePay;
using Ets.Model.DomainModel.Finance;
using ETS.Pay.YeePay;
using ETS.Security;
using Ets.Service.IProvider.Finance;
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
using Ets.Service.Provider.Finance;
using ETS.Util;
using ETS.Pay.AliPay;
using System.Xml;
using Ets.Dao.Order;
using Ets.Model.DataModel.Order;
//using ETS.Pay.WxPay;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.Business;
using Ets.Dao.User;
using ETS.Transaction.Common;
using ETS.Transaction;
using Ets.Dao.Finance;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using Ets.Dao.Business;
using Config = ETS.Config;
using ETS.Library.Pay.WxPay;
using System.Web;

namespace Ets.Service.Provider.Pay
{
    public class PayProvider : IPayProvider
    {
        //AlipayIntegrate alipayIntegrate = new AlipayIntegrate();
        OrderChildDao orderChildDao = new OrderChildDao();
        private IBusinessFinanceProvider iBusinessFinanceProvider = new BusinessFinanceProvider();
        private IClienterFinanceProvider iClienterFinanceProvider = new ClienterFinanceProvider();
        private YeePayRecordDao yeePayRecordDao = new YeePayRecordDao();
        private QueryBalance queryBalance = new QueryBalance();
        private ClienterFinanceDao clienterFinanceDao = new ClienterFinanceDao();
        private BusinessFinanceDao businessFinanceDao = new BusinessFinanceDao();
        private BusinessFinanceProvider businessFinanceProvider = new BusinessFinanceProvider();
        private ClienterFinanceProvider clienterFinanceProvider = new ClienterFinanceProvider();
        private readonly BusinessFinanceAccountDao businessFinanceAccountDao = new BusinessFinanceAccountDao();
        private readonly ClienterFinanceAccountDao clienterFinanceAccountDao = new ClienterFinanceAccountDao();
        #region 生成支付宝、微信二维码订单

        /// <summary>
        /// 生成支付宝订单
        /// 窦海超
        /// 2015年5月12日 14:35:05
        /// </summary>
        /// <param name="model"></param>       
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
                return CreateAliPayOrder(orderNo, payStatusModel.TotalPrice, model.orderId, model.payStyle);
                //}
            }
            if (model.payType == PayTypeEnum.WeiXin.GetHashCode())
            {
                //微信支付
                LogHelper.LogWriter("=============微信支付：");
                return CreateWxPayOrder(orderNo, payStatusModel.TotalPrice, model.orderId, model.payStyle);

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

            Task.Factory.StartNew(() =>
            {
                JPushModel jpushModel = new JPushModel()
                {
                    Alert = "订单支付完成！",
                    City = string.Empty,
                    Content = string.Concat(model.orderId, "_", model.orderChildId, "_", orderChildPayModel.PayStatus),
                    RegistrationId = orderChildPayModel.clienterId.ToString(),//通过订单ID获取要发送的骑士ID
                    TagId = 0,
                    Title = "订单提醒"
                };
                Ets.Service.Provider.MyPush.Push.PushMessage(jpushModel);
            });
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
                //if (model.payType == PayTypeEnum.ZhiFuBao.GetHashCode())
                //{
                return new OrderChildDao().CheckOrderChildPayStatus(model.orderId);
                //return alipayIntegrate.GetOrder(orderNo, model.orderId, model.childId, unfinish);
                //return null;
                //}
                //微信
                //if (model.payType == PayTypeEnum.WeiXin.GetHashCode())
                //{
                //    WXpayService wxpay = new WXpayService();
                //    return wxpay.GetOrder(orderNo);
                //}
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
        private ResultModel<PayResultModel> CreateAliPayOrder(string orderNo, decimal payAmount, int orderId, int payStyle)
        {
            #region 通过订单ID，用于查询商家信息用
            BusinessDM businessModel = new BusinessDao().GetByOrderId(orderId);
            string businessName = string.Empty;
            if (businessModel == null || string.IsNullOrEmpty(businessModel.Name))
            {
                businessName = "E代送收款";
            }
            else
            {
                businessName = businessModel.Name;
            }
            #endregion
            PayResultModel resultModel = new PayResultModel();
            string qrcodeUrl = string.Empty;
            if (payStyle == 1)//用户扫二维码
            {
                //qrcodeUrl = alipayIntegrate.GetQRCodeUrl(orderNo, payAmount, businessName);
                AliModel aliModel = new AliModel()
                {
                    body = string.Empty,
                    orderNo = orderNo,
                    payMoney = payAmount,
                    productName = businessName
                };
                qrcodeUrl = new AliCallBack().GetOrder(aliModel);
                if (string.IsNullOrEmpty(qrcodeUrl))
                {
                    return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
                }
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
                        payType = PayTypeEnum.WeiXin.GetHashCode(),
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


        /// <summary>
        /// 商家充值
        /// 窦海超
        /// 2015年5月29日 15:09:29
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<BusinessRechargeResultModel> BusinessRecharge(BusinessRechargeModel model)
        {
            LogHelper.LogWriter("=============商家充值支付请求数据：", model);

            // string.Concat(model.productId, "_", model.orderId, "_", model.childId, "_", model.payStyle);

            #region 金额验证
            if (model.payAmount <= 0 || model.payAmount > 100000)
            {
                return ResultModel<BusinessRechargeResultModel>.Conclude(AliPayStatus.fail);
            }
            #endregion

            string orderNo = Helper.generateOrderCode(model.Businessid);
            BusinessRechargeResultModel resultModel = new BusinessRechargeResultModel()
            {
                notifyUrl = ETS.Config.NotifyUrl.Replace("Notify", "BusinessRechargeNotify"),
                orderNo = orderNo,
                payAmount = model.payAmount,
                PayType = model.PayType
            };
            //所属产品_主订单号_子订单号_支付方式

            return ResultModel<BusinessRechargeResultModel>.Conclude(AliPayStatus.success, resultModel);
        }


        /// <summary>
        /// 微信商家充值回调方法 
        /// 窦海超
        /// 2015年8月6日 23:06:02
        /// </summary>
        /// <returns></returns>
        public void BusinessRechargeWxNotify()
        {
            try
            {
                #region 参数绑定

                //var request = System.Web.HttpContext.Current.Request;
                //AlipayNotifyData notify = new AlipayNotifyData();
                //notify.buyer_email = request["buyer_email"];
                //notify.trade_status = request["trade_status"];
                //notify.out_trade_no = request["out_trade_no"];
                //notify.trade_no = request["trade_no"];
                //notify.total_fee = ParseHelper.ToDecimal(request["total_fee"], 0);
                //notify.out_biz_no = ParseHelper.ToInt(request["body"], 0);//businessid
                WxNotifyResultModel notify = new ResultNotify().ProcessNotify();
                string errmsg = "<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[{0}]]></return_msg></xml>";
                #endregion
                //如果状态为空或状态不等于同步成功和异步成功就认为是错误

                #region 回调完成状态
                if (notify.return_code == "SUCCESS")
                {
                    string ordermsg = notify.order_no;//商家ID_充值单ID
                    if (ordermsg.Contains("_"))
                    {

                        HttpContext.Current.Response.Write(string.Format(errmsg, "回调的数据有问题，不存在下划线"));
                        HttpContext.Current.Response.End();
                    }
                    if (new BusinessRechargeDao().Check(notify.order_no))
                    {
                        //如果存在就退出，这里写的很扯，因为支付宝要的是success不带双引号.
                        //但WEBAPI直接返回时带引号，所以现在要去库里查一次。
                        //回头找到原因一定要改
                        HttpContext.Current.Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
                        HttpContext.Current.Response.End();
                    }

                    int businessid = ParseHelper.ToInt(ordermsg.Split('_')[0]);
                    string orderno = ordermsg.Split('_')[1];
                    Ets.Model.DataModel.Business.BusinessRechargeModel businessRechargeModel = new Ets.Model.DataModel.Business.BusinessRechargeModel()
                    {
                        BusinessId = businessid,
                        OrderNo = orderno,
                        OriginalOrderNo = notify.order_no,//第三方的订单号
                        PayAmount = ParseHelper.ToDecimal(ParseHelper.ToInt(notify.total_fee) / 100),
                        PayBy = notify.openid,
                        PayStatus = 1,
                        PayType = PayTypeEnum.WeiXin.GetHashCode()
                    };
                    BusinessRechargeSusess(businessRechargeModel);
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "Alipay自动返回异常");
                HttpContext.Current.Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 商家充值回调方法 
        /// 窦海超
        /// 2015年5月29日 15:17:07
        /// </summary>
        /// <returns></returns>
        public dynamic BusinessRechargeNotify()
        {
            try
            {
                #region 参数绑定

                var request = System.Web.HttpContext.Current.Request;
                AlipayNotifyData notify = new AlipayNotifyData();
                notify.buyer_email = request["buyer_email"];
                notify.trade_status = request["trade_status"];
                notify.out_trade_no = request["out_trade_no"];
                notify.trade_no = request["trade_no"];
                notify.total_fee = ParseHelper.ToDecimal(request["total_fee"], 0);
                notify.out_biz_no = ParseHelper.ToInt(request["body"], 0);//businessid
                #endregion
                //如果状态为空或状态不等于同步成功和异步成功就认为是错误
                if (string.IsNullOrEmpty(notify.trade_status) || notify.total_fee <= 0)
                {
                    string fail = string.Concat("商家充值状态是空或金额<=0");
                    LogHelper.LogWriter(fail);
                    return "fail";
                }
                #region 回调完成状态
                if (notify.trade_status == "TRADE_SUCCESS" || notify.trade_status == "TRADE_FINISHED")
                {
                    if (new BusinessRechargeDao().Check(notify.trade_no))
                    {
                        //如果存在就退出，这里写的很扯，因为支付宝要的是success不带双引号.
                        //但WEBAPI直接返回时带引号，所以现在要去库里查一次。
                        //回头找到原因一定要改
                        return "success";
                    }

                    string orderNo = notify.out_trade_no;
                    if (string.IsNullOrEmpty(orderNo) || notify.out_biz_no <= 0)
                    {
                        string fail = string.Concat("商家充值错误啦orderNo：", orderNo, "，商家ID为：", notify.out_biz_no);
                        LogHelper.LogWriter(fail);
                        return "fail";
                    }

                    Ets.Model.DataModel.Business.BusinessRechargeModel businessRechargeModel = new Ets.Model.DataModel.Business.BusinessRechargeModel()
                    {
                        BusinessId = notify.out_biz_no,
                        OrderNo = orderNo,
                        OriginalOrderNo = notify.trade_no,//第三方的订单号
                        PayAmount = notify.total_fee,
                        PayBy = notify.buyer_email,
                        PayStatus = 1,
                        PayType = 1
                    };
                    BusinessRechargeSusess(businessRechargeModel);
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


        /// <summary>
        /// 商家充值成功方法
        /// 窦海超 
        /// 2015年5月29日 16:00:07
        /// </summary>
        /// <param name="model"></param>
        private dynamic BusinessRechargeSusess(Ets.Model.DataModel.Business.BusinessRechargeModel model)
        {
            #region 充值、流水、商家金额实体组装
            BusinessBalanceRecord businessBalanceRecord = new BusinessBalanceRecord()
            {
                Amount = model.PayAmount,
                BusinessId = model.BusinessId,
                Operator = model.PayBy,
                RecordType = BusinessBalanceRecordRecordType.Recharge.GetHashCode(),
                RelationNo = model.OrderNo,
                Remark = "商家客户端充值",
                Status = 1,
                WithwardId = 0
            };
            UpdateForWithdrawPM forWithdrawPM = new UpdateForWithdrawPM()
            {
                Id = model.BusinessId,
                Money = model.PayAmount
            };
            #endregion

            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                new BusinessRechargeDao().Insert(model);//写入充值 
                new BusinessDao().UpdateForWithdrawC(forWithdrawPM); //更新商家金额、可提现金额
                new BusinessBalanceRecordDao().Insert(businessBalanceRecord);//写商家流水                
                tran.Complete();
            }
            #region jpush推送

            Task.Factory.StartNew(() =>
            {
                JPushModel jpushModel = new JPushModel()
                {
                    Alert = string.Concat("已成功充值", model.PayAmount, "元"),
                    City = string.Empty,
                    RegistrationId = model.BusinessId.ToString(),//发送商家ID
                    TagId = 1,
                    Title = "充值成功提醒"
                };
                Ets.Service.Provider.MyPush.Push.PushMessage(jpushModel);
            });
            #endregion

            string success = string.Concat("成功完成商家充值订单号:", model.OrderNo);
            LogHelper.LogWriter(success);
            return "success";
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
        public ResultModel<PayResultModel> CreateWxPayOrder(string orderNo, decimal totalPrice, int orderId, int payStyle)
        {
            //支付方式-主订单ID-子订单ID
            PayResultModel resultModel = new PayResultModel();

            string code_url = string.Empty;
            if (payStyle == 1)//用户扫二维码
            {
                NativePay nativePay = new NativePay();
                code_url = nativePay.GetPayUrl(orderNo, totalPrice, "E代送收款", Config.WXNotifyUrl);
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
        public dynamic WxNotify()
        {

            #region 参数绑定
            WxNotifyResultModel notify = new ResultNotify().ProcessNotify();
            string errmsg = "<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[{0}]]></return_msg></xml>";
            #region 回调完成状态
            if (notify.return_code == "SUCCESS")
            {
                string orderNo = notify.order_no;
                if (string.IsNullOrEmpty(orderNo) || !orderNo.Contains("_"))
                {
                    string fail = string.Concat("错误啦orderNo：", orderNo);
                    LogHelper.LogWriter(fail);
                    HttpContext.Current.Response.Write(string.Format(errmsg, fail));
                    HttpContext.Current.Response.End();
                }
                int productId = ParseHelper.ToInt(orderNo.Split('_')[0]);//产品编号
                int orderId = ParseHelper.ToInt(orderNo.Split('_')[1]);//主订单号
                int orderChildId = ParseHelper.ToInt(orderNo.Split('_')[2]);//子订单号
                int payStyle = ParseHelper.ToInt(orderNo.Split('_')[3]);//支付方式(1 用户支付 2 骑士代付)
                if (orderId <= 0 || orderChildId <= 0)
                {
                    string fail = string.Concat("错误啦orderId：", orderId, ",orderChildId:", orderChildId);
                    LogHelper.LogWriter(fail);
                    HttpContext.Current.Response.Write(string.Format(errmsg, fail));
                    HttpContext.Current.Response.End();
                }

                OrderChildFinishModel model = new OrderChildFinishModel()
                {
                    orderChildId = orderChildId,
                    orderId = orderId,
                    payBy = notify.openid,
                    payStyle = payStyle,
                    payType = PayTypeEnum.ZhiFuBao.GetHashCode(),
                    originalOrderNo = notify.transaction_id,
                };

                if (orderChildDao.FinishPayStatus(model))
                {
                    //jpush
                    //Ets.Service.Provider.MyPush.Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", myorder.businessId.ToString(), string.Empty);
                    FinishOrderPushMessage(model);//完成后发送jpush消息
                    string success = string.Concat("成功，当前订单OrderId:", orderId, ",OrderChild:", orderChildId);
                    LogHelper.LogWriter(success);
                    HttpContext.Current.Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
                    HttpContext.Current.Response.End();
                }
            }
            #endregion
            LogHelper.LogWriter("支付回调异常", notify);
            HttpContext.Current.Response.Write(errmsg);
            HttpContext.Current.Response.End();
            #endregion

            return null;
        }

        #endregion


        #region 易宝相关
        /// <summary>
        /// 易宝转账回调接口
        /// </summary>
        /// <param name="data"></param>

        public bool YeePayCashTransferCallback(string data)
        {
            bool result = false;
            string username = "易宝提现回调";
            CashTransferCallback model = JsonHelper.JsonConvertToObject<CashTransferCallback>(ResponseYeePay.OutRes(data, true));
            YeePayRecordDao yeePayRecordDao = new YeePayRecordDao();
            YeePayRecord yeePayRecordDbModel = yeePayRecordDao.GetReocordByRequestId(model.cashrequestid);
            if (yeePayRecordDbModel == null)
            {
                EmailHelper.SendEmailTo(string.Format("易宝请求号为{0}的提现单回调失败，数据库提示无该提现记录，回调的完整参数为{1}",
                    model.cashrequestid, ResponseYeePay.OutRes(data, true)), ConfigSettings.Instance.EmailToAdress);
                return false;
            }
            long withwardId = yeePayRecordDbModel.WithdrawId; //提现单id
            yeePayRecordDao.Insert(new YeePayRecord()
            {
                WithdrawId = withwardId,
                RequestId = model.cashrequestid,
                CustomerNumber = model.customernumber,
                Ledgerno = model.ledgerno,
                Amount = model.amount,
                Status = model.status,
                Lastno = model.lastno,
                Desc = model.desc,
                TransferType = TransferTypeYee.CallBack.GetHashCode(),
                UserType = yeePayRecordDbModel.UserType
            });

            if (model.status == "SUCCESS") //提现成功 走 成功的逻辑
            {
                if (yeePayRecordDbModel.UserType == UserTypeYee.Business.GetHashCode()) //B端逻辑
                {
                    result = iBusinessFinanceProvider.BusinessWithdrawPayOk(new BusinessWithdrawLog()
                    {
                        Operator = username,
                        Remark = "易宝提现打款成功" + model.desc,
                        OldStatus = BusinessWithdrawFormStatus.Paying.GetHashCode(),
                        Status = BusinessWithdrawFormStatus.Success.GetHashCode(),
                        WithwardId = withwardId
                    });
                }
                else if (yeePayRecordDbModel.UserType == UserTypeYee.Clienter.GetHashCode()) //C端逻辑
                {
                    result = iClienterFinanceProvider.ClienterWithdrawPayOk(new ClienterWithdrawLog()
                    {
                        Operator = username,
                        Remark = "易宝提现打款成功" + model.desc,
                        Status = ClienterWithdrawFormStatus.Success.GetHashCode(),
                        OldStatus = ClienterWithdrawFormStatus.Paying.GetHashCode(),
                        WithwardId = withwardId
                    });
                }
            }
            else if (model.status == "FAIL") //提现失败 走 失败的逻辑
            {
                if (yeePayRecordDbModel.UserType == UserTypeYee.Business.GetHashCode()) //B端逻辑
                {
                    result = iBusinessFinanceProvider.BusinessWithdrawPayFailed(new BusinessWithdrawLogModel()
                    {
                        Operator = username,
                        Remark = "易宝提现打款失败，" + model.desc,
                        Status = BusinessWithdrawFormStatus.Error.GetHashCode(),
                        OldStatus = BusinessWithdrawFormStatus.Paying.GetHashCode(),
                        WithwardId = withwardId,
                        PayFailedReason = ""
                    }, model); //商户提现失败
                }
                else if (yeePayRecordDbModel.UserType == UserTypeYee.Clienter.GetHashCode()) //C端逻辑
                {
                    result = iClienterFinanceProvider.ClienterWithdrawPayFailed(new ClienterWithdrawLogModel()
                    {
                        Operator = username,
                        Remark = "易宝提现打款失败，" + model.desc,
                        Status = ClienterWithdrawFormStatus.Error.GetHashCode(),
                        OldStatus = ClienterWithdrawFormStatus.Paying.GetHashCode(),
                        WithwardId = withwardId,
                        PayFailedReason = ""
                    }, model);
                }
            }
            if (!result)
            {
                EmailHelper.SendEmailTo(string.Format("易宝请求号为{0}的提现单回调失败，提现单号为{1}，提现单数据库出现数据异常",
                   model.cashrequestid, withwardId.ToString()), ConfigSettings.Instance.EmailToAdress);
            }
            return result;

        }

        /// <summary> 
        /// 注册易宝子账户 add by caoheyang 20150722
        /// </summary>
        /// <param name="para"></param>
        public RegisterReturnModel RegisterYee(YeeRegisterParameter para)
        {
            Register regisiter = new Register();
            RegisterReturnModel retunModel = regisiter.RegSubaccount(para);
            if (retunModel != null && retunModel.code == "1")  //易宝返回成功 记录所有当前请求相关的数据
            {
                new YeePayUserDao().Insert(TranslateRegisterYeeModel(para));
            }
            return retunModel;
        }

        /// <summary>
        /// 根据易宝注册参数 转  YeePayUser 实体 add by caoheyang 20150722
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private YeePayUser TranslateRegisterYeeModel(YeeRegisterParameter para)
        {
            return new YeePayUser()
            {
                UserId = para.UserId,
                UserType = para.UserType,
                RequestId = para.RequestId,
                CustomerNumberr = para.CustomerNumberr,
                HmacKey = para.HmacKey,
                BindMobile = para.BindMobile,
                CustomerType = para.CustomerType.ToString(),
                SignedName = para.SignedName,
                LinkMan = para.LinkMan,
                IdCard = para.IdCard,
                BusinessLicence = para.BusinessLicence,
                LegalPerson = para.LegalPerson,
                MinsettleAmount = para.MinsettleAmount,
                Riskreserveday = para.RiskReserveday,
                BankAccountNumber = DES.Encrypt(para.BankAccountNumber),
                BankName = para.BankName,
                AccountName = para.AccountName,
                BankAccountType = para.BankAccountType,
                BankProvince = para.BankProvince,
                BankCity = para.BankCity,
                ManualSettle = para.ManualSettle,
                Hmac = para.Hmac,
                Ledgerno = para.Ledgerno
            };
        }

        /// <summary>
        /// 易宝提现  add by caoheyang 20150722
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TransferReturnModel CashTransferYee(YeeCashTransferParameter model)
        {
            Transfer transfer = new Transfer();
            TransferReturnModel retunModel = transfer.CashTransfer(ref model);
            if (retunModel != null && retunModel.code == "1")  //易宝返回成功 记录所有当前请求相关的数据
            {
                new YeePayRecordDao().Insert(CashTransferYeeModel(model, retunModel));
            }
            return retunModel;
        }


        /// <summary>
        /// 根据易宝发起提现参数 转  YeePayRecord 实体 add by caoheyang 20150722
        /// </summary>
        /// <param name="model"></param>
        /// <param name="retunModel"></param>
        /// <returns></returns>
        private YeePayRecord CashTransferYeeModel(YeeCashTransferParameter model, TransferReturnModel retunModel)
        {
            return new YeePayRecord()
            {
                RequestId = model.RequestId,
                CustomerNumber = model.CustomerNumber,
                HmacKey = model.HmacKey,
                Ledgerno = model.Ledgerno,
                SourceLedgerno = "",
                Amount = model.Amount,
                TransferType = TransferTypeYee.Withdraw.GetHashCode(), //发起提现  
                Payer = PayerYee.Child.GetHashCode(),  //提现支出方是 1 子账户
                Code = retunModel.code,
                Hmac = model.Hmac,
                Msg = retunModel.msg,
                CallbackUrl = model.CallbackUrl,
                WithdrawId = model.WithdrawId,
                UserType = model.UserType
            };
        }

        /// <summary> 
        /// 易宝转账 add by caoheyang 20150722
        /// </summary> 
        /// <param name="para"></param>
        public TransferReturnModel TransferAccountsYee(YeeTransferParameter para)
        {
            Transfer transfer = new Transfer();
            TransferReturnModel retunModel = transfer.TransferAccounts(ref para);
            if (retunModel != null && retunModel.code == "1")  //易宝返回成功 记录所有当前请求相关的数据
            {
                new YeePayRecordDao().Insert(TransferYeeModel(para, retunModel));
            }
            return retunModel;
        }

        public QueryBalanceReturnModel QueryBalanceYee(YeeQueryBalanceParameter model)
        {
            model.CustomerNumber = KeyConfig.YeepayAccountId;//商户编号 
            model.HmacKey = KeyConfig.YeepayHmac;//密钥 
            return queryBalance.GetBalance(model);
        }

        /// <summary>
        /// 根据易宝转账参数 转  YeePayRecord 实体 add by caoheyang 20150722
        /// </summary>
        /// <param name="model"></param>
        /// <param name="retunModel"></param>
        /// <returns></returns>
        private YeePayRecord TransferYeeModel(YeeTransferParameter model, TransferReturnModel retunModel)
        {
            int payer = string.IsNullOrWhiteSpace(model.Ledgerno) && !string.IsNullOrWhiteSpace(model.SourceLedgerno)
                ? PayerYee.Child.GetHashCode()
                : PayerYee.Main.GetHashCode();//0 主账户 1 子账户
            return new YeePayRecord()
            {
                RequestId = model.RequestId,
                CustomerNumber = model.CustomerNumber,
                HmacKey = model.HmacKey,
                Ledgerno = model.Ledgerno,
                SourceLedgerno = model.SourceLedgerno,
                Amount = model.Amount,
                TransferType = TransferTypeYee.Transfer.GetHashCode(), //转账
                Payer = payer,
                Code = retunModel.code,
                Hmac = model.Hmac,
                Msg = retunModel.msg,
                CallbackUrl = "",
                WithdrawId = model.WithdrawId,
                UserType = model.UserType
            };
        }
        /// <summary>
        /// 易宝自动对账
        /// danny-20150730
        /// </summary>
        public void YeePayReconciliation()
        {
            #region 对象声明及实例化
            var sbEmail = new StringBuilder();
            var overTimeDateDiff = ParseHelper.ToInt(Config.ConfigKey("OverTimeDateDiff"));//易宝回调超时时间配置
            var activeDateDiff = ParseHelper.ToInt(Config.ConfigKey("ActiveDateDiff"));//易宝活跃用户时间配置
            var emailSendTo = Config.ConfigKey("EmailSendTo");
            var copyTo = Config.ConfigKey("CopyTo");
            var yeePayUserList = yeePayRecordDao.GetYeePayUserList(activeDateDiff);
            #endregion

            #region 同步活跃易宝账户余额
            if (yeePayUserList != null && yeePayUserList.Count > 0)
            {
                LogHelper.LogWriter("活跃用户账户余额同步开始:" + DateTime.Now);
                foreach (var yeePayUser in yeePayUserList)
                {
                    var reg = QueryBalanceYee(new YeeQueryBalanceParameter() { Ledgerno = yeePayUser.Ledgerno });

                    if (reg.code != "1")
                    {
                        sbEmail.AppendLine("调用易宝余额查询接口失败：" + reg.msg + "(" + reg.code + ")");
                        EmailHelper.SendEmailTo(sbEmail.ToString(), emailSendTo, "易宝对账结果", copyTo, false);
                        return;
                    }
                    var yeeBalance = ParseHelper.ToDecimal(reg.ledgerbalance.Contains(":") ? reg.ledgerbalance.Split(':')[1] : "0");
                    if (yeeBalance != yeePayUser.YeeBalance)
                    {
                        LogHelper.LogWriter("易宝用户余额同步开始:" + DateTime.Now);
                        yeePayRecordDao.ModifyYeeBalance(new YeePayUser()
                        {
                            Ledgerno = yeePayUser.Ledgerno,
                            YeeBalance = yeeBalance
                        });
                        LogHelper.LogWriter("易宝用户余额同步结束:" + DateTime.Now);
                    }
                }
                LogHelper.LogWriter("活跃用户账户余额同步结束:" + DateTime.Now);
            }
            #endregion

            #region 获取预警数据及处理
            var exceptYeePayUser = yeePayRecordDao.GetBalanceExceptYeePayUserList();
            var varnClienterWithdrawForm = yeePayRecordDao.GetWarnClienterWithdrawForm(overTimeDateDiff);
            var varnBusinessWithdrawForm = yeePayRecordDao.GetWarnBusinessWithdrawForm(overTimeDateDiff);
            #region 账户余额异常
            if (exceptYeePayUser != null && exceptYeePayUser.Count > 0)
            {
                var yeeBalanceDiff = exceptYeePayUser.Where(t => t.YeeBalance != t.BalanceRecord).ToList();
                var yeeBalanceExcpt = exceptYeePayUser.Where(t => t.YeeBalance > 0).ToList();
                #region 易宝账户本系统余额和易宝系统不一致
                if (yeeBalanceDiff.Count > 0)
                {
                    LogHelper.LogWriter("易宝账户本系统余额和易宝系统不一致数据获取开始:" + DateTime.Now);
                    sbEmail.AppendLine("易宝账户本系统余额和易宝系统不一致：");
                    foreach (var item in yeeBalanceDiff)
                    {
                        sbEmail.AppendLine("易宝账户:【" + item.Ledgerno + "】,本系统账户余额：【" + item.BalanceRecord + "元】,易宝系统余额：【" +
                                           item.YeeBalance + "元】");
                    }
                    sbEmail.AppendLine();
                    LogHelper.LogWriter("易宝账户本系统余额和易宝系统不一致数据获取结束:" + DateTime.Now);
                }
                #endregion
                #region 易宝子账户余额大于0
                if (yeeBalanceExcpt.Count > 0)
                {
                    LogHelper.LogWriter("易宝子账户余额大于0数据获取开始:" + DateTime.Now);
                    sbEmail.AppendLine("易宝子账户余额大于0：");
                    foreach (var item in yeeBalanceExcpt)
                    {
                        sbEmail.AppendLine("易宝账户:【" + item.Ledgerno + "】,易宝系统余额：【" + item.YeeBalance + "元】");
                        //反转账
                        var regRTransfer = new PayProvider().TransferAccountsYee(new YeeTransferParameter()
                        {
                            UserType = item.UserType,
                            WithdrawId = ParseHelper.ToLong(item.UserType + TimeHelper.GetTimeStamp(false) + item.UserId),
                            Ledgerno = "",
                            SourceLedgerno = item.Ledgerno,
                            Amount = item.YeeBalance.ToString()
                        });
                        if (regRTransfer != null && !string.IsNullOrEmpty(regRTransfer.code) && regRTransfer.code.Trim() == "1")
                        {
                            clienterFinanceDao.AddYeePayUserBalanceRecord(new YeePayUserBalanceRecord()
                            {
                                LedgerNo = item.Ledgerno,
                                WithwardId = ParseHelper.ToLong(item.UserType + TimeHelper.GetTimeStamp(false) + item.UserId),
                                Amount = item.YeeBalance,
                                Balance = 0,
                                RecordType = YeeRecordType.C2P.GetHashCode(),
                                Operator = "自动服务",
                                Remark = "易宝子账户向主账户反转【" + item.YeeBalance + "】元"
                            });
                            yeePayRecordDao.ModifyYeeBalance(new YeePayUser()
                            {
                                Ledgerno = item.Ledgerno,
                                YeeBalance = 0
                            });
                        }
                    }
                    sbEmail.AppendLine();
                    LogHelper.LogWriter("易宝子账户余额大于0数据获取结束:" + DateTime.Now);
                }
                #endregion
            }
            #endregion
            #region 骑士提款单异常
            if (varnClienterWithdrawForm != null && varnClienterWithdrawForm.Count > 0)
            {
                var exceptClienterWithdrawForm = varnClienterWithdrawForm.Where(t => t.Status == ClienterWithdrawFormStatus.Except.GetHashCode()).ToList();
                var overtimeClienterWithdrawForm = varnClienterWithdrawForm.Where(t => t.Status == ClienterWithdrawFormStatus.Paying.GetHashCode()).ToList();
                if (exceptClienterWithdrawForm.Count > 0)//单据异常
                {
                    LogHelper.LogWriter("异常骑士提现单获取获取开始:" + DateTime.Now);
                    sbEmail.AppendLine("骑士提现单状态异常：");
                    foreach (var item in exceptClienterWithdrawForm)
                    {
                        sbEmail.AppendLine("提现单号:【" + item.WithwardNo + "】,提现单状态：【" + ETS.Extension.EnumExtenstion.GetEnumItem(
                            ((ClienterWithdrawFormStatus)item.Status).GetType(), (ClienterWithdrawFormStatus)item.Status).Text + "】,异常描述：【" +
                                           item.PayFailedReason + "】");
                    }
                    sbEmail.AppendLine();
                    LogHelper.LogWriter("异常骑士提现单获取获取结束:" + DateTime.Now);
                }
                if (overtimeClienterWithdrawForm.Count > 0)//回调超时
                {
                    LogHelper.LogWriter("回调超时骑士提现单获取获取开始:" + DateTime.Now);
                    sbEmail.AppendLine("骑士提现单超时：");
                    foreach (var item in overtimeClienterWithdrawForm)
                    {
                        sbEmail.AppendLine("提现单号:【" + item.WithwardNo + "】,提现单状态：【" + ETS.Extension.EnumExtenstion.GetEnumItem(
                            ((ClienterWithdrawFormStatus)item.Status).GetType(), (ClienterWithdrawFormStatus)item.Status).Text + "】,超时时间：【" +
                                           item.DateDiff + "天】");
                    }
                    sbEmail.AppendLine();
                    LogHelper.LogWriter("回调超时骑士提现单获取获取结束:" + DateTime.Now);
                }
            }
            #endregion
            #region 商户提款单异常
            if (varnBusinessWithdrawForm != null && varnBusinessWithdrawForm.Count > 0)
            {
                var exceptBusinessWithdrawForm = varnBusinessWithdrawForm.Where(t => t.Status == BusinessWithdrawFormStatus.Except.GetHashCode()).ToList();
                var overtimeBusinessWithdrawForm = varnBusinessWithdrawForm.Where(t => t.Status == BusinessWithdrawFormStatus.Paying.GetHashCode()).ToList();
                if (exceptBusinessWithdrawForm.Count > 0)//单据异常
                {
                    LogHelper.LogWriter("异常商户提现单获取获取开始:" + DateTime.Now);
                    sbEmail.AppendLine("商户提现单状态异常：");
                    foreach (var item in exceptBusinessWithdrawForm)
                    {
                        sbEmail.AppendLine("提现单号:【" + item.WithwardNo + "】,提现单状态：【" + ETS.Extension.EnumExtenstion.GetEnumItem(
                            ((BusinessWithdrawFormStatus)item.Status).GetType(), (BusinessWithdrawFormStatus)item.Status).Text + "】,异常描述：【" +
                                           item.PayFailedReason + "】");
                    }
                    sbEmail.AppendLine();
                    LogHelper.LogWriter("异常商户提现单获取获取结束:" + DateTime.Now);
                }
                if (overtimeBusinessWithdrawForm.Count > 0)//回调超时
                {
                    LogHelper.LogWriter("回调超时商户提现单获取获取开始:" + DateTime.Now);
                    sbEmail.AppendLine("商户提现单超时：");
                    foreach (var item in overtimeBusinessWithdrawForm)
                    {
                        sbEmail.AppendLine("提现单号:【" + item.WithwardNo + "】,提现单状态：【" + ETS.Extension.EnumExtenstion.GetEnumItem(
                            ((BusinessWithdrawFormStatus)item.Status).GetType(), (BusinessWithdrawFormStatus)item.Status).Text + "】,超时时间：【" +
                                           item.DateDiff + "天】");
                    }
                    sbEmail.AppendLine();
                    LogHelper.LogWriter("回调超时商户提现单获取获取结束:" + DateTime.Now);
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(sbEmail.ToString()))
            {
                LogHelper.LogWriter(sbEmail.ToString() + DateTime.Now);
                EmailHelper.SendEmailTo(sbEmail.ToString(), emailSendTo, "易宝自动对账", copyTo, false);
            }
            #endregion
        }
        /// <summary>
        ///自动处理提款申请单（原确认打款逻辑）
        /// danny-20150804
        /// </summary>
        public void AutoDealWithdrawForm()
        {
            #region 对象声明及实例化
            var emailSendTo = Config.ConfigKey("EmailSendTo");
            var copyTo = Config.ConfigKey("CopyTo");
            #endregion

            #region 处理商户提现单
            Task.Factory.StartNew(() =>
            {
                var bfaList = businessFinanceDao.GetBusinessFinanceAccountList();
                if (bfaList != null && bfaList.Count > 0)
                {
                    foreach (var item in bfaList)
                    {
                        #region 对象声明及实例化
                        if (businessFinanceDao.ModifyBusinessWithdrawDealCount(item.WithwardId) > 5)
                        {
                            businessFinanceDao.BusinessWithdrawPayFailed(new BusinessWithdrawLogModel()
                            {
                                Status = BusinessWithdrawFormStatus.Except.GetHashCode(),
                                OldStatus = BusinessWithdrawFormStatus.Paying.GetHashCode(),
                                Operator = "自动处理提现服务",
                                Remark = "处理次数超限",
                                PayFailedReason = "处理次数超限",
                                WithwardId = item.WithwardId
                            });
                            EmailHelper.SendEmailTo("商户提现单自动处理提现次数超限，单号为【" + item.WithwardNo + "】", emailSendTo, "自动处理商户提现单异常", copyTo, false);
                            continue;
                        }
                        string key = string.Format(RedissCacheKey.Ets_Withdraw_Deal_B, item.WithwardId);
                        var redis = new ETS.NoSql.RedisCache.RedisCache();
                        var dealStatus = ParseHelper.ToInt(redis.Get<int>(key));
                        var amount = item.HandChargeOutlay == 0 ? item.Amount : item.Amount + item.HandCharge;//转账及提现金额（计算手续费）

                        #endregion

                        #region 初始值
                        if (dealStatus == WithdrawDealStatus.Default.GetHashCode())
                        {
                            if (string.IsNullOrEmpty(item.YeepayKey) || item.YeepayStatus == 1)//无易宝账户或账户信息有更新
                            {
                                var registResult = new PayProvider().RegisterYee(new YeeRegisterParameter
                                {
                                    BindMobile = item.PhoneNo,
                                    SignedName = item.TrueName,
                                    CustomerType = item.BelongType == 0 ? CustomertypeEnum.PERSON : CustomertypeEnum.ENTERPRISE,
                                    LinkMan = item.TrueName,
                                    IdCard = item.IDCard,
                                    BusinessLicence = item.IDCard,
                                    LegalPerson = item.TrueName,
                                    BankAccountNumber = ParseHelper.ToDecrypt(item.AccountNo),
                                    BankName = item.OpenBank,
                                    AccountName = item.TrueName,
                                    BankProvince = item.OpenProvince,
                                    BankCity = item.OpenCity,
                                    UserId = item.BusinessId,
                                    UserType = UserTypeYee.Business.GetHashCode(),
                                    AccountId = item.Id.ToString()
                                });//注册帐号
                                if (registResult != null && !string.IsNullOrEmpty(registResult.code) && registResult.code.Trim() == "1") //调用易宝注册接口成功
                                {
                                    dealStatus = WithdrawDealStatus.Registering.GetHashCode();
                                    redis.Set(key, dealStatus.GetHashCode());
                                    item.YeepayKey = registResult.ledgerno;
                                }
                                else
                                {
                                    DealRegisterYeeFailedB(registResult, item);
                                    continue; //跳出此次循环
                                }
                            }
                            else//有易宝账户且无账户信息更新
                            {
                                dealStatus = WithdrawDealStatus.Registered.GetHashCode();
                                redis.Set(key, dealStatus.GetHashCode());
                            }
                        }
                        #endregion

                        #region 待注册
                        if (dealStatus == WithdrawDealStatus.Registering.GetHashCode())
                        {
                            if (!businessFinanceAccountDao.ModifyYeepayInfoById(Convert.ToInt32(item.Id), item.YeepayKey, 0))
                            {
                                continue;
                            }
                            dealStatus = WithdrawDealStatus.Registered.GetHashCode();
                            redis.Set(key, dealStatus.GetHashCode());
                        }

                        #endregion

                        #region 已注册
                        if (dealStatus == WithdrawDealStatus.Registered.GetHashCode())
                        {
                            var regTransfer = new PayProvider().TransferAccountsYee(new YeeTransferParameter()
                            {
                                UserType = UserTypeYee.Business.GetHashCode(),
                                WithdrawId = item.WithwardId,
                                Ledgerno = item.YeepayKey,
                                SourceLedgerno = "",
                                Amount = amount.ToString()
                            });
                            if (regTransfer.code != "1")
                            {
                                businessFinanceDao.ModifyBusinessWithdrawPayFailedReason(new BusinessWithdrawLogModel()
                                {
                                    WithwardId = item.WithwardId,
                                    PayFailedReason = "转:" + regTransfer.msg + "(" + regTransfer.code + ")",
                                    Remark = "转:" + regTransfer.msg + "(" + regTransfer.code + ")",
                                    Operator = "自动处理服务"
                                });
                                continue;
                            }
                            dealStatus = WithdrawDealStatus.Transfering.GetHashCode();
                            redis.Set(key, dealStatus.GetHashCode());
                        }
                        #endregion

                        #region 待转账
                        if (dealStatus == WithdrawDealStatus.Transfering.GetHashCode())
                        {
                            if (!DealTransferAccountsYeeOk(item, amount))
                            {
                                continue;
                            }
                            dealStatus = WithdrawDealStatus.Transfered.GetHashCode();
                            redis.Set(key, dealStatus.GetHashCode());
                        }
                        #endregion

                        #region 已转账
                        if (dealStatus == WithdrawDealStatus.Transfered.GetHashCode())
                        {
                            var regCash = new PayProvider().CashTransferYee(new YeeCashTransferParameter()
                            {
                                UserType = UserTypeYee.Business.GetHashCode(),
                                WithdrawId = ParseHelper.ToInt(item.WithwardId),
                                Ledgerno = item.YeepayKey,
                                App = APP.B,
                                Amount = amount.ToString()
                            });
                            if (regCash.code != "1")
                            {
                                businessFinanceDao.ModifyBusinessWithdrawPayFailedReason(new BusinessWithdrawLogModel()
                                {
                                    WithwardId = item.WithwardId,
                                    PayFailedReason = "提:" + regCash.msg + "(" + regCash.code + ")",
                                    Remark = "提:" + regCash.msg + "(" + regCash.code + ")",
                                    Operator = "自动处理服务"
                                });
                                continue;
                            }
                            dealStatus = WithdrawDealStatus.Cashing.GetHashCode();
                            redis.Set(key, dealStatus.GetHashCode());
                        }
                        #endregion

                        #region 待提现
                        if (dealStatus == WithdrawDealStatus.Cashing.GetHashCode())
                        {
                            if (!DealCashTransferYeeOkB(item, amount))
                            {
                                continue;
                            }
                            dealStatus = WithdrawDealStatus.Cashed.GetHashCode();
                            redis.Set(key, dealStatus.GetHashCode());
                        }
                        #endregion
                    }
                }
            });
            #endregion

            #region 处理骑士提现单
            Task.Factory.StartNew(() =>
            {
                var cfaList = clienterFinanceDao.GetClienterFinanceAccountList();
                if (cfaList != null && cfaList.Count > 0)
                {
                    foreach (var item in cfaList)
                    {
                        #region 对象声明及实例化
                        if (clienterFinanceDao.ModifyClienterWithdrawDealCount(item.WithwardId) > 5)
                        {
                            clienterFinanceDao.ClienterWithdrawPayFailed(new ClienterWithdrawLogModel()
                            {
                                Status = ClienterWithdrawFormStatus.Except.GetHashCode(),
                                OldStatus = ClienterWithdrawFormStatus.Paying.GetHashCode(),
                                Operator = "自动处理提现服务",
                                Remark = "处理次数超限",
                                PayFailedReason = "处理次数超限",
                                WithwardId = item.WithwardId
                            });
                            EmailHelper.SendEmailTo("骑士提现单自动处理提现次数超限，单号为【" + item.WithwardNo + "】", emailSendTo, "自动处理骑士提现单异常", copyTo, false);
                            continue;
                        }
                        string key = string.Format(RedissCacheKey.Ets_Withdraw_Deal_C, item.WithwardId);
                        var redis = new ETS.NoSql.RedisCache.RedisCache();
                        var dealStatus = ParseHelper.ToInt(redis.Get<int>(key));
                        var amount = item.HandChargeOutlay == 0 ? item.Amount : item.Amount + item.HandCharge;//转账及提现金额（计算手续费）

                        #endregion

                        #region 初始值
                        if (dealStatus == WithdrawDealStatus.Default.GetHashCode())
                        {
                            if (string.IsNullOrEmpty(item.YeepayKey) || item.YeepayStatus == 1)//无易宝账户或账户信息有更新
                            {
                                var registResult = new PayProvider().RegisterYee(new YeeRegisterParameter
                                {
                                    BindMobile = item.PhoneNo,
                                    SignedName = item.TrueName,
                                    CustomerType = item.BelongType == 0 ? CustomertypeEnum.PERSON : CustomertypeEnum.ENTERPRISE,
                                    LinkMan = item.TrueName,
                                    IdCard = item.IDCard,
                                    BusinessLicence = item.IDCard,
                                    LegalPerson = item.TrueName,
                                    BankAccountNumber = ParseHelper.ToDecrypt(item.AccountNo),
                                    BankName = item.OpenBank,
                                    AccountName = item.TrueName,
                                    BankProvince = item.OpenProvince,
                                    BankCity = item.OpenCity,
                                    UserId = item.ClienterId,
                                    UserType = UserTypeYee.Clienter.GetHashCode(),
                                    AccountId = item.Id.ToString()
                                });//注册帐号
                                if (registResult != null && !string.IsNullOrEmpty(registResult.code) && registResult.code.Trim() == "1") //调用易宝注册接口成功
                                {
                                    dealStatus = WithdrawDealStatus.Registering.GetHashCode();
                                    redis.Set(key, dealStatus.GetHashCode());
                                    item.YeepayKey = registResult.ledgerno;
                                }
                                else
                                {
                                    DealRegisterYeeFailedC(registResult, item);
                                    continue; //跳出此次循环
                                }
                            }
                            else//有易宝账户且无账户信息更新
                            {
                                dealStatus = WithdrawDealStatus.Registered.GetHashCode();
                                redis.Set(key, dealStatus.GetHashCode());
                            }
                        }
                        #endregion

                        #region 待注册
                        if (dealStatus == WithdrawDealStatus.Registering.GetHashCode())
                        {
                            if (!clienterFinanceAccountDao.ModifyYeepayInfoById(Convert.ToInt32(item.Id), item.YeepayKey, 0))
                            {
                                continue;
                            }
                            dealStatus = WithdrawDealStatus.Registered.GetHashCode();
                            redis.Set(key, dealStatus.GetHashCode());
                        }

                        #endregion

                        #region 已注册
                        if (dealStatus == WithdrawDealStatus.Registered.GetHashCode())
                        {
                            var regTransfer = new PayProvider().TransferAccountsYee(new YeeTransferParameter()
                            {
                                UserType = UserTypeYee.Clienter.GetHashCode(),
                                WithdrawId = item.WithwardId,
                                Ledgerno = item.YeepayKey,
                                SourceLedgerno = "",
                                Amount = amount.ToString()
                            });
                            if (regTransfer.code != "1")
                            {
                                clienterFinanceDao.ModifyClienterWithdrawPayFailedReason(new ClienterWithdrawLogModel()
                                {
                                    WithwardId = item.WithwardId,
                                    PayFailedReason = "转:" + regTransfer.msg + "(" + regTransfer.code + ")",
                                    Remark = "转:" + regTransfer.msg + "(" + regTransfer.code + ")",
                                    Operator = "自动处理服务"
                                });
                                continue;
                            }
                            dealStatus = WithdrawDealStatus.Transfering.GetHashCode();
                            redis.Set(key, dealStatus.GetHashCode());
                        }
                        #endregion

                        #region 待转账
                        if (dealStatus == WithdrawDealStatus.Transfering.GetHashCode())
                        {
                            if (!DealTransferAccountsYeeOk(item, amount))
                            {
                                continue;
                            }
                            dealStatus = WithdrawDealStatus.Transfered.GetHashCode();
                            redis.Set(key, dealStatus.GetHashCode());
                        }
                        #endregion

                        #region 已转账
                        if (dealStatus == WithdrawDealStatus.Transfered.GetHashCode())
                        {
                            var regCash = new PayProvider().CashTransferYee(new YeeCashTransferParameter()
                            {
                                UserType = UserTypeYee.Clienter.GetHashCode(),
                                WithdrawId = ParseHelper.ToInt(item.WithwardId),
                                Ledgerno = item.YeepayKey,
                                App = APP.C,
                                Amount = amount.ToString()
                            });
                            if (regCash.code != "1")
                            {
                                clienterFinanceDao.ModifyClienterWithdrawPayFailedReason(new ClienterWithdrawLogModel()
                                {
                                    WithwardId = item.WithwardId,
                                    PayFailedReason = "提:" + regCash.msg + "(" + regCash.code + ")",
                                    Remark = "提:" + regCash.msg + "(" + regCash.code + ")",
                                    Operator = "自动处理服务"
                                });
                                continue;
                            }
                            dealStatus = WithdrawDealStatus.Cashing.GetHashCode();
                            redis.Set(key, dealStatus.GetHashCode());
                        }
                        #endregion

                        #region 待提现
                        if (dealStatus == WithdrawDealStatus.Cashing.GetHashCode())
                        {
                            if (!DealCashTransferYeeOkC(item, amount))
                            {
                                continue;
                            }
                            dealStatus = WithdrawDealStatus.Cashed.GetHashCode();
                            redis.Set(key, dealStatus.GetHashCode());
                        }
                        #endregion
                    }
                }
            });
            #endregion
        }
        /// <summary>
        /// 商户易宝账户注册失败
        /// danny-20150804
        /// </summary>
        private void DealRegisterYeeFailedB(RegisterReturnModel registResult, BusinessFinanceAccountModel model)
        {
            string payFailedReason;
            if (registResult == null)
            {
                payFailedReason = "商户绑定易宝支付失败,返回结果为null!";
            }
            else
            {
                LogHelper.LogWriterString("商户绑定易宝支付失败",
                    string.Format("易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}",
                        registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg));
                payFailedReason = string.Format("绑定失败:code{0},ledgerno:{1},hmac{2},msg{3}", registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg);
            }
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (businessFinanceAccountDao.ModifyYeepayInfoById(Convert.ToInt32(model.Id), "", 1))
                {
                    if (businessFinanceDao.ModifyBusinessWithdrawPayFailedReason(new BusinessWithdrawLogModel()
                    {
                        WithwardId = model.WithwardId,
                        PayFailedReason = payFailedReason,
                        Remark = payFailedReason,
                        Operator = "自动处理服务"
                    }))
                    {
                        tran.Complete();
                    }
                }
            }
        }
        /// <summary>
        /// 骑士易宝账户注册失败
        /// danny-20150804
        /// </summary>
        private void DealRegisterYeeFailedC(RegisterReturnModel registResult, ClienterFinanceAccountModel model)
        {
            string payFailedReason;
            if (registResult == null)
            {
                payFailedReason = "骑士绑定易宝支付失败,返回结果为null!";
            }
            else
            {
                LogHelper.LogWriterString("骑士绑定易宝支付失败",
                    string.Format("易宝错误信息:code{0},ledgerno:{1},hmac{2},msg{3}",
                        registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg));
                payFailedReason = string.Format("绑定失败:code{0},ledgerno:{1},hmac{2},msg{3}", registResult.code, registResult.ledgerno, registResult.hmac, registResult.msg);
            }
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (clienterFinanceAccountDao.ModifyYeepayInfoById(Convert.ToInt32(model.Id), "", 1))
                {
                    if (clienterFinanceDao.ModifyClienterWithdrawPayFailedReason(new ClienterWithdrawLogModel()
                    {
                        WithwardId = model.WithwardId,
                        PayFailedReason = payFailedReason,
                        Remark = payFailedReason,
                        Operator = "自动处理服务"
                    }))
                    {
                        tran.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// 易宝转账成功
        /// danny-20150804
        /// </summary>
        /// <param name="model"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private bool DealTransferAccountsYeeOk(dynamic model, decimal amount)
        {
            bool reg = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (clienterFinanceDao.AddYeePayUserBalanceRecord(new YeePayUserBalanceRecord()
                {
                    LedgerNo = model.YeepayKey,
                    WithwardId = model.WithwardId,
                    Amount = amount,
                    Balance = model.BalanceRecord + amount,
                    RecordType = YeeRecordType.P2C.GetHashCode(),
                    Operator = "自动处理服务",
                    Remark = "易宝主账户向商户子账户转账【" + amount + "】元"
                }))
                {
                    if (clienterFinanceDao.UpdateYeeBalanceRecord(model.YeepayKey, amount))
                    {
                        reg = true;
                        tran.Complete();
                    }
                }
            }
            return reg;
        }
        /// <summary>
        /// 商户易宝提现成功
        /// danny-20150804
        /// </summary>
        /// <param name="model"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private bool DealCashTransferYeeOkB(BusinessFinanceAccountModel model, decimal amount)
        {
            bool reg = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (clienterFinanceDao.AddYeePayUserBalanceRecord(new YeePayUserBalanceRecord()
                {
                    LedgerNo = model.YeepayKey,
                    WithwardId = model.WithwardId,
                    Amount = amount,
                    Balance = model.BalanceRecord - amount,
                    RecordType = YeeRecordType.Ccash.GetHashCode(),
                    Operator = "自动处理服务",
                    Remark = "易宝子账户提现【" + amount + "】元"
                }))
                {
                    if (clienterFinanceDao.UpdateYeeBalanceRecord(model.YeepayKey, -amount))
                    {
                        if (businessFinanceDao.BusinessWithdrawPayDealOk(new BusinessWithdrawLog()
                        {
                            WithwardId = model.WithwardId,
                            DealStatus = BusinessWithdrawFormDealStatus.Dealed.GetHashCode(),
                            Operator = "自动处理服务",
                            Remark = "自动处理服务处理商户提款申请单"
                        }))
                        {
                            reg = true;
                            tran.Complete();
                        }
                    }

                }
            }
            return reg;
        }
        /// <summary>
        /// 骑士易宝提现成功
        /// danny-20150805
        /// </summary>
        /// <param name="model"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private bool DealCashTransferYeeOkC(ClienterFinanceAccountModel model, decimal amount)
        {
            bool reg = false;
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                if (clienterFinanceDao.AddYeePayUserBalanceRecord(new YeePayUserBalanceRecord()
                {
                    LedgerNo = model.YeepayKey,
                    WithwardId = model.WithwardId,
                    Amount = amount,
                    Balance = model.BalanceRecord - amount,
                    RecordType = YeeRecordType.Ccash.GetHashCode(),
                    Operator = "自动处理服务",
                    Remark = "易宝子账户提现【" + amount + "】元"
                }))
                {
                    if (clienterFinanceDao.UpdateYeeBalanceRecord(model.YeepayKey, -amount))
                    {
                        if (clienterFinanceDao.ClienterWithdrawPayDealOk(new ClienterWithdrawLog()
                        {
                            WithwardId = model.WithwardId,
                            DealStatus = ClienterWithdrawFormDealStatus.Dealed.GetHashCode(),
                            Operator = "自动处理服务",
                            Remark = "自动处理服务处理骑士提款申请单"
                        }))
                        {
                            reg = true;
                            tran.Complete();
                        }
                    }

                }
            }
            return reg;
        }
        #endregion
    }
}
