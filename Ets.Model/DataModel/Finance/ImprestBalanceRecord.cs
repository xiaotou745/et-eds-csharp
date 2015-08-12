using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 实体类ImprestRechargeDTO 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-08-12 17:02:11
    /// add  by caoheyang 
    /// </summary>
    public class ImprestBalanceRecord
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 操作前金额
        /// </summary>
        public decimal BeforeAmount { get; set; }
        /// <summary>
        /// 操作后金额
        /// </summary>
        public decimal AfterAmount { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OptTime { get; set; }
        /// <summary>
        /// 类型：1、充值、2、骑士支出
        /// </summary>
        public int OptType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 骑士姓名
        /// </summary>
        public string ClienterName { get; set; }
        /// <summary>
        /// 骑士手机号
        /// </summary>
        public string ClienterPhoneNo { get; set; }

    }
}
