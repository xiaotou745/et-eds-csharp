using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public class OrderListModel
    {
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
        public DateTime PubDate { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceviceName { get; set; }
        /// <summary>
        /// 收货地址
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
        public bool? IsPay { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 订单佣金
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
        /// 订单状态
        /// </summary>
        public byte? Status { get; set; }
        /// <summary>
        /// 骑士Id
        /// </summary>
        public int clienterId { get; set; }
        /// <summary>
        /// 商户Id
        /// </summary>
        public int businessId { get; set; }
        /// <summary>
        /// 收获人所在城市
        /// </summary>
        public string ReceviceCity { get; set; }
        /// <summary>
        /// 收获地址经度
        /// </summary>
        public double ReceviceLongitude { get; set; }
        /// <summary>
        /// 收货地址纬度
        /// </summary>
        public double ReceviceLatitude { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public int OrderFrom { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public string OrderFromName { get; set; }
        /// <summary>
        /// 原始订单Id
        /// </summary>
        public long OriginalOrderId { get; set; }
        /// <summary>
        /// 原始订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }
        /// <summary>
        ///数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 收货省份
        /// </summary>
        public string ReceiveProvince { get; set; }
        /// <summary>
        /// 收货地区
        /// </summary>
        public string ReceiveArea { get; set; }
        /// <summary>
        /// 收货省份编码
        /// </summary>
        public string ReceiveProvinceCode { get; set; }
        /// <summary>
        /// 收货城市编码
        /// </summary>
        public string ReceiveCityCode { get; set; }
        /// <summary>
        /// 收货地区编码
        /// </summary>
        public string ReceiveAreaCode { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }
        public double KM { get; set; }
        /// <summary>
        /// 锅具数量
        /// </summary>
        public int GuoJuQty { get; set; }
        /// <summary>
        /// 炉具数量
        /// </summary>
        public int LuJuQty { get; set; }
        /// <summary>
        /// 送餐时间
        /// </summary>
        public string SongCanDate { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 佣金比例
        /// </summary>
        public decimal CommissionRate { get; set; }

        public string OrderSign { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string BusinessName { get; set; }
        /// <summary>
        /// 商家手机号
        /// </summary>
        public string BusinessPhoneNo { get; set; }

        /// <summary>
        /// 商家手机号2
        /// </summary>
        public string BusinessPhoneNo2 { get; set; }
        /// <summary>
        /// 商家地址
        /// </summary>
        public string BusinessAddress { get; set; }
        /// <summary>
        /// 骑士姓名
        /// </summary>
        public string ClienterName { get; set; }
        /// <summary>
        /// 骑士手机号
        /// </summary>
        public string ClienterPhoneNo { get; set; }
        /// <summary>
        /// 订单所属商家的集团名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 商家所在城市
        /// </summary>
        public string BusinessCity { get; set; }

        /// <summary>
        /// 超人姓名
        /// </summary>
        public string ClienterTrueName { get; set; }

        /// <summary>
        /// 集团id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 骑士收入
        /// </summary>
        public decimal? AccountBalance { get; set; }


        /// <summary>
        /// 补贴金额
        /// </summary>
        public decimal Adjustment { get; set; }

        /// <summary>
        /// 商家结算比例
        /// </summary>
        public decimal BusinessCommission { get; set; }

        /// <summary>
        /// 结算类型：1：固定比例 2：固定金额   add by 彭宜   20150727
        /// </summary>
        public int CommissionType { get; set; }

        /// <summary>
        /// 固定金额   add by 彭宜   20150727
        /// </summary>
        public decimal CommissionFixValue { get; set; }

        /// <summary>
        ///  取货码（目前只有全时再用）
        /// </summary>
        public string PickupCode { get; set; }
        /// <summary>
        /// 需要上传的小票图片张数
        /// </summary>
        public int NeedUploadCount { get; set; }
        /// <summary>
        /// 已上传小票数量
        /// </summary>
        public int HadUploadCount { get; set; }
        /// <summary>
        /// 小票图片路径用竖线分隔（|）
        /// </summary>
        public string ReceiptPic { get; set; }
        /// <summary>
        /// 取消原应
        /// </summary>
        public string OtherCancelReason { get; set; }
        /// <summary>
        /// 订单明细
        /// </summary>
        public List<OrderChild> OrderChildList { get; set; }
        /// <summary>
        /// 子订单
        /// </summary>
        public List<OrderDetail> OrderDetailList { get; set; }
        /// <summary>
        /// 商户结算金额
        /// </summary>
        public decimal SettleMoney { get; set; }
    
        /// <summary>
        /// 餐费结算方式（0：线下结算 1：线上结算）
        /// </summary>
        public int MealsSettleMode { get; set; }
        /// <summary>
        /// 订单是否已分账（0：否 1：是）
        /// </summary>
        public int IsJoinWithdraw { get; set; }
        /// <summary>
        /// 商户应收
        /// </summary>
        public decimal BusinessReceivable { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OptUserName { get; set; }

        /// <summary>
        /// 抢单-完成的距离
        /// </summary>
        public double GrabToCompleteDistance { get; set; }
        /// <summary>
        /// 任务接单时间
        /// </summary>
        public DateTime? GrabTime { get; set; }
        /// <summary>
        /// 是否无效订单
        /// </summary>
        public int IsNotRealOrder { get; set; }

        /// <summary>
        /// 最终给骑士的佣金
        /// </summary>
        public decimal RealOrderCommission { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsEnable { get; set; }
        /// <summary>
        /// 结算类型
        /// </summary>
        public string SettleType { get; set; }
        /// <summary>
        /// 配送公司结算数值
        /// </summary>
        public decimal SettleValue { get; set; }
        /// <summary>
        /// 骑士结算数值
        /// </summary>
        public decimal SuperManSettleValue { get; set; }
        /// <summary>
        /// 取货时间
        /// </summary>
        public DateTime? TakeTime { get; set; }
        /// <summary>
        /// 物流公司结算金额
        /// </summary>
        public decimal DeliveryCompanySettleMoney { get; set; }
        /// <summary>
        /// 物流公司id
        /// </summary>
        public int DeliveryCompanyID { get; set; }
        /// <summary>
        /// 扣除补贴原因
        /// </summary>
        public string DeductCommissionReason { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public int FinishAll { get; set; }

        /// <summary>
        /// 扣除网站补贴方式    1:自动扣除    2:手动扣除
        /// </summary>
        public int DeductCommissionType { get; set; }
    }
}
