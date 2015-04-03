using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.Commom
{
    /// <summary>
    /// 报警系统邮件设置
    /// </summary>
    [Serializable]
    public class TaskAlertToEmail
    {
        /// <summary>
        /// 计划任务的系统标识
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 报警系统发送到的邮件地址
        /// </summary>
        public string SendToEmailAddresses { get; set; }
    }

    /// <summary>
    /// 邮件类型
    /// </summary>
    public enum EmailType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,
        /// <summary>
        /// 报警系统邮件
        /// </summary>
        AlertSystem = 1
    }
}
