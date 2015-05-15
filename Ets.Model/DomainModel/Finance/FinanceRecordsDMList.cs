using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    /// <summary>
    /// 骑士/商户交易流水API接口返回分组实体  add by caoheyang 20150505
    /// </summary>
    public class FinanceRecordsDMList
    {
        /// <summary>
        /// 月份简写
        /// </summary>
        public string MonthIfo { get; set; }
      
        /// <summary>
        /// 数据实体
        /// </summary>
        public IList<FinanceRecordsDM> Datas { get; set; }
    }
}
