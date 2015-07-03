using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    /// <summary>
    /// 版本检查参数model
    /// </summary>
    public class VersionCheckModel
    {
        /// <summary>
        /// 客户端类型 1:Android 2 :IOS 默认Android
        /// </summary>
        public int PlatForm { get; set; }
        /// <summary>
        /// 用户版本 1 骑士 2 商家 默认1骑士
        /// </summary>
        public int UserType { get; set; }
    }
}
