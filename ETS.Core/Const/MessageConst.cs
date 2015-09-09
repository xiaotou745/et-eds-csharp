using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Const
{
    public class MessageConst
    {
        /// <summary>
        /// 确认打款
        /// </summary>
        public const string ConfirmPlayMoney = "尊敬的E代送骑士您好，您{0}月{1}日提现的{2}元，已通过审核，系统将在1-2个工作日内进行打款，请注意查收。如有任何疑问请致电：010-57462756";
        /// <summary>
        /// 打款成功
        /// </summary>
        public const string PlayMoneySuccess = "尊敬的E代送骑士您好，您于{0}月{1}日提现的{2}元已打款到账，请您注意查收。如有任何疑问请致电：010-57462756";
        /// <summary>
        /// 打款失败
        /// </summary>
        public const string PlayMoneyFailure = "尊敬的E代送骑士您好，您的{0}月{1}日提现申请打款失败，提现金额已退回您的账户余额。具体原因为：{2}。如有任何疑问请致电：010-57462756";

        /// <summary>
        /// 审核拒绝
        /// </summary>
        public const string AuditRejection = "尊敬的骑士您好，您{0}月{1}日提现申请已被拒绝，相应余额已退回账户，具体原因为：{2}！如有疑问请致电010-57462756";

    }
}
