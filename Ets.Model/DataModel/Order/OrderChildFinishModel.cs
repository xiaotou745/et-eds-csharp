using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public class OrderChildFinishModel
    {
        /// <summary>
        /// 主订单ID
        /// </summary>
        public int orderId { get; set; }

        /// <summary>
        /// 子订单ID
        /// </summary>
        public int orderChildId { get; set; }

        /// <summary>
        /// 支付方式(1 用户支付 2 骑士代付)
        /// </summary>
        public int payStyle { get; set; }

        /// <summary>
        /// 支付人账号
        /// </summary>
        public string payBy { get; set; }

        /// <summary>
        /// 支付类型(1 支付宝 2 微信 3 网银)
        /// </summary>
        public int payType { get; set; }

        /// <summary>
        /// 第三方平台订单号
        /// </summary>
        public string originalOrderNo { get; set; }
    }
}
