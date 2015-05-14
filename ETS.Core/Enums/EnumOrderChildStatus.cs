using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Enums
{
    public enum EnumOrderChildStatus
    {
        [DisplayText("待支付")]
        DaiZhiFu = 0,
        [DisplayText("已完成")]
        YiWanCheng = 1,
        [DisplayText("支付中")]
        ZhiFuZhong = 2
    }
}
