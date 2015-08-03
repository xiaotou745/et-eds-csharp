using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 商户提现日志表实体类BusinessBalanceRecordDTO 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com  caoheyang
    /// Generate Time: 2015-05-11 16:37:45
    /// </summary>
    public class BusinessWithdrawLog
    {/// <summary>
        /// 自增ID(PK)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 提现单ID
        /// </summary>
        public long WithwardId { get; set; }
        /// <summary>
        /// 操作后状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatTime { get; set; }
        /// <summary>
        /// 之前状态
        /// </summary>
        public int OldStatus { get; set; }


    }

}
