using Ets.Model.DomainModel.Bussiness;
using Ets.Model.DomainModel.Clienter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Order;
namespace Ets.Model.DomainModel.Order
{
    public class OrderDM
    {
        public OrderDM() { }

        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 取货地址
        /// </summary>
        public string PickUpAddress { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PubDate { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceviceName { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string RecevicePhoneNo { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceviceAddress { get; set; }
        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualDoneDate { get; set; }
        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 订单佣金-应付
        /// </summary>
        public decimal? OrderCommission { get; set; }
        /// <summary>
        /// 外送费
        /// </summary>
        public decimal? DistribSubsidy { get; set; }
        /// <summary>
        /// 网站补贴
        /// </summary>
        public decimal? WebsiteSubsidy { get; set; }
        /// <summary>
        /// 配送说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单状态  0:订单新增 1：订单已完成 2：订单已接单 3：订单已取消  30 待接入订单
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 超人Id
        /// </summary>
        public int? clienterId { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>
        public int? businessId { get; set; }
        /// <summary>
        /// 收获人所在城市
        /// </summary>
        public string ReceviceCity { get; set; }
        /// <summary>
        /// 收获人所在城市经度
        /// </summary>
        public decimal? ReceviceLongitude { get; set; }
        /// <summary>
        /// 收获人所在城市纬度
        /// </summary>
        public decimal? ReceviceLatitude { get; set; }
        /// <summary>
        /// 订单来源，默认0表示E代送B端订单，1易淘食,2万达，3全时，4美团
        /// </summary>
        public int OrderFrom { get; set; }
        /// <summary>
        /// 其它平台的来源订单Id
        /// </summary>
        public long OriginalOrderId { get; set; }
        /// <summary>
        /// 其它平台的来源订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }
        /// <summary>
        /// 份数(菜的份数)
        /// </summary>
        public int? Quantity { get; set; }
        /// <summary>
        /// 订单总重量(海底捞)
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// 接收省份
        /// </summary>
        public string ReceiveProvince { get; set; }
        /// <summary>
        /// 接收区域
        /// </summary>
        public string ReceiveArea { get; set; }
        /// <summary>
        /// 接收省份代码 
        /// </summary>
        public string ReceiveProvinceCode { get; set; }
        /// <summary>
        /// 接收城市代码 
        /// </summary>
        public string ReceiveCityCode { get; set; }
        /// <summary>
        /// 接收区域代码 
        /// </summary>
        public string ReceiveAreaCode { get; set; }
        /// <summary>
        /// 订单类型1送餐订单2取餐盒订单(海底捞）
        /// </summary>
        public int? OrderType { get; set; }
        /// <summary>
        /// 送餐距离(海底捞)
        /// </summary>
        public decimal? KM { get; set; }
        /// <summary>
        /// 锅具数量(海底捞)
        /// </summary>
        public int? GuoJuQty { get; set; }
        /// <summary>
        /// 炉具数量(海底捞)
        /// </summary>
        public int? LuJuQty { get; set; }
        /// <summary>
        /// 送餐时间,客户要求送餐时间
        /// </summary>
        public DateTime? SongCanDate { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int? OrderCount { get; set; }
        /// <summary>
        /// 订单佣金比例
        /// </summary>
        public decimal? CommissionRate { get; set; }
        /// <summary>
        /// 支付类型 0 现金
        /// </summary>
        public int? Payment { get; set; }
        /// <summary>
        /// 订单佣金计算方法 0：默认 1：根据时间段设置不同补贴 2保本算法
        /// </summary>
        public int? CommissionFormulaMode { get; set; }
        /// <summary>
        /// 额外补贴金额
        /// </summary>
        public decimal? Adjustment { get; set; }
        /// <summary>
        /// 商户结算比例
        /// </summary>
        public decimal? BusinessCommission { get; set; }
        /// <summary>
        /// 结算金额-应收
        /// </summary>
        public decimal? SettleMoney { get; set; }
        /// <summary>
        /// 额外补贴次数
        /// </summary>
        public int DealCount { get; set; }
        /// <summary>
        /// 取货码（目前只有全时再用）
        /// </summary>
        public string PickupCode { get; set; }
        /// <summary>
        /// 美团订单（商户取消接单原因）
        /// </summary>
        public string OtherCancelReason { get; set; }
        /// <summary>
        /// 结算类型(1固定比例2固定金额)
        /// </summary>
        public int? CommissionType { get; set; }
        /// <summary>
        /// 固定金额
        /// </summary>
        public decimal? CommissionFixValue { get; set; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public int? BusinessGroupId { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeSpan { get; set; }

        /// <summary>
        /// 子订单集合
        /// </summary>
        public List<OrderChild> listOrderChild { get; set; }

        /// <summary>
        /// 订单明细集合
        /// </summary>
        public List<OrderDetail> listOrderDetail { get; set; }        
    }

    /// <summary>
    /// 订单实体
    /// </summary>
    public class BusiGetOrderModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        public string superManName { get; set; }
        public string superManPhone { get; set; }
        public string PickUpName { get; set; }
        /// <summary>
        /// 取货地址
        /// </summary>
        public string PickUpAddress { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string PubDate { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceviceName { get; set; }
        /// <summary>
        /// 收货电话
        /// </summary>
        public string RecevicePhoneNo { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceviceAddress { get; set; }
        /// <summary>
        /// 实际完成时间
        /// </summary>
        public string ActualDoneDate { get; set; }
        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool? IsPay { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 配送说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public byte? Status { get; set; }
        /// <summary>
        /// 商家到收货人的距离
        /// </summary>
        public double distanceB2R { get; set; }
    }
}
