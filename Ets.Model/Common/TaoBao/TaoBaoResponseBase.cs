using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common.TaoBao
{
    /// <summary>
    /// 淘宝 api 调用 返回值数据结构
    /// caoheyang 20151116
    /// </summary>
   public class TaoBaoResponseBase
    {
        /// <summary>
        ///  调用是否成功
        /// </summary>
        public bool? is_success { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string error_code { get; set; }
        /// <summary>
        ///  错误信息
        /// </summary>
        public string error_msg { get; set; }
        /// <summary>
        /// 取件是否成功
        /// </summary>
        public bool? result { get; set; }
    }
}
