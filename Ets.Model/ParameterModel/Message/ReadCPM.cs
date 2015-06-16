using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Message
{
    /// <summary>
    ///  骑士阅读接口更新消息状态接口参数实体 add by caoheyang 20150615
    /// </summary>
    public class ReadCPM
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public int MessageId { get; set; }
        /// <summary>
        /// 版本号1.0
        /// </summary>
        [Required(ErrorMessage = "版本号不能为空")]
        public string Version { get; set; }
    }
}
