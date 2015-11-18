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
}
