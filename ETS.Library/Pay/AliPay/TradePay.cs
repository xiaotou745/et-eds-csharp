using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Library.Pay.AliPay
{
    public class TradePay
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no{get;set;}

        /// <summary>
        /// 卖家支付宝用户ID
        /// </summary>
        public string seller_id{get;set;}

        /// <summary>
        /// 订单总金额
        /// </summary>
        public string total_amount{get;set;}


        /// <summary>
        /// 可打折金额
        /// </summary>
        public string discountable_amount{get;set;}
        
	    /// <summary>
        /// 不可打折金额
        /// </summary>
        public string undiscountable_amount{get;set;}	

	    /// <summary>
        /// 订单标题
        /// </summary>
        public string subject{get;set;}	

	    /// <summary>
        /// 订单描述
        /// </summary>
        public string body{get;set;}	

	    /// <summary>
        /// 商品明细列表
        /// </summary>
        public string goods_detail{get;set;}
	    /// <summary>
        /// 商户操作员编号
        /// </summary>
        public string operator_id{get;set;}
         /// <summary>
        /// 商户门店编号
        /// </summary>
        public string store_id{get;set;}
         /// <summary>
        /// 机具终端编号
        /// </summary>
        public string terminal_id{get;set;}
         /// <summary>
        /// 扩展参数
        /// </summary>
        public string extend_params{get;set;}
         /// <summary>
        /// 支付超时时间表达式
        /// </summary>
        public string timeout_express{get;set;}		

        /// <summary>
        /// 分账信息
        /// </summary>
        public string royalty_info{get;set;}	

        /// <summary>
        /// 异步url
        /// </summary>
        public string notify_url { get; set; }	      


	
    }
}
