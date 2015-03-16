using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Enums
{
    /// <summary>
    /// 登录状态
    /// </summary>
    public enum LoginModelStatus
    {
        Success,
        [DisplayText("用户名或密码错误")]
        InvalidCredential
    }
}
