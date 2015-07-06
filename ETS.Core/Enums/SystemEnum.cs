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
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("系统错误")]
        SystemError = 0,
        [DisplayText("录入信息有误，请确认后重试。")]
        ParaError = -2,
        [DisplayText("版本号不能为空")]
        VersionError = -3
    }
    /// <summary>
    /// 设置显示页数
    /// </summary>
    /// <UpdateBy>hulingbo</UpdateBy>
    /// <UpdateTime>20150706</UpdateTime>
    public enum PageSizeEnum
    {
        Web_PageSize = 15,
        App_PageSize = 50
    }

}
