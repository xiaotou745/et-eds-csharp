using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 备用金支出model
    /// 2015年8月13日10:33:25
    /// 茹化肖
    /// </summary>
    public class ImprestPayoutPM
    {
        /// <summary>
        /// 支出金额
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OprName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作类型 1 收入 2 骑士支出
        /// </summary>
        public int OptType { get; set; }
        /// <summary>
        /// 骑士姓名
        /// </summary>
        public string ClienterName { get; set; }
        /// <summary>
        /// 骑士电话
        /// </summary>
        public string ClienterPhoneNo { get; set; }
    }
}
