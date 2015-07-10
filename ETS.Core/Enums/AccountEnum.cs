using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Enums
{
    public enum AccountType
    {
        // 管理员 
        AdminUser = 1,
        // 普通帐号
        User = 2,
    }

    /// <summary>
    /// 图片类型  add by pengyi 20150709
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// 商户
        /// </summary>
        Business = 1,
        /// <summary>
        /// 骑士
        /// </summary>
        Clienter = 2,
        /// <summary>
        /// 小票
        /// </summary>
        Receipt = 3,
    }
}
