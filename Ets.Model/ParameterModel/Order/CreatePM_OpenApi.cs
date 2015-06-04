using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Ets.Model.Common;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 创建订单  参数实体 add by caoheyang 20150317
    /// </summary>
    public class CreatePM_OpenApi
    {
        /// <summary>
        /// 订单号 第三方订单号
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
        /// 支付类型 默认0   0 现金   1 在线支付 
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

        ///// <summary>
        ///// 外送费，默认为0   无用，暂时注释
        ///// </summary>
        public decimal delivery_fee { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal weight { get; set; }

        /// <summary>
        /// 订单数量，默认为1（并单时使用)
        /// </summary>
        public int? package_count { get; set; }

        /// <summary>
        /// 取货码（目前只有全时再用）
        /// </summary>
        public string pickupcode { get; set; }

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

        /// <summary>
        /// 订单佣金计算方法 0：默认 1：根据时间段设置不同补贴
        /// </summary>
        public int CommissionFormulaMode { get; set; }

        /// <summary>
        /// 订单结算金额 由业务逻辑层计算所得
        /// </summary>
        public decimal settlemoney { get; set; }

        /// <summary>
        /// 订单额外补贴金额 由业务逻辑层计算所得
        /// </summary>
        public decimal adjustment { get; set; }

        /// <summary>
        /// 初始订单状态 默认为 0  由业务逻辑层计算所得 add by caoheyang 20150421 
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// E代送商户id 由业务逻辑层所得 允许为0 为0时代表需要新增商户  add by caoheyang 20150421 
        /// </summary>
        public int businessId { get; set; }

        /// <summary>
        ///订单来源   由业务逻辑层所得 add by caoheyang 20150422 
        /// 默认0表示E代送B端订单，1易淘食,2万达，3全时，4美团 
        /// </summary>
        public int orderfrom { get; set; }

        /// <summary>
        /// 结算类型：1：固定比例 2：固定金额  由业务逻辑层所得 
        /// </summary>
        public int CommissionType { get; set; }
        /// <summary>
        /// 固定金额  由业务逻辑层所得 
        /// </summary>
        public decimal CommissionFixValue { get; set; }
        /// <summary>
        /// 分组ID  由业务逻辑层所得 
        /// </summary>
        public int BusinessGroupId { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoice_title { get; set; }

        /// <summary>
        /// 订单号  由业务逻辑层所得   
        /// </summary>
        public string OrderNo { get; set; }

        public int MealsSettleMode { get; set; }

        /// <summary>
        /// 应退商家金额
        /// </summary>
        public decimal BusinessReceivable { get; set; }
        
    }

    /// <summary>
    /// 收货地址信息  参数实体 add by caoheyang 20150317
    /// </summary>
    public class Address
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required]
        public string user_name { get; set; }

        /// <summary>
        /// 用户联系电话
        /// </summary>
        [Required]
        public string user_phone { get; set; }

        /// <summary>
        /// 用户所在省份code
        /// </summary>
        public string province_code { get; set; }

        /// <summary>
        /// 用户所在城市code
        /// </summary>
        public string city_code { get; set; }

        /// <summary>
        /// 用户所在区域code
        /// </summary>
        public string area_code { get; set; }

        /// <summary>
        ///用户所在省份名称
        /// </summary>
        [Required]
        public string province { get; set; }

        /// <summary>
        /// 用户所在城市名称
        /// </summary>
        [Required]
        public string city { get; set; }

        /// <summary>
        /// 用户所在区域名称
        /// </summary>
        [Required]
        public string area { get; set; }

        /// <summary>
        /// 用户收货地址
        /// </summary>
        [Required]
        public string address { get; set; }

        /// <summary>
        /// 用户收货地址所在区域经度
        /// </summary>
        public decimal longitude { get; set; }

        /// <summary>
        /// 用户收货地址所在区域纬度
        /// </summary>
        public decimal latitude { get; set; }

    }

    /// <summary>
    /// 收货地址信息  参数实体 add by caoheyang 20150317
    /// </summary>
    public class Store
    {
        /// <summary>
        /// 对接方店铺ID
        /// </summary>
        [Required]
        public int store_id { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        [Required]
        public string store_name { get; set; }

        /// <summary>
        /// 集团：3:万达    查询得到。赋值
        /// </summary>
        public int group { get; set; }

        /// <summary>
        /// 店铺身份证号
        /// </summary>
        public string id_card { get; set; }

        /// <summary>
        /// 门店联系电话
        /// </summary>
        [Required]
        public string phone { get; set; }

        /// <summary>
        /// 门店第二联系电话
        /// </summary>
        public string phone2 { get; set; }

        /// <summary>
        /// 门店地址
        /// </summary>
        public string address { get; set; }


        /// <summary>
        /// 门店所在省份code
        /// </summary>
        public string province_code { get; set; }

        /// <summary>
        /// 门店所在城市code
        /// </summary>
        public string city_code { get; set; }

        /// <summary>
        /// 门店所在区域code
        /// </summary>
        public string area_code { get; set; }

        /// <summary>
        ///门店所在省份名称
        /// </summary>
        [Required]
        public string province { get; set; }

        /// <summary>
        /// 门店所在城市名称
        /// </summary>
        [Required]
        public string city { get; set; }

        /// <summary>
        /// 门店所在区域名称
        /// </summary>
        [Required]
        public string area { get; set; }

        /// <summary>
        /// 门店所在区域经度
        /// </summary>
        public decimal longitude { get; set; }

        /// <summary>
        /// 门店所在区域纬度
        /// </summary>
        public decimal latitude { get; set; }

        /// <summary>
        /// 外送费,默认为0
        /// </summary>
        public decimal delivery_fee { get; set; }

        /// <summary>
        /// 佣金类型，涉及到快递员的佣金计算方式，默认1
        /// </summary>
        public int? commission_type { get; set; }

        /// <summary>
        /// 结算比例 目前由业务逻辑层计算查询所得
        /// </summary>
        public decimal businesscommission { get; set; }
    }


    /// <summary>
    /// -订单明细信息  参数实体 add by caoheyang 20150317
    /// </summary>
    public class OrderDetail
    {
        /// <summary>
        ///  第三方订单明细id
        /// </summary>
        [Required]
        public int detail_id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        public string product_name { get; set; }

        /// <summary>
        /// 商品单价，精确到两位小数
        /// </summary>
        [Required]
        public decimal unit_price { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [Required]
        public int quantity { get; set; }
    }
}
