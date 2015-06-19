using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Statistics
{
    public class BusinessBalanceInfo
    {
        /// <summary>
        /// 充值自增ID
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string RelationNo { get; set; }
        /// <summary>
        /// 商家id
        /// </summary>
        public Int64 BusinessId { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商家帐号（手机号）
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 商家地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 充值日期
        /// </summary>
        public DateTime OperateTime { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }
    }
}
