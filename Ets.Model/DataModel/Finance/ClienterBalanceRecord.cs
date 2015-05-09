using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 实体类ClienterBalanceRecordDTO 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 14:59:36
    /// </summary>
    public class ClienterBalanceRecord
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
        /// 交易类型(1佣金 2奖励 3提现 4取消订单赔偿 5无效订单扣款)
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
        /// 关联单号
        /// </summary>
        public string RelationNo { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

    }
}
