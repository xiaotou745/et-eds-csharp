using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 商户流水 查询实体类  dd by caoheyang 20150514
    /// </summary>
    public class BussinessRecordsPM
    {
        /// <summary>
        /// 商户ID
        /// </summary>
              [Range(1, int.MaxValue, ErrorMessage = "商户无效")]
        public int BusinessId { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        [Required(ErrorMessage = "版本号不能为空")]
        public string Version { get; set; }
    }
}
