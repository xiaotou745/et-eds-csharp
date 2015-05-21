using System;
using System.Collections.Generic;

namespace Ets.Model.DomainModel.Statistics
{
    /// <summary>
    /// 每小时任务数量
    /// </summary>
    public class TaskStatisticsPerHourInfo
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        public IList<int> TaskCounts { get; set; }

        ///// <summary>
        ///// 0点任务数量
        ///// </summary>
        //public int Count0 { get; set; }

        ///// <summary>
        ///// 1点任务数量
        ///// </summary>
        //public int Count1 { get; set; }

        //public int Count2 { get; set; }
        //public int Count3 { get; set; }
        //public int Count4 { get; set; }
        //public int Count5 { get; set; }
        //public int Count6 { get; set; }
        //public int Count7 { get; set; }
        //public int Count8 { get; set; }
        //public int Count9 { get; set; }
        //public int Count10 { get; set; }
        //public int Count11 { get; set; }
        //public int Count12 { get; set; }
        //public int Count13 { get; set; }
        //public int Count14 { get; set; }
        //public int Count15 { get; set; }
        //public int Count16 { get; set; }
        //public int Count17 { get; set; }
        //public int Count18 { get; set; }
        //public int Count19 { get; set; }
        //public int Count20 { get; set; }
        //public int Count21 { get; set; }
        //public int Count22 { get; set; }
        //public int Count23 { get; set; }
    }

    public class ParamTaskPerHour
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