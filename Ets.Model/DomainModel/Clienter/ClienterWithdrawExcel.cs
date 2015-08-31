using System;
using System.ComponentModel;
using System.Runtime.Remoting;

namespace Ets.Model.DomainModel.Clienter
{
    public class ClienterWithdrawExcel
    {
        [Description("卡号")]
        public string AccountNo { get; set; }
        [Description("骑士姓名")]
        public string ClienterName { get; set; }
        [Description("提款金额")]
        public string Amount { get; set; }
        [Description("申请时间")]
        public string WithdrawDateStart { get; set; }
        [Description("电话")]
        public string ClienterPhoneNo { get; set; }
        [Description("开户行")]
        public string OpenBank { get; set; }
        [Description("账户名")]
        public string TrueName { get; set; } 
    }
}
