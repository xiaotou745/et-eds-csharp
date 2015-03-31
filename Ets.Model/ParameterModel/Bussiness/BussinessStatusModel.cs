using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Bussiness
{
    /// <summary>
    /// B端用户状态
    /// </summary>
    public class BussinessStatusModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } 
        /// <summary>
        /// 结算比例
        /// </summary>
        public decimal BusinessCommission { get; set; }
        /// <summary>
        /// 外送费
        /// </summary>
        public decimal DistribSubsidy { get; set; }
    }
}
