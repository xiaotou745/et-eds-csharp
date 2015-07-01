using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 一键发单修改地址和电话入参对象
    /// </summary>
  public  class NewAddressPM
    {
      /// <summary>
      /// 订单id
      /// </summary>
      public  string OrderId{get;set;}
      /// <summary>
      /// 订单地址
      /// </summary>
      public string NewAddress{get;set;}
      /// <summary>
      /// 订单电话
      /// </summary>
      public string NewPhone { get; set; }
    }
}
