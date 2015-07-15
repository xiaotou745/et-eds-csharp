using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    public class BankProvinceCityPM
    {
        /// <summary>
        /// API版本号
        /// </summary>
        [Required(ErrorMessage = "API版本号不能为空")]
        public string Version { get; set; }

        /// <summary>
        /// 数据版本号
        /// </summary>
        [Required(ErrorMessage = "数据版本号不能为空")]
        public string DataVersion { get; set; }
    }
}
