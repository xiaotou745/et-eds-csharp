using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.Commom
{
    public class TaskRunInfo
    {
        /// <summary>
        /// 计划名称
        /// </summary>
        public string PlanName { get; set; }
        private int _runResult = 2;
        /// <summary>
        /// 执行结果(-1取消执行0执行失败1执行成功)
        /// </summary>
        public int RunResult
        {
            get { return _runResult; }
            set { _runResult = value; }
        }
        private DateTime _runTime = DateTime.Now;
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime RunTime
        {
            get { return _runTime; }
            set { _runTime = value; }
        }
    }
}
