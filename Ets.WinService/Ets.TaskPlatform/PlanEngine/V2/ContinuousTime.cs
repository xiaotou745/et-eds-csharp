using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.PlanEngine.V2
{
    /// <summary>
    /// 表示持续时间
    /// </summary>
    public class ContinuousTime
    {
        internal event UpdatePropertyHandler UpdatePropertyDescription;
        protected void OnUpdatePropertyDescription()
        {
            if (UpdatePropertyDescription != null)
            {
                UpdatePropertyDescription();
            }
        }

        private DateTime _startTime = DateTime.Now;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnUpdatePropertyDescription(); }
        }
        private DateTime _endTime = DateTime.Now;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; OnUpdatePropertyDescription(); }
        }
        private bool _isForever = true;
        /// <summary>
        /// 是否无结束时间
        /// </summary>
        public bool IsForever
        {
            get { return _isForever; }
            set { _isForever = value; OnUpdatePropertyDescription(); }
        }

        /// <summary>
        /// 持续时间的描述
        /// </summary>
        public string Description
        {
            get
            {
                if (_isForever)
                {
                    return string.Format("将从 {0} 开始使用计划。", _startTime.DateString());
                }
                else
                {
                    return string.Format("将在 {0} 到 {1} 之间使用计划。", _startTime.DateString(), _endTime.DateString());
                }
            }
        }

        /// <summary>
        /// 获取该持续时间的深度副本
        /// </summary>
        /// <returns></returns>
        public ContinuousTime Clone()
        {
            ContinuousTime continuousTime = new ContinuousTime();
            continuousTime.EndTime = this.EndTime;
            continuousTime.IsForever = this.IsForever;
            continuousTime.StartTime = this.StartTime;
            return continuousTime;
        }
    }
}
