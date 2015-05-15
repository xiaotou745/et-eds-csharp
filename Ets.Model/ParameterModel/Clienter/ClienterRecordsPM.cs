using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 骑士流水  查询实体类 add by caoheyang 20150514
    /// </summary>
    public class ClienterRecordsPM
    {
        /// <summary>
        /// 骑士ID
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "骑士无效")]
        public int ClienterId { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        [Required(ErrorMessage = "版本号不能为空")]
        public string Version { get; set; }
    }
}
