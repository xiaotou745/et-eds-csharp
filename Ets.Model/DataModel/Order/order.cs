using Ets.Model.ParameterModel.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string BusinessName { get; set; }

        public string BusinessPhone { get; set; }

        public string PickUpCity { get; set; }

        public Nullable<double> BusiLongitude { get; set; }
        public Nullable<double> BusiLatitude { get; set; }


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

    }
}
