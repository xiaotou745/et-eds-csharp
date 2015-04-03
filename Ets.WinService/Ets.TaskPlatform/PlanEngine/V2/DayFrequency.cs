using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.PlanEngine.V2
{
    /// <summary>
    /// 表示每天频率
    /// </summary>
    public class DayFrequency
    {
        /// <summary>
        /// 当计划需要被执行时触发。
        /// </summary>
        internal event UpdatePropertyHandler UpdateProperty;
        /// <summary>
        /// 当执行计划属性被更改时执行
        /// </summary>
        protected void OnUpdateProperty()
        {
            if (UpdateProperty != null)
            {
                UpdateProperty();
            }
        }

        internal event UpdatePropertyHandler UpdatePropertyDescription;
        protected void OnUpdatePropertyDescription()
        {
            if (UpdatePropertyDescription != null)
            {
                UpdatePropertyDescription();
            }
        }

        private PlanType _planType = PlanType.Once;
        /// <summary>
        /// 每天频率类型
        /// </summary>
        public PlanType DayPlanType
        {
            get { return _planType; }
            set
            {
                _planType = value;
                OnUpdateProperty();
            }
        }

        private DateTime _time = DateTime.Now.Date;
        /// <summary>
        /// 执行一次的时间
        /// </summary>
        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnUpdateProperty();
            }
        }

        private int _interval = 1;
        /// <summary>
        /// 执行间隔数值
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                OnUpdateProperty();
            }
        }

        private DayFrequencyUnit _unit = DayFrequencyUnit.Hour;
        /// <summary>
        /// 时间间隔类型
        /// </summary>
        public DayFrequencyUnit Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                OnUpdateProperty();
            }
        }

        private DateTime _startTime = DateTime.Now.Date;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnUpdateProperty();
            }
        }

        private DateTime _endTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnUpdateProperty();
            }
        }

        /// <summary>
        /// 每天频率的描述
        /// </summary>
        public string Description
        {
            get
            {
                string result = string.Empty;
                switch (DayPlanType)
                {
                    case PlanType.Once:
                        result = string.Format("的 {0} 执行。", Time.TimeString());
                        break;
                    case PlanType.Repeat:
                        result = string.Format("在 {0} 和 {1} 之间、每 {2} {3} 执行。", StartTime.TimeString(), EndTime.TimeString(), Interval, Unit.GetEnumDescription());
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// 获取该每天频率的深度副本
        /// </summary>
        /// <returns></returns>
        public DayFrequency Clone()
        {
            DayFrequency dayFrequency = new DayFrequency();
            dayFrequency.DayPlanType = this.DayPlanType;
            dayFrequency.EndTime = this.EndTime;
            dayFrequency.Interval = this.Interval;
            dayFrequency.StartTime = this.StartTime;
            dayFrequency.Time = this.Time;
            dayFrequency.Unit = this.Unit;
            return dayFrequency;
        }
    }
}
