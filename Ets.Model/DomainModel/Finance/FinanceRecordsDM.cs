using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    /// <summary>
    ///骑士/商户交易流水API接口返回实体 add by caoheyang 20150512
    /// </summary>
    public class FinanceRecordsDM
    {
        /// <summary>
        /// 自增ID（PK）
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 骑士/商户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 流水金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 流水状态(1、交易成功 2、交易中）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 流水状态(1、交易成功 2、交易中）
        /// </summary>
        public string StatusStr { get; set; }

        /// <summary>
        /// 交易后余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 交易类型(1订单餐费 2配送费 3提现 4充值)
        /// </summary>
        public int RecordType { get; set; }

        /// <summary>
        /// 交易类型(1订单餐费 2配送费 3提现 4充值)
        /// </summary>
        public string RecordTypeStr { get; set; }

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

        /// <summary>
        /// 时间信息小时分  eg 15:32
        /// </summary>
        public string TimeInfo { get; set; }

        /// <summary>
        /// 日期信息简写 eg 05-13
        /// </summary>
        public string DateInfo { get; set; }

        /// <summary>
        /// 月份信息 eg 2015-03
        /// </summary>
        public string MonthInfo { get; set; }
    }
}
