using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    public class BusinessOptionLog
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商户Id
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public int OptId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }
        /// <summary>
        /// 写入时间
        /// </summary>
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 平台属性
        /// </summary>
        public int Platform { get; set; }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal RechargeAmount { get; set; }
        /// <summary>
        /// 赠送金额
        /// </summary>
        public decimal RechargeAmountFree { get; set; }
        /// <summary>
        /// 1 充值 2 赠送 3 充值加赠送
        /// </summary>
        public int RechargeType { get; set; }

    }
    /// <summary>
    /// 商户充值实体类
    /// </summary>
    public class BusinessRechargeLog
    {
       
        /// <summary>
        /// 商户Id
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal RechargeAmount { get; set; }
        /// <summary>
        /// 1 充值 2 赠送 3 充值加赠送
        /// </summary>
        public int RechargeType { get; set; }

    }
}
