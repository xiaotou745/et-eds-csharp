using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 骑士绑定银行卡功能 参数实体 add by caoheyang 20150511
    /// </summary>
    public class CardBindCPM:CardBindPM
    {
        /// <summary>
        /// 骑士ID
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "骑士不能为空")]
        public int ClienterId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required(ErrorMessage = "身份证号不能为空")]
        public string IDCard { get; set; }
    }
}
