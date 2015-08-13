using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 备用金支出-骑士手机号验证返回值
    /// </summary>
    public class ImprestClienterModel
    {
        /// <summary>
        /// 返回状态 1 成功 0失败
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 超人Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 帐户余额
        /// </summary>
        public decimal? AccountBalance { get; set; }
        /// <summary>
        /// 可提现余额
        /// </summary>
        public decimal AllowWithdrawPrice { get; set; }
        /// <summary>
        /// 提现中余额
        /// </summary>
        public decimal WithdrawingPrice { get; set; }

        /// <summary>
        /// 备用金余额
        /// </summary>
        public decimal ImprestPrice { get; set; }

    }
    /// <summary>
    /// 点击提现返回值
    /// </summary>
    public class ImprestPayoutModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
