using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 创建订单  参数实体 add by caoheyang 20150317
    /// </summary>
    public class OrderDetailPM_OpenApi
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        public string order_id { get; set; }

        /// <summary>
        /// 订单下单时间
        /// </summary>
        [Required]
        public DateTime create_time { get; set; }

        /// <summary>
        /// 要求送餐时间
        /// </summary>
        [Required]
        public DateTime receive_time { get; set; }

        /// <summary>
        /// 用户是否已付款，true 已付款 false 未付款
        /// </summary>
        [Required]
        public bool is_pay { get; set; }

        /// <summary>
        /// 支付类型 默认0  0 现金
        /// </summary>
        public int payment { get; set; }

        /// <summary>
        /// 订单金额，精确到两位小数
        /// </summary>
        [Required]
        public decimal total_price { get; set; }

        /// <summary>
        /// 收货地址信息
        /// </summary>
        [Required]
        public Address address { get; set; }

        /// <summary>
        /// 门店信息
        /// </summary>
        [Required]
        public Store store_info { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        [Required]
        public OrderDetail[] order_details { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 重量，默认为0
        /// </summary>
        public decimal delivery_fee { get; set; }

        /// <summary>
        /// 外送费,默认为0
        /// </summary>
        public decimal weight { get; set; }

        /// <summary>
        /// 订单数量，默认为1（并单时使用)
        /// </summary>
        public int? package_count { get; set; }

        /// <summary>
        /// 骑士订单佣金 目前由业务逻辑层计算赋值所得
        /// </summary>
        public decimal ordercommission { get; set; }

        /// <summary>
        /// 网站补贴 目前由业务逻辑层计算查询所得
        /// </summary>
        public decimal websitesubsidy { get; set; }

        /// <summary>
        /// 订单佣金比例  目前由业务逻辑层计算查询所得
        /// </summary>
        public decimal commissionrate { get; set; }

    }
}
