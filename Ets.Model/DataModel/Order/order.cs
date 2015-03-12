using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public partial class order
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
        public Nullable<decimal> DistribSubsidy { get; set; }
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

        public virtual business business { get; set; }
    }
}
