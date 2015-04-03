/*
 * 配置类
 * 2015年4月2日15:47:15
 */

namespace Task.Model
{
    public class Config
    {
        /// <summary>
        ///     查询的条数。
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        ///     连接字符串（读）
        /// </summary>
        public string ReadConnectionString { get; set; }
        /// <summary>
        ///     连接字符串(写)
        /// </summary>
        public string WriteConnectionString { get; set; }
        /// <summary>
        ///     最小订单数量。
        /// </summary>
        public int MinOrderQty { get; set; }
        /// <summary>
        ///     最大订单数量。
        /// </summary>
        public int MaxOrderQty { get; set; }
        /// <summary>
        /// 间隔分钟数
        /// </summary>
        public int IntervalMinute { get; set; }
        /// <summary>
        /// 间隔分钟数
        /// </summary>
        public decimal AdjustAmount { get; set; }
        /// <summary>
        /// 超时分钟数集合
        /// </summary>
        public string IntervalMinuteList { get; set; }
    }
}