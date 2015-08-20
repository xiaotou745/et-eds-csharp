using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Finance;

namespace Ets.Model.DomainModel.Finance
{
    public class ClienterFinanceAccountModel : ClienterFinanceAccount
    {
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 手续费阈值,例如100
        /// </summary>
        public decimal HandChargeThreshold { get; set; }
        /// <summary>
        /// 手续费,例如1元
        /// </summary>
        public decimal HandCharge { get; set; }
        /// <summary>
        /// 手续费支出方:0个人,1易代送
        /// </summary>
        public int HandChargeOutlay { get; set; }
        /// <summary>
        /// 提现时间
        /// </summary>
        public DateTime WithdrawTime { get; set; }
        /// <summary>
        /// 提现人身份证号
        /// </summary>
        public string CliIDCard { get; set; }
        /// <summary>
        /// 本系统易宝账户余额
        /// </summary>
        public decimal BalanceRecord { get; set; }
        /// <summary>
        /// 易宝系统易宝账户余额
        /// </summary>
        public decimal YeeBalance { get; set; }
        /// <summary>
        /// 打款处理状态（0：待处理 1：已处理）
        /// </summary>
        public int DealStatus { get; set; }
        /// <summary>
        /// 打款处理次数
        /// </summary>
        public int DealCount { get; set; }
        /// <summary>
        /// 提现申请单Id
        /// </summary>
        public long WithwardId { get; set; }
        /// <summary>
        /// 提现申请单号
        /// </summary>
        public string WithwardNo { get; set; }
        /// <summary>
        /// 提现单状态（1待审核 2 审核通过 3打款完成 20打款中 -1审核拒绝 -2 打款失败 4 打款异常）
        /// </summary>
        public int WithdrawStatus { get; set; }
        /// <summary>
        /// 提现失败原因
        /// </summary>
        public string PayFailedReason { get; set; }
        
        
    }
}
