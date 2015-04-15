using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Subsidy
{
    public class CrossShopModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 骑士ID
        /// </summary>
        public int ClienterId { get; set; }

        /// <summary>
        /// 奖励金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 商户数量
        /// </summary>
        public int BusinessCount { get; set; }

        /// <summary>
        /// 平台属性：0：商家端;1：配送端;2：服务平台;3：管理后台
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 写入时间
        /// </summary>
        public DateTime InsertTime { get; set; }

    }
}
