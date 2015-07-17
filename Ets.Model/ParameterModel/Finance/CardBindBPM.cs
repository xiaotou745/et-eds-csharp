using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 商户绑定银行卡功能 参数实体 add by caoheyang 20150511
    /// </summary>
    public class CardBindBPM:CardBindPM
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "商家不能为空")]
        public int BusinessId { get; set; }
        //TODO 身份证和营业执照分开
        /// <summary>
        /// 营业执照号 
        /// </summary>
        public string BusinessLicence { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }
    }
}
