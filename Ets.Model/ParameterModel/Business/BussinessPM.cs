using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// 商户 查询实体类 
    /// </summary>
    public class BussinessPM
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int BussinessId { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 订单金额 
        /// add by 彭宜   20150714
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 订单
        /// add by 彭宜   20150714
        /// </summary>
        public int OrderCount { get; set; }
    }
}
