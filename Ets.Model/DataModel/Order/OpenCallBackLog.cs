using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    /// <summary>
    /// 实体类OpenCallBackLog 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: wangyuchuan
    /// Generate Time: 2016-02-18 10:40:23
    /// </summary>
    public class OpenCallBackLog
    {
        public OpenCallBackLog() { }
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 请求地址URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 请求体(包括get post参数)
        /// </summary>
        public string RequestBody { get; set; }
        /// <summary>
        /// 响应体(JSON)
        /// </summary>
        public string ResponseBody { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }

}
