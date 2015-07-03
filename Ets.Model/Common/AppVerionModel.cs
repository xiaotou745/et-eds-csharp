using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    public class AppVerionModel
    {
        public AppVerionModel()
        {
            Version = "";
            IsMust = false;
            UpdateUrl = "";
            Message = "";
        }
        /// <summary>
        /// 当前版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 是否强制升级 1 是 0否 默认0
        /// </summary>
        public bool IsMust { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string UpdateUrl { get; set; }

        /// <summary>
        /// 升级信息 可以不填
        /// </summary>
        public string Message { get; set; }

    }
}
