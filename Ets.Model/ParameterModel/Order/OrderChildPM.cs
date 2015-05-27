using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// B端发布子订单所需数据
    /// hulingbo 20150511
    /// </summary>
    public class OrderChlidPM
    {
        public int ChildId { get; set; }
        public decimal GoodPrice { get; set; }
    }
}
