using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.PlanEngine
{
    /// <summary>
    /// 触发心跳时所包含的数据
    /// </summary>
    [Serializable]
    public class BeatEventArgs : EventArgs
    {
        /// <summary>
        /// 心跳触发时的时间
        /// </summary>
        public DateTime BeatTime { get; set; }

        /// <summary>
        /// 上次发生心跳的处理时间。
        /// </summary>
        public TimeSpan LastProcessTimeSpan { get; set; }
    }
    /// <summary>
    /// 计划被触发时所包含的数据
    /// </summary>
    [Serializable]
    public class PlanTrigEventArgs : EventArgs
    {
        /// <summary>
        /// 要执行的计划的名称
        /// </summary>
        public string TrigPlanName { get; set; }
        /// <summary>
        /// 触发执行的时间
        /// </summary>
        public DateTime RunTime { get; set; }

        /// <summary>
        /// 上次的计划任务处理时间。
        /// </summary>
        public TimeSpan LastProcessTimeSpan { get; set; }
    }

    /// <summary>
    /// 引擎触发计划时所包含的数据
    /// </summary>
    [Serializable]
    public class ExecutePlanEventArgs : EventArgs
    {
        /// <summary>
        /// 需要执行的计划的名称。
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 需要执行的计划上次执行耗时。
        /// </summary>
        public TimeSpan LastProcessTimeSpan { get; set; }

        /// <summary>
        /// 计划的开始执行时间。
        /// </summary>
        public DateTime ExecuteTime { get; set; }
    }
}
