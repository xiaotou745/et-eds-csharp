using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.PlatformLog
{
    /// <summary>
    /// 日志类型
    /// </summary>
    [Serializable]
    public enum LogType
    {
        /// <summary>
        /// 系统日志
        /// </summary>
        System,
        /// <summary>
        /// 计划任务执行日志
        /// </summary>
        Tasks,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom
    }
}
