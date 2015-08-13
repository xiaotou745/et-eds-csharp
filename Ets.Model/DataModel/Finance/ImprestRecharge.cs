using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 实体类ImprestRecharge 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-08-12 16:49:48  
    /// add by  caoheyang 
    /// </summary>
    public class ImprestRecharge
    {/// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 充值总金额
        /// </summary>
        public decimal TotalRecharge { get; set; }
        /// <summary>
        /// 剩余金额
        /// </summary>
        public decimal RemainingAmount { get; set; }
        /// <summary>
        /// 累计支出
        /// </summary>
        public decimal TotalPayment { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
