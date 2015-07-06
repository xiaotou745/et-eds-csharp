using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Enums
{
    public enum OrderStatus
    { 
        [DisplayText("待接单")]
        Status0 = 0,     
        [DisplayText("订单已完成")]
        Status1 = 1,     
        [DisplayText("订单已接单")]
        Status2 = 2,    
        [DisplayText("订单已取消")]
        Status3 = 3,     
        [DisplayText("订单已取货")]
        Status4 = 4,   
        [DisplayText("第三方待接入订单")]
        Status30 = 30
    }
}
