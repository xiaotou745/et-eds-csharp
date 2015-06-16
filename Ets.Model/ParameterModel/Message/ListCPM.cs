using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Message
{
    /// <summary>
    /// 商户端获取消息列表接口参数 add by caoheyang 20150615
    /// </summary>
    public class ListCPM
    {
        /// <summary>
        /// 骑士Id
        /// </summary>
        public int ClienterId { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 版本号1.0
        /// </summary>
        [Required(ErrorMessage = "版本号不能为空")]
        public string Version { get; set; }

    }
}
