using System;

namespace Ets.Model.DataModel.Strategy
{
    public class StrategyModel
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 策略名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 策略ID
        /// </summary>
        public int StrategyId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更改人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 最后更改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
