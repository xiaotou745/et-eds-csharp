using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Ets.Model.Common;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 同步订单状态  参数实体 add by caoheyang 20150326
    /// </summary>
    public class AsyncStatusPM_OpenApi
    {
        /// <summary>
        /// 订单号
        /// </summary>

        public string order_no { get; set; }
        /// <summary>
        /// E代送内部订单状态
        /// </summary>
        [Required]
        public int status { get; set; }
        /// <summary>
        /// 超人名称    TODO  后续根据第三方对接集团的增加，可能要扩展成实体对象形式
        /// </summary>
        public string ClienterTrueName { get; set; }


        /// <summary>
        /// 超人电话    TODO  后续根据第三方对接集团的增加，可能要扩展成实体对象形式
        /// </summary>
        public string ClienterPhoneNo { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 第三方订单号
        /// </summary>
        [Required]
        public string OriginalOrderNo { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        public string OtherCancelReason { get; set; }
    }
}
