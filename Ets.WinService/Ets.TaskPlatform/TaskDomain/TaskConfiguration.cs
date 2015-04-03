using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.TaskDomain
{
    /// <summary>
    /// 计划任务配置
    /// </summary>
    [Serializable]
    public class TaskConfiguration
    {
        /// <summary>
        /// 计划任务系统名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 任务循环执行类型
        /// </summary>
        public LoopType LoopRunType { get; set; }
        private bool _canSendEmail = true;
        /// <summary>
        /// 是否可发送邮件。默认可以发送
        /// </summary>
        public bool CanSendEmail
        {
            get { return _canSendEmail; }
            set { _canSendEmail = value; }
        }
        private bool _canSendSMSMessage = false;
        /// <summary>
        /// 是否可发送短信。默认不可以发送。
        /// </summary>
        public bool CanSendSMSMessage
        {
            get { return _canSendSMSMessage; }
            set { _canSendSMSMessage = value; }
        }
    }

    /// <summary>
    /// 循环执行类型
    /// </summary>
    [Serializable]
    public enum LoopType
    {
        /// <summary>
        /// 不允许循环执行
        /// </summary>
        CannotLoop = -1,
        /// <summary>
        /// 默认。取决于任务返回值
        /// </summary>
        Default = 0,
        /// <summary>
        /// 必须循环执行
        /// </summary>
        MustLoop = 1
    }
}
