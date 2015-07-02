using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{

    /// <summary>
    /// 收货人地址表  add By  caoheyang   20150702
    /// </summary>
    public class ReceviceAddress
    {
        /// <summary>
        /// 自增ID(PK)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 订单发布日期
        /// </summary>
        public DateTime PubDate { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 商户id
        /// </summary>
        public int BusinessId { get; set; }

    }
}
