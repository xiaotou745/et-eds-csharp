using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Subsidy
{
    public class subsidy
    {
        public int Id { get; set; }
        public Nullable<decimal> OrderCommission { get; set; }
        public Nullable<decimal> DistribSubsidy { get; set; }
        public Nullable<decimal> WebsiteSubsidy { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<decimal> PKMCost { get; set; }
        public Nullable<int> OrderType { get; set; }
    }
}
