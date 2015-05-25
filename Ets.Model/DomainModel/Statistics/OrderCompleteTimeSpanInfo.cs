using System;

namespace Ets.Model.DomainModel.Statistics
{

    /// <summary>
    /// 订单完成时间间隔统计查询条件
    /// </summary>
    public class ParamOrderCompleteTimeSpan
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 是否按照城市来查询
        /// </summary>
        public bool AsCityQuery { get; set; }
    }

    /// <summary>
    /// 订单完成时间间隔统计对象
    /// </summary>
    public class OrderCompleteTimeSpanInfo
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 任务总数量
        /// </summary>
        public int TaskCount { get; set; }

        /// <summary>
        /// 小于5分钟完成的任务数量
        /// </summary>
        public int LessThanFiveTaskCount { get; set; }

        /// <summary>
        /// 小于5分钟完成的任务占比
        /// </summary>
        public decimal LessThanFiveRate { get; set; }

        /// <summary>
        /// 5分钟-10分钟完成任务数量
        /// </summary>
        public int FiveToTenTaskCount { get; set; }

        /// <summary>
        /// 5-10分钟完成任务占比
        /// </summary>
        public decimal FiveToTenRate { get; set; }

        /// <summary>
        /// 10-15分钟完成的任务数量
        /// </summary>
        public int TenToFifteenTaskCount { get; set; }

        /// <summary>
        /// 10-15分钟完成的任务占比
        /// </summary>
        public decimal TenToFifteenRate { get; set; }

        /// <summary>
        /// 大于15分钟完成的任务数量
        /// </summary>
        public int GreaterThanFifteenCount { get; set; }

        /// <summary>
        /// 大于15分钟完成的任务占比
        /// </summary>
        public decimal GreaterThanFifteenRate { get; set; }

        /// <summary>
        /// 大于2小时完成的任务数量
        /// </summary>
        public int GreaterThanTwoHoursCount { get; set; }

        /// <summary>
        /// 大于1天完成的任务数量
        /// </summary>
        public int GreaterThanOneDayCount { get; set; }
    }

    /// <summary>
    /// 城市级别统计
    /// </summary>
    public class CityOrderCompleteTimeSpanInfo : OrderCompleteTimeSpanInfo
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 城市任务数量
        /// </summary>
        public int CityTaskCount { get; set; }

        /// <summary>
        /// 城市任务数量占比
        /// </summary>
        public decimal CityTaskRate { get; set; }
    }
}