using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    public class RushOrderModel
    {
        public int userId { get; set; }
        public string orderNo { get; set; }
        public string Version { get; set; }

        public double RushLongitude { get; set; }
    }
}
