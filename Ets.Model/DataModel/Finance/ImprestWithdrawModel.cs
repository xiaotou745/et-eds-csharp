using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 骑士备用金提现
    /// 2015年8月12日18:44:26
    /// 茹化肖
    /// </summary>
    public class ImprestWithdrawModel
    {
        /// <summary>
        /// 骑士ID
        /// </summary>
        public int ClienterId { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal WithdrawPrice { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 骑士手机号
        /// </summary>
        public string PhoneNum { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OprName { get; set; }
    }
}
