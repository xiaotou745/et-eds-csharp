using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Business
{
    public class BusinessCommissionExcel
    {
        [Description("商户名称")]
        public string BusinessName { get; set; }
        [Description("订单金额")]
        public decimal Amount { get; set; }
        [Description("订单数量")]
        public int OrderCount { get; set; }
        [Description("结算比例(%)")]
        public decimal BusinessCommission { get; set; }
        [Description("开始时间")]
        public DateTime StartTime { get; set; }
        [Description("结束时间")]
        public DateTime EndTime { get; set; }
        [Description("结算金额")]
        public decimal TotalAmount { get; set; } 
    }
}
