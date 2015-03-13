using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public class OrderCommission
    {
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> CommissionRate { get; set; }
        public Nullable<decimal> DistribSubsidy { get; set; }
        public Nullable<decimal> WebsiteSubsidy { get; set; }
        public Nullable<int> OrderCount { get; set; }
    }
}
