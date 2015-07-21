using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 骑士修改绑定银行卡功能 参数实体 add by caoheyang 20150511
    /// </summary>
    public class CardModifyCPM:CardModifyPM
    {
        /// <summary>
        /// 骑士ID
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "骑士不能为空")]
        public int ClienterId { get; set; }
        /// <summary>
        /// 身份证号 
        /// </summary> 
        [RegularExpression(Config.IDCARD_REG, ErrorMessage = "请正确填写18位有效身份证号码")]
        public string IDCard { get; set; }
    }
}
