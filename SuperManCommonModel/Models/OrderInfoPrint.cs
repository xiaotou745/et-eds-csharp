using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManCommonModel.Models
{
    /// <summary>
    /// 需要打印的订单信息
    /// </summary>
    public class OrderInfoPrint
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }
        /// <summary>
        /// 抢单骑士名称
        /// </summary>
        public string ClienterName { get; set; }
        /// <summary>
        /// 抢单时间
        /// </summary>
        //public string RushOrderTime { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string ReceiveName { get; set; }
        /// <summary>
        /// 顾客电话
        /// </summary>
        public string ReceivePhone { get; set; }
        /// <summary>
        /// 顾客地址
        /// </summary>
        public string ReceiveAddress { get; set; }
        /// <summary>
        /// 抢单骑士电话
        /// </summary>
        public string ClienterPhone { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string BussinessName { get; set; }
        /// <summary>
        /// 订单类型1送餐，2取餐盒
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int BusiId { get; set; }
        /// <summary>
        /// 取餐具备注
        /// </summary>
        public string QuCanJuRemark { get; set; }
        public string Remark { get; set; }
    }
}