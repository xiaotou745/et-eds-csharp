using System;
using System.Collections.Generic;
using System.Web;
using Aop.Api.Response;
namespace ETS.Library.Pay.SSAliPay
{
    public class AliNativePay
    {
        AliPayApi aliPayApi = new AliPayApi();
        /// <summary>
        /// 取消订单 闪送模式        
        /// </summary>
        /// 胡灵波
        /// 2015年12月9日 17:08:21
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool CloseOrder(string orderNo)
        {
            AlipayTradeCancelResponse alipayTradeCancelResponse=aliPayApi.Cancel();
            if (alipayTradeCancelResponse.Code != "10000")
                return false;
  
            return true;
        }

        /// <summary>
        /// 退款 闪送模式
        /// </summary>
        /// 胡灵波
        /// 2015年12月9日 17:36:08
        /// <param name="orderNo"></param>
        /// <param name="out_refund_no"></param>
        /// <param name="total_fee"></param>
        /// <param name="refund_fee"></param>
        /// <param name="op_user_id"></param>
        /// <returns></returns>
        public bool Refund()
        {
            AlipayTradeRefundResponse alipayTradeCancelResponse = aliPayApi.Refund();
            if (alipayTradeCancelResponse.Code != "10000")
                return false;

            return true;
        }
    
    }
}