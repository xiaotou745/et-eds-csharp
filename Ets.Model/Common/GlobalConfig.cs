using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    public class GlobalConfig
    {
        /// <summary>
        /// 自增ID（PK）
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        public string KeyName { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public DateTime Remark { get; set; }
        /// <summary>
        /// 分组Id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 策略Id
        /// </summary>
        public int StrategyId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }
    }
}
