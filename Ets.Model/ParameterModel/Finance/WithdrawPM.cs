using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 商户/骑士提现功能API实体 add by caoheyang 20150509
    /// 父类
    /// </summary>
    public class WithdrawPM
    {
        /// <summary>
        /// 用于提现的金融帐号id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "提现金融帐号不能为空")]
        public int FinanceAccountId { get; set; } 
        /// <summary>
        /// 版本
        /// </summary>
        [Required(ErrorMessage = "版本号不能为空")]
        public string Version { get; set; }

        /// <summary>
        /// 省名称
        /// </summary>
        [Required(ErrorMessage = "省份不能为空")]
        public string OpenProvince { get; set; }
        /// <summary>
        /// 市区名称
        /// </summary>
        [Required(ErrorMessage = "城市不能为空")]
        public string OpenCity { get; set; } 
        /// <summary>
        /// 省代码
        /// </summary>
        public int OpenProvinceCode { get; set; }
        /// <summary>
        /// 市区代码
        /// </summary>
        public int OpenCityCode { get; set; }
    }
}
