using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskPlatform.TaskDomain;

namespace TaskPlatform.Commom
{
    /// <summary>
    /// 计划任务平台配置
    /// </summary>
    [Serializable]
    public class PlatformConfiguration
    {
        private bool _enableLog = true;
        /// <summary>
        /// 开启日志功能
        /// </summary>
        public bool EnableLog
        {
            get { return _enableLog; }
            set { _enableLog = value; }
        }
        private bool _enableFileLog = true;
        /// <summary>
        /// 开启文件日志
        /// </summary>
        public bool EnableFileLog
        {
            get { return _enableFileLog; }
            set { _enableFileLog = value; }
        }
        private bool _enableDBLog = true;
        /// <summary>
        /// 开启数据库日志
        /// </summary>
        public bool EnableDBLog
        {
            get { return _enableDBLog; }
            set { _enableDBLog = value; }
        }
        private List<TaskConfiguration> _taskConfigList;
        /// <summary>
        /// 计划任务配置列表
        /// </summary>
        public List<TaskConfiguration> TaskConfigList
        {
            get
            {
                if (_taskConfigList == null)
                    _taskConfigList = new List<TaskConfiguration>();
                return _taskConfigList;
            }
            set { _taskConfigList = value; }
        }
    }
}
