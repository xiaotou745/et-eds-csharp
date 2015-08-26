using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;

namespace ETS.Enums
{
    /// <summary>
    /// 请求日志记录枚举(哪个平台发起的请求)
    /// </summary>
    public enum RequestPlatFormEnum
    {
        /// <summary>
        /// 未知平台
        /// </summary>
        [DisplayText("未知平台")]
        Unknow = 0,
        /// <summary>
        /// 管理后台
        /// </summary>
        [DisplayText("管理后台")]
        EdsManagePlat = 1,
        /// <summary>
        /// WebApi
        /// </summary>
        [DisplayText("WebApi")]
        WebApiPlat = 2,
        /// <summary>
        /// OpenApi
        /// </summary>
        [DisplayText("OpenApi")]
        OpenApiPlat = 3,
        /// <summary>
        /// 第三方平台
        /// </summary>
        [DisplayText("第三方平台")]
        ThridPlat = 4
    }
    /// <summary>
    /// 请求记录枚举(操作类型)
    /// </summary>
    public enum HtypeEnum
    {
        /// <summary>
        /// 未知
        /// </summary>
        [DisplayText("未知")]
        Unknow = 0,
        /// <summary>
        /// 请求(请求第三方)
        /// </summary>
        [DisplayText("请求(请求第三方)")]
        ReqType = 1,
        /// <summary>
        /// 响应
        /// </summary>
        [DisplayText("响应")]
        RespType = 2,
        /// <summary>
        /// 第三方回调
        /// </summary>
        [DisplayText("第三方回调")]
        ThridCallback = 3
    }
}
