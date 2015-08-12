using Ets.Model.ParameterModel.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.ParameterModel.Order;
namespace Ets.Model.DataModel.Order
{
    public class order
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public string PickUpAddress { get; set; }
        public Nullable<System.DateTime> PubDate { get; set; }
        public string ReceviceName { get; set; }
        public string RecevicePhoneNo { get; set; }
        public string ReceviceAddress { get; set; }
        public Nullable<System.DateTime> ActualDoneDate { get; set; }
        public Nullable<bool> IsPay { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> OrderCommission { get; set; }

        /// <summary>
        /// 外送费
        /// </summary>
        public Nullable<decimal> DistribSubsidy { get; set; }
        /// <summary>
        /// 网站补贴
        /// </summary>
        public Nullable<decimal> WebsiteSubsidy { get; set; }
        public string Remark { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<int> clienterId { get; set; }
        public Nullable<int> businessId { get; set; }
        public string ReceviceCity { get; set; }
        public Nullable<double> ReceviceLongitude { get; set; }
        public Nullable<double> ReceviceLatitude { get; set; }
        public int OrderFrom { get; set; }
        public Nullable<long> OriginalOrderId { get; set; }
        public string OriginalOrderNo { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string ReceiveProvince { get; set; }
        public string ReceiveArea { get; set; }
        public string ReceiveProvinceCode { get; set; }
        public string ReceiveCityCode { get; set; }
        public string ReceiveAreaCode { get; set; }
        public Nullable<int> OrderType { get; set; }
        public Nullable<double> KM { get; set; }
        public Nullable<int> GuoJuQty { get; set; }
        public Nullable<int> LuJuQty { get; set; }
        public Nullable<System.DateTime> SongCanDate { get; set; }
        public Nullable<int> OrderCount { get; set; }
        public Nullable<decimal> CommissionRate { get; set; }
        /// <summary>
        /// 基本补贴佣金 add by 彭宜 20150807
        /// </summary>
        public Nullable<decimal> BaseCommission { get; set; }

        public string BusinessName { get; set; }

        public string BusinessPhone { get; set; }

        public string BusinessPhone2 { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string Landline { get; set; }        

        /// <summary>
        /// 发货人地址
        /// </summary>
        public string BusinessAddress { get; set; }     

        public string PickUpCity { get; set; }

        public Nullable<double> BusiLongitude { get; set; }
        public Nullable<double> BusiLatitude { get; set; }

        /// <summary>
        /// 配送员姓名
        /// </summary>
        public string ClienterName { get; set; }

        /// <summary>
        /// 配送员电话
        /// </summary>
        public string ClienterPhoneNo { get; set; }

        /// <summary>
        /// 订单佣金计算方法 0：默认 1：根据时间段设置不同补贴
        /// </summary>
        public int CommissionFormulaMode { get; set; }

        /// <summary>
        /// 商户结算比例
        /// </summary>
        public decimal BusinessCommission { get; set; }


        /// <summary>
        /// 应收 结算金额
        /// </summary>
        public decimal SettleMoney { get; set; }

        /// <summary>
        /// 额外补贴金额
        /// </summary>
        public decimal Adjustment { get; set; }

        /// <summary>
        /// 集团id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 已经上传的小票数量
        /// </summary>
        public int HadUploadCount { get; set; }

        /// <summary>
        /// 结算类型：1：固定比例 2：固定金额
        /// </summary>
        public int CommissionType { get; set; }
        /// <summary>
        /// 固定金额
        /// </summary>
        public decimal CommissionFixValue { get; set; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public int BusinessGroupId { get; set; }   
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeSpan { get; set; }


        /// <summary>
        /// 订单小票列表
        /// </summary>
        public List<OrderChlidPM> listOrderChild { get; set; }

        /// <summary>
        /// 支付类型 0 现金
        /// </summary>
        public int? Payment { get; set; }
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
        /// 发票
        /// </summary>
        public string Invoice { get; set; }
        
        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; }
        /// <summary>
        ///  纬度
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// 须上传小票数量
        /// </summary>
        public int NeedUploadCount { get; set; }

        /// <summary>
        /// 总配送费
        /// </summary>
        public decimal TotalDistribSubsidy { get; set; }

        /// <summary>
        /// 抢单时间
        /// </summary>
        public DateTime GrabTime { get; set; }

        /// <summary>
        /// 商户发单经度
        /// </summary>
        public double PubLongitude { get; set; }

        /// <summary>
        /// 商户发单纬度
        /// </summary>
        public double PubLatitude { get; set; }
        /// <summary>
        /// 餐费结算方式（0：线下结算 1：线上结算）
        /// </summary>
        public int MealsSettleMode { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal? TotalAmount { get; set; }


         /// <summary>
        /// 应退商家金额
        /// </summary>
        public decimal BusinessReceivable { get; set; }


        /// <summary>
        /// 骑士到门店的距离  米
        /// </summary>    
        public double distance { get; set; }

        /// <summary>
        ///  商户到收货人的距离  千米
        /// </summary>
        public double distanceB2R { get; set; }

        /// <summary>
        /// 是否一键发单（0否，1是）
        /// </summary>
        public int OneKeyPubOrder { get; set; }
        /// <summary>
        /// 是否需要审核
        /// </summary>
        public int IsOrderChecked { get; set; }


    }
}
