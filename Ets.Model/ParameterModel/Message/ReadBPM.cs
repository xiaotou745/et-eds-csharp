using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Message
{
    /// <summary>
    /// 商户阅读接口更新消息状态接口参数实体 add by caoheyang 20150615
    /// </summary>
    public class ReadBPM
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public int MessageId { get; set; }
        /// <summary>
        /// 版本号1.0
        /// </summary>
        public string Version { get; set; }

    }
}
