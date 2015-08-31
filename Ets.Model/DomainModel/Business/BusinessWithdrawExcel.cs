using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Business
{
    public class BusinessWithdrawExcel
    {  
        [Description("商户名称")]
        public string BusinessName { get; set; } 
        [Description("电话")]
        public string BusinessPhoneNo { get; set; }
        [Description("开户行")]
        public string OpenBank { get; set; }
        [Description("账户名")]
        public string TrueName { get; set; }
        [Description("卡号")]
        public string AccountNo { get; set; }
        [Description("提款金额")]
        public decimal Amount { get; set; }
    }
}
