using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common.YeePay
{
    public class YeePayUserBalanceRecord
    {
        /// <summary>
        /// 自增ID（PK）
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 易宝账号
        /// </summary>
        public string LedgerNo { get; set; }
        /// <summary>
        /// 提现申请单Id
        /// </summary>
        public long WithwardId { get; set; }
        /// <summary>
        /// 流水金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 交易类型(1主账户向子账户转账 2子账户向主账户转账 3子账户提现 )
        /// </summary>
        public int RecordType { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否有效（1：是 0：否）
        /// </summary>
        public int IsEnable { get; set; }

    }
}
