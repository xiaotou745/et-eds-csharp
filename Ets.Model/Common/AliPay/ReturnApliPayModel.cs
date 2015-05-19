using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common.AliPay
{
    public class ReturnApliPayModel
    {
        public string is_success { get; set; }
        //public string error_code { get; set; }
        public string out_trade_no { get; set; }
    }
}
