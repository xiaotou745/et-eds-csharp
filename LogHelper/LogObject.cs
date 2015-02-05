using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{
    /// <summary>
    /// 日志实体
    /// </summary>
    public class LogObject
    {

        #region 日志属性
        /// <summary>
        /// 日志标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 日志名称
        /// </summary>
        public string LogName { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType LogType { get; set; }
        /// <summary>
        /// 日志事件发生时间
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 计划任务名称
        /// </summary>
        public string TaskName { get; set; }
        private DateTime? _trigTime = null;
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime? TrigTime
        {
            get { return _trigTime; }
            set { _trigTime = value; }
        }
        private DateTime? _startExecuteTime = null;
        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime? StartExecuteTime
        {
            get { return _startExecuteTime; }
            set { _startExecuteTime = value; }
        }
        private DateTime? _endExecuteTime = null;
        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime? EndExecuteTime
        {
            get { return _endExecuteTime; }
            set { _endExecuteTime = value; }
        }
        private string _executeTime = null;
        /// <summary>
        /// 执行耗时
        /// </summary>
        public string ExecuteTime
        {
            get { return _executeTime; }
            set { _executeTime = value; }
        }
        private string _threadQueueTime = null;
        /// <summary>
        /// 线程排队耗时
        /// </summary>
        public string ThreadQueueTime
        {
            get { return _threadQueueTime; }
            set { _threadQueueTime = value; }
        }
        private bool _isCancle = false;
        /// <summary>
        /// 是否已取消
        /// </summary>
        public bool IsCancle
        {
            get { return _isCancle; }
            set { _isCancle = value; }
        }
        #endregion

        private bool _isSuccess = false;
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get { return _isSuccess; }
            set { _isSuccess = value; }
        }
        private string _runTaskResult = string.Empty;
        /// <summary>
        /// 执行结果
        /// </summary>
        public string RunTaskResult
        {
            get { return _runTaskResult; }
            set { _runTaskResult = value; }
        }
    }
}
