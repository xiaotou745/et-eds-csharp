using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// 商户 余额 可提现余额 
    /// </summary>
    public class BusinessMoneyPM
    {
        /// <summary>
        /// 商户Id(Business表）
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 流水金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 流水状态(1、交易成功 2、交易中）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public int RecordType { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }
        /// <summary>
        /// 提现单ID
        /// </summary>
        public long WithwardId { get; set; }
        /// <summary>
        /// 关联单号
        /// </summary>
        public string RelationNo { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
    }
}
