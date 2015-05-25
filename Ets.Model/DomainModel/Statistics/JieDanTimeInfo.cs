using System;

namespace Ets.Model.DomainModel.Statistics
{
    /// <summary>
    /// 发单-接单-完成 时长统计
    /// </summary>
    public class JieDanTimeInfo
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 任务总量
        /// </summary>
        public int TaskCount { get; set; }

        /// <summary>
        /// 骑士总数量
        /// </summary>
        public int ClienterCount { get; set; }

        /// <summary>
        /// 日人均任务量=任务总量/骑士总数量
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public decimal AvgClienterCount { get; set; }

        /// <summary>
        /// 活跃骑士数量
        /// </summary>
        public int ActiveClienterCount { get; set; }

        /// <summary>
        /// 日在线人均任务量
        /// </summary>
        public decimal AvgActiveClienterCount { get; set; }

        /// <summary>
        /// 发单-接单总时长（秒）
        /// </summary>
        public long PubReciveTotalSeconds { get; set; }

        /// <summary>
        /// 日均接单时长(分钟)
        /// </summary>
        public decimal AvgPubReceiveMinutes { get; set; }

        /// <summary>
        /// 发单-完成总时长（秒）
        /// </summary>
        public long PubCompleteTotalSeconds { get; set; }

        /// <summary>
        /// 日均配送时长(分钟)
        /// </summary>
        public decimal AvgPubCompleteMinutes { get; set; }
    }

    public class ParamJieDanTimeInfo
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
}