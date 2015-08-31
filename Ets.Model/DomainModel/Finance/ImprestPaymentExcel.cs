using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    public class ImprestPaymentExcel
    {
        [Description("骑士姓名")]
        public string ClienterName { get; set; }
        [Description("电话")]
        public string ClienterPhoneNo { get; set; }
        [Description("支出金额")]
        public decimal Amount { get; set; }
        [Description("余额")]
        public decimal AfterAmount { get; set; }
        [Description("日期")]
        public DateTime OptTime { get; set; }
        [Description("操作人")]
        public string OptName { get; set; }
        [Description("备注")]
        public string Remark { get; set; }
    }
}
