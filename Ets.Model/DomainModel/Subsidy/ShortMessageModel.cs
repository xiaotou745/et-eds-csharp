using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Subsidy
{
    /// <summary>
    /// 抢单量
    /// </summary>
    public class ShortMessageModel
    {  
        /// <summary>
        /// 创建时间日期
        /// </summary>
        public string InsertTime { get; set; }
        /// <summary>
        /// 奖励金额
        /// </summary>
        public decimal SumAmount { get; set; }

        /// <summary>
        /// 骑士ID
        /// </summary>
        public string ClienterId { get; set; }

        /// <summary>
        /// 骑士电话
        /// </summary>
        public string PhoneNo { get; set; }
    }
}
