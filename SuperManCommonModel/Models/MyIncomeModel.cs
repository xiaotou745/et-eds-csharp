using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    /// <summary>
    /// 我的收入
    /// </summary>
    public class MyIncomeModel
    {
        public string PhoneNo { get; set; }
        public string MyIncome { get; set; }
        public decimal? MyInComeAmount { get; set; }
        public DateTime? InsertTime { get; set; }
    }
}
