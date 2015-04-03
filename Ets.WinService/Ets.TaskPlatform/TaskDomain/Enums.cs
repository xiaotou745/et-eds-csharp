using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.TaskDomain
{
    /// <summary>
    /// 删除计划任务信息类型枚举
    /// </summary>
    public enum DeleteTaskType
    {
        /// <summary>
        /// 不删除任何文件
        /// </summary>
        None = -1,
        /// <summary>
        /// 只删除日志信息
        /// </summary>
        Logs = 0,
        /// <summary>
        /// 只删除计划任务dll
        /// </summary>
        TaskFile = 2,
        /// <summary>
        /// 只删除计划任务的配置文件
        /// </summary>
        TaskConfigFile = 3,
        /// <summary>
        /// 删除计划任务dll文件和日志信息
        /// </summary>
        TaskFileAndLogs = 5,
        /// <summary>
        /// 删除计划任务dll和配置文件
        /// </summary>
        TaskFileAndConfigFile = 7,
        /// <summary>
        /// 删除日志信息和计划任务配置文件
        /// </summary>
        LogsAndConfigFile = 11,
        /// <summary>
        /// 删除计划任务dll、配置文件以及日志信息
        /// </summary>
        All = 13
    }
}
