using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;

namespace ETS.Enums
{
   /// <summary>
   /// 系统级别接口返回 枚举
   /// </summary>
    public enum SystemEnum
    {
        Success=1,
        [DisplayText("系统错误")]
        SystemError=0,
        [DisplayText("参数错误")]
        ParaError = -2
    }
}
