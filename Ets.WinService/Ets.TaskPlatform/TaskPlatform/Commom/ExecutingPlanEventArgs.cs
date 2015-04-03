using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.Commom
{
    public class ExecutingPlanEventArgs : EventArgs
    {
        /// <summary>
        /// 计划任务名称
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime ExecuteTime { get; set; }

        /// <summary>
        /// 执行类型
        /// </summary>
        public ExecuteType ExecuteType { get; set; }

        /// <summary>
        /// 计划任务名称
        /// </summary>
        public string Information { get; set; }
    }

    /// <summary>
    /// 执行类型
    /// </summary>
    public enum ExecuteType
    {
        /// <summary>
        /// 执行前
        /// </summary>
        Before,
        /// <summary>
        /// 执行中
        /// </summary>
        Executing,
        /// <summary>
        /// 执行后
        /// </summary>
        After
    }
}
