using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    public class RemoveAddressBPM
    {
        ///<summary>
        /// 商户ID
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "商户不能为空")]
        public int BusinessId { get; set; }

        ///<summary>
        /// 数据库地址id
        /// </summary>
        public long AddresssId { get; set; }
        ///<summary>
        /// 版本号1.0
        /// </summary>
        /// <summary>
        /// 版本
        /// </summary>
        [Required(ErrorMessage = "版本号不能为空")]
        public string Version { get; set; }
    }
}
