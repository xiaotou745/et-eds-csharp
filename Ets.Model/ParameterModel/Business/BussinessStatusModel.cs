using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// B端用户状态
    /// </summary>
    public class BussinessStatusModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 一健发单
        /// </summary>
        public int OneKeyPubOrder { get; set; }
        /// <summary>
        /// 是否允许透支，1允许，0不允许
        /// </summary>
        public int IsAllowOverdraft { get; set; }
        /// <summary>
        /// 商户余额
        /// </summary>
        public decimal BalancePrice { get; set; }
    }
}
