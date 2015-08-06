using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETS.Library.Pay.WxPay
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify : Notify
    {
        //public ResultNotify(Page page)
        //    : base(page)
        //{
        //}

        public WxNotifyResultModel ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            //string openid = 
            //string product_id =

            //string out_trade_no =

            WxNotifyResultModel model = new WxNotifyResultModel();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                //WxPayData res = new WxPayData();
                //res.SetValue("return_code", "FAIL");
                //res.SetValue("return_msg", "支付结果中微信订单号不存在");
                //Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                //page.Response.Write(res.ToXml());
                //page.Response.End();
                model.return_code = "FAIL";
                model.return_msg = "支付结果中微信订单号不存在";
                return model;
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                //WxPayData res = new WxPayData();
                //res.SetValue("return_code", "FAIL");
                //res.SetValue("return_msg", "订单查询失败");
                //Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                //page.Response.Write(res.ToXml());
                //page.Response.End();
                model.return_code = "FAIL";
                model.return_msg = "订单查询失败";
                return model;
            }
            //查询订单成功
            //else
            //{
            //WxPayData res = new WxPayData();
            //res.SetValue("return_code", "SUCCESS");
            //res.SetValue("return_msg", "OK");
            //Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
            //page.Response.Write(res.ToXml());
            //page.Response.End();
            model.return_code = "SUCCESS";
            model.return_msg = "OK";
            model.openid = notifyData.GetValue("openid").ToString();//支付人
            model.total_fee = notifyData.GetValue("total_fee").ToString();//总金额
            model.transaction_id = notifyData.GetValue("transaction_id").ToString();//微信支付订单号
            model.order_no = notifyData.GetValue("attach").ToString();
            return model;
            //}
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}