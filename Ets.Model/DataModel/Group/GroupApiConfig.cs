using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Group
{   
    /// <summary>
    /// 集团appkey add by caoheyang 20150327
    /// </summary>
    public class GroupApiConfig
    {
        /// <summary>
        /// 应用id
        /// </summary>
        public int AppId { get; set; }
        /// <summary>
        /// AppKey
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// api版本
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// 集团id，万达 3
        /// </summary>
        public int GroupId { get; set; }
    }
}
