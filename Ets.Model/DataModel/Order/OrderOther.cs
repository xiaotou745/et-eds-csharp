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

        /// <summary>
        /// 商户发单经度
        /// </summary>
        public decimal? PubLongitude { get; set; }
        /// <summary>
        /// 商户发单纬度
        /// </summary>
        public decimal? PubLatitude { get; set; }
        /// <summary>
        /// 骑士抢单时间
        /// </summary>
        public DateTime? GrabTime { get; set; }
        /// <summary>
        /// 骑士抢单经度
        /// </summary>
        public decimal? GrabLongitude { get; set; }
        /// <summary>
        /// 骑士抢单纬度
        /// </summary>
        public decimal? GrabLatitude { get; set; }
        /// <summary>
        /// 骑士完成订单经度
        /// </summary>
        public decimal? CompleteLongitude { get; set; }
        /// <summary>
        /// 骑士完成订单纬度
        /// </summary>
        public decimal? CompleteLatitude { get; set; }
        /// <summary>
        /// 取货时间
        /// </summary>
        public DateTime? TakeTime { get; set; }
        /// <summary>
        ///  取货经度
        /// </summary>
        public decimal? TakeLongitude { get; set; }
        /// <summary>
        /// 取货纬度 
        /// </summary>
        public decimal? TakeLatitude { get; set; }

        /// <summary>
        /// 是否允许修改小票
        /// </summary>
        public bool IsModifyTicket { get; set; }


    }
}
