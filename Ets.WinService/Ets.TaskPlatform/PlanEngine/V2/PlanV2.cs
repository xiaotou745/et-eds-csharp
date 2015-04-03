using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.PlanEngine.V2
{
    /// <summary>
    /// V2版本执行计划
    /// </summary>
    public class PlanV2
    {
        /// <summary>
        /// 当计划需要被执行时触发。
        /// </summary>
        public event UpdatePropertyHandler PropertiesChanged;
        /// <summary>
        /// 当执行计划属性被更改时执行
        /// </summary>
        protected void OnPropertiesChanged()
        {
            if (PropertiesChanged != null)
            {
                PropertiesChanged();
            }
        }

        /// <summary>
        /// 实例化V2版本执行计划对象
        /// </summary>
        public PlanV2()
        {
            FrequencyOfPlan.UpdateProperty += UpdateDescription;
            FrequencyOfPlan.UpdatePropertyDescription += OnPropertiesChanged;
            FrequencyOfDay.UpdateProperty += UpdateDescription;
            FrequencyOfDay.UpdatePropertyDescription += OnPropertiesChanged;
            ContinuousTime.UpdatePropertyDescription += OnPropertiesChanged;
        }

        private string _planName = string.Empty;
        /// <summary>
        /// 获取执行计划名称
        /// </summary>
        public string PlanName
        {
            get { return _planName; }
            set { _planName = value; }
        }

        private bool _enable = true;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        private PlanType _type = PlanType.Repeat;
        /// <summary>
        /// 执行类型
        /// </summary>
        public PlanType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                UpdateDescription();
            }
        }

        private string _planDescription = string.Empty;
        /// <summary>
        /// 获取执行计划描述
        /// </summary>
        public string PlanDescription
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_planDescription))
                {
                    UpdateDescription();
                }
                _setDescription();
                return _planDescription;
            }
            private set
            {
                _planDescription = value;
            }
        }

        private DateTime _executeTime = DateTime.Now;
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecuteTime
        {
            get { return _executeTime; }
            set
            {
                _executeTime = value;
                OnPropertiesChanged();
            }
        }

        private DateTime _previousExecuteTime = DateTime.MinValue;
        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime PreviousExecuteTime
        {
            get
            {
                if (_previousExecuteTime == null)
                    _previousExecuteTime = DateTime.MinValue;
                return _previousExecuteTime;
            }
            set { _previousExecuteTime = value; }
        }

        private DateTime _nextExecuteTime = DateTime.Now;
        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextExecuteTime
        {
            get
            {
                DateTime nowTime = DateTime.Now;
                if (_nextExecuteTime == null)
                {
                    _nextExecuteTime = DateTime.Now;
                }
                if (Type == PlanType.Repeat && _nextExecuteTime < nowTime)
                {
                    FixExecuteTime(nowTime, 0, false, nowTime);
                }
                return _nextExecuteTime;
            }
            set { _nextExecuteTime = value; }
        }

        private PlanFrequency _frequencyOfPlan;
        /// <summary>
        /// 频率
        /// </summary>
        public PlanFrequency FrequencyOfPlan
        {
            get
            {
                if (_frequencyOfPlan == null)
                    _frequencyOfPlan = new PlanFrequency();
                return _frequencyOfPlan;
            }
            set { _frequencyOfPlan = value; }
        }

        private DayFrequency _frequencyOfDay = new DayFrequency();
        /// <summary>
        /// 每天频率
        /// </summary>
        public DayFrequency FrequencyOfDay
        {
            get
            {
                if (_frequencyOfDay == null)
                    _frequencyOfDay = new DayFrequency();
                return _frequencyOfDay;
            }
            set { _frequencyOfDay = value; }
        }

        private ContinuousTime _continuousTime;
        /// <summary>
        /// 持续时间
        /// </summary>
        public ContinuousTime ContinuousTime
        {
            get
            {
                if (_continuousTime == null)
                    _continuousTime = new ContinuousTime();
                return _continuousTime;
            }
            set { _continuousTime = value; }
        }

        /// <summary>
        /// 设置描述的函数
        /// </summary>
        private Action _setDescription;

        //private Func<DateTime> _getNextExecuteTime = null;

        /// <summary>
        /// 更新计划描述
        /// </summary>
        private void UpdateDescription()
        {
            if (_type == PlanType.Once)
            {
                _setDescription = () =>
                {
                    _planDescription = string.Format("在 {0} 的 {1} 执行。", ExecuteTime.DateString(), ExecuteTime.TimeString());
                };
            }
            else if (_type == PlanType.Repeat)
            {
                _setDescription = () =>
                {
                    _planDescription = FrequencyOfPlan.Description + FrequencyOfDay.Description + ContinuousTime.Description;
                };
            }
            _setDescription();
            OnPropertiesChanged();
        }

        /// <summary>
        /// 修复事件绑定
        /// </summary>
        private void FixEvents()
        {
            try
            {
                FrequencyOfPlan.UpdateProperty -= UpdateDescription;
            }
            catch { }
            FrequencyOfPlan.UpdateProperty += UpdateDescription;
            try
            {
                FrequencyOfPlan.UpdatePropertyDescription -= OnPropertiesChanged;
            }
            catch { }
            FrequencyOfPlan.UpdatePropertyDescription += OnPropertiesChanged;
            try
            {
                FrequencyOfDay.UpdateProperty -= UpdateDescription;
            }
            catch { }
            FrequencyOfDay.UpdateProperty += UpdateDescription;
            try
            {
                FrequencyOfDay.UpdatePropertyDescription -= OnPropertiesChanged;
            }
            catch { }
            FrequencyOfDay.UpdatePropertyDescription += OnPropertiesChanged;
            try
            {
                ContinuousTime.UpdatePropertyDescription -= OnPropertiesChanged;
            }
            catch { }
            ContinuousTime.UpdatePropertyDescription += OnPropertiesChanged;
        }

        /// <summary>
        /// 返回执行时间并移动到下一个执行时间
        /// </summary>
        /// <param name="baseTime">基线时间，如果计算出的下次执行时间不晚于该时间，则不会进行移动操作</param>
        /// <param name="forceSecond">强制后推forceSecond秒</param>
        /// <param name="currentTimeToCompute">是否以当前时间计算下次执行时间</param>
        /// <returns></returns>
        public DateTime MoveNextExecuteTime(DateTime baseTime, int forceSecond = -1, bool currentTimeToCompute = false)
        {
            DateTime nowTime = DateTime.Now;

            #region 计算下次执行时间

            PreviousExecuteTime = NextExecuteTime;
            if (forceSecond > 0)
            {
                NextExecuteTime = (currentTimeToCompute ? nowTime : NextExecuteTime).AddSeconds(forceSecond);
            }
            else
            {
                if (Type == PlanType.Once)
                {
                    NextExecuteTime = ExecuteTime;
                }
                else
                {
                    NextExecuteTime = ComputerTime(nowTime);
                }
            }

            #endregion

            #region 如果已记录的下次执行时间在当前时间之前，应该循环计算直到当前时间附近

            FixExecuteTime(baseTime, forceSecond, currentTimeToCompute, nowTime);

            #endregion

            return NextExecuteTime;
        }

        private void FixExecuteTime(DateTime baseTime, int forceSecond, bool currentTimeToCompute, DateTime nowTime)
        {
            if (Type != PlanType.Once && forceSecond < 1)
            {
                while (NextExecuteTime < nowTime)
                {
                    NextExecuteTime = MoveNextExecuteTime(baseTime, forceSecond, currentTimeToCompute);
                }
            }
        }

        /// <summary>
        /// 计算执行时间
        /// </summary>
        /// <returns>执行时间</returns>
        private DateTime ComputerTime(DateTime nowTime)
        {
            // 重复执行

            switch (FrequencyOfPlan.SpanType)
            {
                case PlanSpanType.Monthly:
                    // 按月执行
                    nowTime = ComputerMonthly(nowTime);
                    break;
                case PlanSpanType.Weekly:
                    // 按周执行
                    nowTime = ComputerWeekly(nowTime);
                    break;
                case PlanSpanType.Daily:
                    // 按天执行
                    nowTime = ComputerDaily(nowTime);
                    break;
            }

            return nowTime;
        }

        /// <summary>
        /// 按月执行
        /// </summary>
        /// <returns></returns>
        private DateTime ComputerMonthly(DateTime nowTime)
        {

            return nowTime;
        }

        /// <summary>
        /// 按周执行
        /// </summary>
        /// <returns></returns>
        private DateTime ComputerWeekly(DateTime nowTime)
        {
            return nowTime;
        }

        /// <summary>
        /// 按天执行
        /// </summary>
        /// <returns></returns>
        private DateTime ComputerDaily(DateTime nowTime)
        {
            if (PreviousExecuteTime == DateTime.MinValue)
            {
                nowTime = nowTime.AddDays(FrequencyOfPlan.SpanValue);
            }

            return nowTime;
        }

        private DateTime ComputerDayFrequency(DateTime nowTime)
        {
            if (nowTime < FrequencyOfDay.StartTime)
            {
                nowTime.Date.AddHours(FrequencyOfDay.StartTime.Hour).AddMinutes(FrequencyOfDay.StartTime.Minute).AddSeconds(FrequencyOfDay.StartTime.Second);
            }
            return nowTime;
        }

        /// <summary>
        /// 获取该执行计划的深度副本
        /// </summary>
        /// <returns></returns>
        public PlanV2 Clone()
        {
            PlanV2 clonePlanV2 = new PlanV2();
            clonePlanV2.ContinuousTime = this.ContinuousTime.Clone();
            clonePlanV2.Enable = this.Enable;
            clonePlanV2.ExecuteTime = this.ExecuteTime;
            clonePlanV2.FrequencyOfDay = this.FrequencyOfDay.Clone();
            clonePlanV2.FrequencyOfPlan = this.FrequencyOfPlan.Clone();
            clonePlanV2.PlanName = this.PlanName;
            clonePlanV2.Type = this.Type;
            clonePlanV2.FixEvents();
            clonePlanV2.UpdateDescription();
            return clonePlanV2;
        }
    }
}
