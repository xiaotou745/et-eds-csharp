using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    public class OrderCompleteModel
    {
        public int userId { get; set; }

        public string orderNo { get; set; }
        public string pickupCode { get; set; }
        public string version { get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        /// <summary>
        /// 暂时没用到，需要让C端APP传值 
        /// </summary>
        public int orderId { get; set; }

    }
}
