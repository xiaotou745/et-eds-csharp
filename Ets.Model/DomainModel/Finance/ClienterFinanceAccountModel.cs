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
        
    }
}
