using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public class OrderMapDetail
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 商户发单经度
        /// </summary>
        public double PubLongitude { get; set; }
        /// <summary>
        /// 商户发单纬度
        /// </summary>
        public double PubLatitude { get; set; }
        /// <summary>
        /// 商户发单时间
        /// </summary>
        public string PubDate { get; set; }
        /// <summary>
        /// 骑士抢单经度
        /// </summary>
        public double GrabLongitude { get; set; }
        /// <summary>
        /// 骑士抢单纬度
        /// </summary>
        public double GrabLatitude { get; set; }
        /// <summary>
        /// 骑士抢单时间
        /// </summary>
        public string GrabTime { get; set; }
        /// <summary>
        /// 骑士取货经度
        /// </summary>
        public double TakeLongitude { get; set; }
        /// <summary>
        /// 骑士取货纬度
        /// </summary>
        public double TakeLatitude { get; set; }
        /// <summary>
        /// 骑士取货时间
        /// </summary>
        public string TakeTime { get; set; }
        /// <summary>
        /// 骑士完成订单经度
        /// </summary>
        public double CompleteLongitude { get; set; }
        /// <summary>
        /// 骑士完成订单纬度
        /// </summary>
        public double CompleteLatitude { get; set; }
        /// <summary>
        /// 骑士完成订单时间
        /// </summary>
        public string ActualDoneDate { get; set; }
    }
}
