using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.TaskInterface
{
    /// <summary>
    /// 计划任务执行结果
    /// </summary>
    [Serializable]
    public class RunTaskResult
    {
        private bool _isCanceled = false;
        private bool _success = false;
        private bool _runAgain = false;
        private string _result = "";
        private long _runningBatch = 0;
        private Dictionary<string, object> _customerArgs = new Dictionary<string, object>();

        /// <summary>
        /// 指示计划任务执行是否成功。
        /// </summary>
        public bool Success
        {
            get
            {
                return _success;
            }
            set
            {
                _success = value;
            }
        }

        /// <summary>
        /// 指示是否立即再运行一次。
        /// </summary>
        public bool RunAgain
        {
            get { return _runAgain; }
            set { _runAgain = value; }
        }

        /// <summary>
        /// 计划任务执行的结果描述。
        /// </summary>
        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                if (string.IsNullOrWhiteSpace(_result))
                    _result = string.Empty;
            }
        }

        /// <summary>
        /// 指示是否已被平台取消
        /// </summary>
        public bool IsCanceled
        {
            get { return _isCanceled; }
            set { _isCanceled = value; }
        }

        /// <summary>
        /// 执行批次
        /// </summary>
        public long RunningBatch
        {
            get { return _runningBatch; }
            set { _runningBatch = value; }
        }

        /// <summary>
        /// 平台自定义参数项
        /// </summary>
        public Dictionary<string, object> CustomerArgs
        {
            get
            {
                return _customerArgs;
            }
            set
            {
                _customerArgs = value;
            }
        }
    }
}
