using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    public class ImprestRechargeExcel
    {
        [Description("充值日期")]
        public DateTime ChongZhiDate { get; set; }
        [Description("充值金额")]
        public decimal ChongZhiAmount { get; set; }
        [Description("备用金接收人")]
        public string JieShouRen { get; set; }
        [Description("操作人")]
        public string OptName { get; set; }
        [Description("备注")]
        public string Remark { get; set; } 
    }
}
