using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    public class SSOrderPM
    {
        public string data { get; set; }
    }
    /// <summary>
    /// 订单 查询实体类 
    /// </summary>
    public class SSOrderCancelPM
    {
        
        /// <summary>
        /// 操作人id
        /// </summary>
        public int OptUserId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OptUserName { get; set; }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string OptLog { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 平台属性：0：商家端;1：配送端;2：服务平台;3：管理后台 4 第三方对接平台
        /// </summary>
        public int Platform { get; set; }

        public string data { get; set; }
    }
}
