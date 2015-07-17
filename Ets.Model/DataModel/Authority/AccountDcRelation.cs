using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Authority
{
    public  class AccountDcRelation
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// 物流公司ID
        /// </summary>
        public int DeliveryCompanyID { get; set; }
        /// <summary>
        /// 创建或修改人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 是否启用 1 启用 0 不启用
        /// </summary>
        public int IsEnable { get; set; }
    }
}
