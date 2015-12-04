using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;

namespace ETS.Enums
{
    /// <summary>
    /// 淘宝取消订单返回值枚举错误
    /// caoheyang 20151118
    /// </summary>
    public enum TaoBaoCancelOrderReturn
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("订单不存在")]
        NoExist = 101,
        [DisplayText("内部错误")]
        Error = 102,
    }


    /// <summary>
    /// 淘宝发布订单    
    /// </summary>
    public enum TaoBaoPushOrder
    {
        [DisplayText("成功")]
        Success = 1,   
        [DisplayText("系统错误")]
        Error = -1,
        [DisplayText("订单已存在")]
        OrderId = -2
    }
}
