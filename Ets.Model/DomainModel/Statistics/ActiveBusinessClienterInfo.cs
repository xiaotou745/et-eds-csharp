using System;

namespace Ets.Model.DomainModel.Statistics
{


    /// <summary>
    /// 活跃商家、骑士统计
    /// </summary>
    public class ActiveBusinessClienterInfo
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 通过审核商家数量
        /// </summary>
        public int BusinessCount { get; set; }

        /// <summary>
        /// 活跃商家数量
        /// </summary>
        public int ActiveBusinessCount { get; set; }

        /// <summary>
        /// 活跃商家数量占比
        /// </summary>
        public decimal ActiveBusinessRate { get; set; }

        /// <summary>
        /// 通过审核骑士数量
        /// </summary>
        public int ClienterCount { get; set; }

        /// <summary>
        /// 活跃骑士数量
        /// </summary>
        public int ActiveClienterCount { get; set; }

        /// <summary>
        /// 活跃骑士数量占比
        /// </summary>
        public decimal ActiveClienterRate { get; set; }

        ///// <summary>
        ///// 任务数量
        ///// </summary>
        //public int TaskCount { get; set; }

        ///// <summary>
        ///// 订单数量
        ///// </summary>
        //public int OrderCount { get; set; }

        ///// <summary>
        ///// 总订单金额 
        ///// </summary>
        //public decimal TotalPrice { get; set; }

        ///// <summary>
        ///// 任务单价
        ///// </summary>
        //public decimal AverageTaskPrice { get; set; }

        ///// <summary>
        ///// 客单价
        ///// </summary>
        //public decimal AverageOrderPrice { get; set; }
    }

    /// <summary>
    /// 各城市活跃商家、骑士统计
    /// </summary>
    public class CityActiveBusinessClienterInfo : ActiveBusinessClienterInfo
    {
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
    }

    /// <summary>
    /// 活跃统计查询条件
    /// </summary>
    public class ParamActiveInfo
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
