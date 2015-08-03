using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 实体类ClienterAllowWithdrawRecordDTO 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-08-03 15:34:50
    /// </summary>
    public class ClienterAllowWithdrawRecord
    {
        /// <summary>
        /// 自增ID（PK）
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 骑士Id(Clienter表）
        /// </summary>
        public int ClienterId { get; set; }
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
