using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Message
{
    /// <summary>
    /// 商户端获取消息列表接口参数 add by caoheyang 20150615
    /// </summary>
    public class ListBPM
    {
        /// <summary>
        /// 商户Id
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 版本号1.0
        /// </summary>
        public string Version { get; set; }

    }
}
