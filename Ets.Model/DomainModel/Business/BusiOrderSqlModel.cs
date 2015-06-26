using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Business
{
    public class BusiOrderSqlModel
    {
        public string ActualDoneDate { get; set; }
        public decimal? Amount { get; set; }
        public bool IsPay { get; set; }
        public string OrderNo { get; set; }
        public string PickUpAddress { get; set; }
        public string PubDate { get; set; }
        
        public string ReceviceAddress { get; set; }
        public string ReceviceName { get; set; }
        public string RecevicePhoneNo { get; set; }
        public string Remark { get; set; }
        public byte Status { get; set; }
        public double? ReceviceLongitude { get; set; }
        public double? ReceviceLatitude { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string PickUpName { get; set; }
        public string SuperManName { get; set; }
        public string SuperManPhone { get; set; }
        public int OrderFrom { get; set; }
        public string OriginalOrderNo { get; set; }
        public int OrderId { get; set; }
        public int MealsSettleMode { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal? TotalAmount { get; set; }
        public int OrderCount { get; set; }
    }
}
