using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 商户余额流水表 实体类BusinessBalanceRecord 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com  caoheyang
    /// Generate Time: 2015-05-11 16:36:08
    /// </summary>
    public class BusinessBalanceRecord
    {
        public BusinessBalanceRecord()
        {
        }

        /// <summary>
        /// 自增ID（PK）
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 商家Id(business表）
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
        /// 交易类型(1订单餐费 2配送费 3提现 4充值)
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
