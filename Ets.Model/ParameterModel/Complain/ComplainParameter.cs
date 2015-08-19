using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Complain
{   
    public class ComplainParameter
    {  
        /// <summary>
        /// 举报人商户Id
        /// </summary>
        [Required(ErrorMessage = "投诉人Id不能为空")]
        public int ComplainId { get; set; }
        /// <summary>
        /// 被举报人骑士Id
        /// </summary>
         [Required(ErrorMessage = "被投诉人Id不能为空")]
        public int ComplainedId { get; set; }
        /// <summary>
        /// 举报原因
        /// </summary>
        [Required(ErrorMessage = "举报原因不能为空")]
        public string Reason { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        [Required(ErrorMessage = "订单Id不能为空")]
        public int OrderId { get; set; } 
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空")]
        public string OrderNo { get; set; }
    }

}
