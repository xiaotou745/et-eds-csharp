using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    public class OrderRemindModel
    {
        public int Id { get; set; } 
        public int OrderId { get; set; }

        public long DeliveryOrderNo { get; set; }

        public int IsOrderRemind { get; set; }

        public string ReceviceName { get; set; }

        public string RecevicePhoneNo { get; set; }
    }
}
