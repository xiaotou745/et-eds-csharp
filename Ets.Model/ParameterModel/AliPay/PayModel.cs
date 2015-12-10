using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.AliPay
{
    public class PayModel
    {
        /// <summary>
        /// 支付方式 1:支付宝;2微信,3网银,6现金支付
        /// </summary>
        public int payType { get; set; }

        /// <summary>
        /// 订单ID,看好。不是OrderNo
        /// </summary>
        public int orderId { get; set; }

        /// <summary>
        /// 子订单ID
        /// </summary>
        public int childId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int productId { get; set; }

        /// <summary>
        /// 支付方式(1 用户支付 2 骑士代付 3商户支付)
        /// </summary>
        public int payStyle { get; set; }

        /// <summary>
        /// 接口版本号
        /// </summary>
        public string version { get; set; }

        ///// <summary>
        ///// 支付类型 0旧版易代送，1闪送模式
        ///// </summary>
        //public int cType { get; set; }
        /// <summary>
        /// 订单状态0订单支付，1小费
        /// </summary>
        public int oType { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 小费金额
        /// </summary>
        public decimal tipAmount { get; set; }
        
    }
}
