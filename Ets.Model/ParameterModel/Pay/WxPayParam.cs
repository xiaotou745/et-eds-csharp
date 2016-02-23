using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Pay
{
    public class WxPayParam
    {
        public string order_no { get; set; }
        public string total_fee { get; set; }
        public string body { get; set; }
        public string notify { get; set; }
        public int platform { get; set; }
        public string attach { get; set; }
   
    }
}
