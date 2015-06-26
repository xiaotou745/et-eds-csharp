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
       /// 商户名称
       /// </summary>
       public string Name { get; set; }
       /// <summary>
       /// 商户id
       /// </summary>
       public string BusinessId { get; set; }
       /// <summary>
       /// 商户账号
       /// </summary>
       public string PhoneNo { get; set; }
       /// <summary>
       /// 城市id
       /// </summary>
       public string CityId { get; set; }
       /// <summary>
       /// 排序方式(0为充值时间倒序，1为充值金额降序，2为充值金额升序)
       /// </summary>
       public int OrderType { get; set; }
       /// <summary>
       /// 页码（从1开始）
       /// </summary>
       public int PageIndex { get; set; }

       /// <summary>
       /// 充值类型:1系统充值；2客户端充值
       /// </summary>
       public int RechargeType { get; set; }

       /// <summary>
       /// 充值金额
       /// </summary>
       public decimal RechargePrice { get; set; }
    }
}
