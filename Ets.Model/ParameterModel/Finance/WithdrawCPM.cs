using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 骑士提现功能API实体 add by caoheyang 20150509
    /// </summary>
    public class WithdrawCPM:WithdrawPM
    {
        /// <summary>
        /// 骑士id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "骑士不能为空")]
        public int ClienterId { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        [Range(0.01, int.MaxValue, ErrorMessage = "提现金额不能小于0.01元")]
        public decimal WithdrawPrice { get; set; }
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
        /// 身份证号
        /// </summary>
        [Required(ErrorMessage = "身份证号不能为空")]
        public string IDCard { get; set; }
        /// <summary>
        /// 省代码
        /// </summary>
        [Required(ErrorMessage = "省份代码不能为空")]
        public int OpenProvinceCode { get; set; }
        /// <summary>
        /// 市区代码
        /// </summary>
        [Required(ErrorMessage = "城市代码不能为空")]
        public int OpenCityCode { get; set; }
    }
}
