using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Statistics
{
   public class BussinessBalanceQuery
    {
       /// <summary>
       /// 开始执日期
       /// </summary>
       public string StartDate { get; set; }
       /// <summary>
       /// 结束日期
       /// </summary>
       public string EndDate { get; set; }
       /// <summary>
       /// 页码（从1开始）
       /// </summary>
       public int PageIndex { get; set; }
    }
}
