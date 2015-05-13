using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    public class OrderChildForTicket
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 子订单Id
        /// </summary>
        public int ChildId { get; set; }
        /// <summary>
        /// 是否上传上票
        /// </summary>
        public bool HasUploadTicket { get; set; }
        /// <summary>
        /// 小票图片路径
        /// </summary>
        public string TicketUrl { get; set; }
        /// <summary>
        /// 需要上传的总数量
        /// </summary>
        public int NeedUploadCount { get; set; }
        /// <summary>
        /// 已经上传的数量
        /// </summary>
        public int HadUploadCount { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }
    }
}
