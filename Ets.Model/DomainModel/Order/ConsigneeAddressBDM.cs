using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    /// <summary>
    ///  B端商户拉取收货人地址缓存到本地接口返回参数 add By  caoheyang   20150702
    /// </summary>
    public class ConsigneeAddressBDM
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
        public string PubDate { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

    }
}
