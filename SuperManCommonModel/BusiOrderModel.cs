using SuperManCommonModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel
{
    /// <summary>
    /// 向B端后台返回的数据
    /// </summary>
    public class BusiOrderModel
    {
        public OrderManageList orderManageList { get; set; }
    }
}
