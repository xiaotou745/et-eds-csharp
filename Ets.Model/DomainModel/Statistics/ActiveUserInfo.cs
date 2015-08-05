using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Statistics
{
   public class ActiveUserInfo
    {
       /// <summary>
       /// 用户编号（骑士id或商家id）
       /// </summary>
       public int UserID { get; set; }
       /// <summary>
       /// 用户名称（骑士名称或商家名称）
       /// </summary>
       public string UserName { get; set; }
       /// <summary>
       /// 注册手机号
       /// </summary>
       public string PhoneNo { get; set; }
       /// <summary>
       /// 任务数量（对骑士来说，指完成（点过完成）任务量，对商家来说，指发布的任务量）
       /// </summary>
       public int TaskNum { get; set; }
       /// <summary>
       /// 订单数量
       /// </summary>
       public int OrderNum { get; set; }
    }
}
