using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskPlatform.PlanEngine;

namespace TaskPlatform.Commom
{
    public class ExecutedPlanEventArgs : EventArgs
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public string RunResult { get; set; }

        /// <summary>
        /// 执行是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime ExecuteTime { get; set; }

        /// <summary>
        /// 本次执行线程排队耗时
        /// </summary>
        public TimeSpan ProcessTraceTimeSpan { get; set; }

        /// <summary>
        /// 本次执行实际耗时
        /// </summary>
        public TimeSpan ProcessTimeSpan { get; set; }

        /// <summary>
        /// 计划任务名称
        /// </summary>
        public string PlanName { get; set; }

        private bool _isCenterMessage = false;
        /// <summary>
        /// 是否为中间消息
        /// </summary>
        public bool IsCenterMessage
        {
            get { return _isCenterMessage; }
            set { _isCenterMessage = value; }
        }
    }
}
