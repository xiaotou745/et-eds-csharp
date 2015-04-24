using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 得到推送通知 从第三方集团拉取订单参数实体 add by caoheyang 20150420
    /// </summary>
    public class PullOrderInfoPM_OpenApi
    {   
        /// <summary>
        /// 第三方店铺id
        /// </summary>
        [Required]
        public int store_id { get; set; }
    }
}
