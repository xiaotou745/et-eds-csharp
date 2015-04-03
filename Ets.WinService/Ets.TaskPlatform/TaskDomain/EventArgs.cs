using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.TaskDomain
{
    /// <summary>
    /// 计划任务运行时域出现异常时所捕获到的数据。
    /// </summary>
    [Serializable]
    public class DomainExceptionEventArgs : EventArgs
    {
        private object _exceptionObject = null;
        private bool _isTerminating = false;
        private object _exceptionSender = null;
        /// <summary>
        /// 获取未经处理的异常对象。.
        /// </summary>
        public object ExceptionObject
        {
            get
            {
                return _exceptionObject;
            }
        }
        /// <summary>
        /// 指示公共语言运行时是否即将终止。
        /// </summary>
        /// <value>
        /// 如果运行时即将终止，则为 true；否则为 false。
        /// </value>
        public bool IsTerminating
        {
            get
            {
                return _isTerminating;
            }
        }

        /// <summary>
        /// 未经处理的异常对象的运行时域内发送者对象。
        /// </summary>
        public object ExceptionSender
        {
            get
            {
                return _exceptionSender;
            }
        }

        /// <summary>
        /// 初始化 <see cref="DomainExceptionEventArgs"/> 类.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="isTerminating">if set to <c>true</c> [is terminating].</param>
        public DomainExceptionEventArgs(object sender, object ex, bool isTerminating)
        {
            _exceptionSender = sender;
            _exceptionObject = ex;
            _isTerminating = isTerminating;
        }
    }
}
