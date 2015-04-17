using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Bussiness
{
    public class BusinessesDistributionModel
    {

        /// <summary>
        /// 创建时间日期
        /// </summary>
        public string InsertTime { get; set; }
        /// <summary>
        /// 商家数量
        /// </summary>
        public int BusinessCount { get; set; }
        /// <summary>
        /// 奖励金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 奖励金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 骑士数量
        /// </summary>
        public int ClienterCount { get; set; }


    }
}
