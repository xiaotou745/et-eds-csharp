using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;

namespace ETS.Enums
{
    /// <summary>
    /// 标签类型
    ///  add  by caoheyang 20150917
    /// </summary>
    public enum TagType
    {
        /// <summary>
        /// 商家
        /// </summary>
        [DisplayText("商家")]
        Business = 0,
        /// <summary>
        /// 骑士
        /// </summary>
        [DisplayText("骑士")]
        Clienter=1
    }
}
