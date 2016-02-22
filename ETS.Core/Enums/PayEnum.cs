using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Enums
{
    public enum PayEnum
    {
        [DisplayText("易代送商家端APP支付")]
        EDSBApp = 1,
        [DisplayText("易代送商家端扫码支付")]
        EDSBQr = 2,
        [DisplayText("易代送闪送商家端APP支付")]
        EDSSSBApp = 3,
        [DisplayText("易代送闪送商家端扫码支付")]
        EDSSSBQr = 4,
        [DisplayText("易代送骑士端APP支付")]
        EDSCApp = 5,
        [DisplayText("易代送骑士端扫码支付")]
        EDSCQr = 6,
        [DisplayText("轻骑士APP支付")]
        EDSQQSCApp = 7,
        [DisplayText("轻骑士扫码支付")]
        EDSQQSCQr = 8
    }
}
