using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public class OrderOther
    {
        public int Id { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }
        public int OrderId { get; set; }
        public int NeedUploadCount { get; set; }
        public string ReceiptPic { get; set; }
        public int HadUploadCount { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime OrderCreateTime { get; set; }
    }
}
