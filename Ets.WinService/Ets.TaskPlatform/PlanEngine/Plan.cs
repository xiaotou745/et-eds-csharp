using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskPlatform.PlatformLog;

namespace TaskPlatform.PlanEngine
{
    /// <summary>
    /// 计划。为计划任务提供计划调度方案。
    /// </summary>
    [Serializable]
    public class Plan
    {
        #region 成员属性
        private string _planName = string.Format("计划：({0})", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        /// <summary>
        /// 计划名称
        /// </summary>
        public string PlanName
        {
            get
            {
                return _planName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("计划的名称不能为null为空字符串。");
                }
                _planName = value;
            }
        }
        private PlanType _type = PlanType.Repeat;
        /// <summary>
        /// 计划类型。
        /// </summary>
        public PlanType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        private int _interval = 15;
        /// <summary>
        /// 计划的时间间隔
        /// </summary>
        public int Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                if (value <= 0 || value > int.MaxValue)
                {
                    throw new ArgumentException("时间间隔不能小于等于0或大于Int32.MaxValue。");
                }
                _interval = value;
                ComputeTime();
            }
        }
        /// <summary>
        /// 计划的时间间隔的单位
        /// </summary>
        public PlanTimeUnit PlanUnit { get; set; }

        private DateTime _planStartTime = DateTime.Now;
        /// <summary>
        /// 计划的第一次开始执行时间(包含)。从这一时刻起，计划开始按照设置执行。
        /// </summary>
        public DateTime PlanStartTime
        {
            get
            {
                return _planStartTime;
            }
            set
            {
                _planStartTime = value;
            }
        }

        private DateTime _planEndTime = DateTime.Now;
        /// <summary>
        /// 计划执行到的时间(包含)。这下一时刻起，计划将自动被停用掉。
        /// </summary>
        public DateTime PlanEndTime
        {
            get
            {
                return _planEndTime;
            }
            set
            {
                _planEndTime = value;
            }
        }

        private DateTime _lastRunTime = DateTime.Now;
        /// <summary>
        /// 计划的最后一次执行时间。设置该属性时，如果计划类型是PlanType.Once，则计划将被自动停用。
        /// 可以通过设置重新启用计划。
        /// </summary>
        public DateTime LastRunTime
        {
            get
            {
                return _lastRunTime;
            }
            set
            {
                _lastRunTime = value;
            }
        }
        private DateTime _nextRunTime = DateTime.Now.AddSeconds(15);
        /// <summary>
        /// 计划的下次执行时间
        /// </summary>
        public DateTime NextRunTime
        {
            get
            {
                return _nextRunTime;
            }
            set
            {
                _nextRunTime = value;
            }
        }

        /// <summary>
        /// 计划是否启用。停用的计划将会自动被引擎从列表中清掉。
        /// </summary>
        public bool Enable { get; set; }
        private TimeSpan _lastProcessTimeSpan = new TimeSpan(0, 0, 0);
        /// <summary>
        /// 计划上次执行的耗时
        /// </summary>
        public TimeSpan LastProcessTimeSpan
        {
            get
            {
                return _lastProcessTimeSpan;
            }
        }
        /// <summary>
        /// 是否没有结束时间
        /// </summary>
        public bool NoOverTime { get; set; }
        #endregion

        /// <summary>
        /// 触发计划执行的委托。
        /// </summary>
        /// <param name="sender">要执行的计划</param>
        /// <param name="e">执行的计划所包含的信息</param>
        public delegate void PlanTrigHandler(object sender, PlanTrigEventArgs e);
        /// <summary>
        /// 当计划需要被执行时触发。
        /// </summary>
        internal event PlanTrigHandler PlanTrig;
        /// <summary>
        /// 触发计划的执行。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPlanTrig(PlanTrigEventArgs e)
        {
            if (PlanTrig != null)
                PlanTrig(this, e);
        }

        ///// <summary>
        ///// 计划启用期开始时间
        ///// </summary>
        //private DateTime _startEnableTime = DateTime.Now.Date;
        ///// <summary>
        ///// 计划启用期开始时间
        ///// </summary>
        //public DateTime StartEnableTime
        //{
        //    get { return _startEnableTime; }
        //    set { _startEnableTime = value; }
        //}
        ///// <summary>
        ///// 计划启用期结束时间
        ///// </summary>
        //private DateTime _endEnableTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
        ///// <summary>
        ///// 计划启用期结束时间
        ///// </summary>
        //public DateTime EndEnableTime
        //{
        //    get { return _endEnableTime; }
        //    set { _endEnableTime = value; }
        //}

        #region 成员方法
        /// <summary>
        /// 重新计算下次的执行时间
        /// </summary>
        /// <param name="currentTimeToCompute">指示是否取当前时间作为计算下次执行时间的基准。</param>
        public void ComputeTime(bool currentTimeToCompute = false, bool runagain = false)
        {
            DateTime time = currentTimeToCompute ? DateTime.Now : _lastRunTime;
            if (runagain)
            {
                NextRunTime = DateTime.Now.AddSeconds(1);
            }
            else
            {
                if (_type == PlanType.Once)
                {
                    NextRunTime = _lastRunTime;
                    Enable = false;
                }
                else
                {
                    switch (PlanUnit)
                    {
                        case PlanTimeUnit.Second:
                            _nextRunTime = time.AddSeconds(_interval);
                            break;
                        case PlanTimeUnit.Minute:
                            _nextRunTime = time.AddMinutes(_interval);
                            break;
                        case PlanTimeUnit.Hour:
                            _nextRunTime = time.AddHours(_interval);
                            break;
                        case PlanTimeUnit.Day:
                            _nextRunTime = time.AddDays(_interval);
                            break;
                        case PlanTimeUnit.Month:
                            _nextRunTime = time.AddMonths(_interval);
                            break;
                        case PlanTimeUnit.Year:
                            _nextRunTime = time.AddYears(_interval);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 触发计划执行
        /// </summary>
        private void TrigPlan()
        {
            _lastRunTime = DateTime.Now;
            try
            {
                PlanTrigEventArgs e = new PlanTrigEventArgs();
                e.TrigPlanName = _planName;
                e.RunTime = _lastRunTime;
                e.LastProcessTimeSpan = _lastProcessTimeSpan;
                OnPlanTrig(e);
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "PlanEngine--PlanException");
                throw ex;
            }
            finally
            {
                try
                {
                    _lastProcessTimeSpan = DateTime.Now - _lastRunTime;
                }
                catch (Exception ex)
                {
                    Log.CustomWrite(ex.ToString(), "PlanEngine--PlanException");
                }
                ComputeTime(true);
            }
        }

        /// <summary>
        /// 标记开始执行计划。
        /// </summary>
        internal void StartPlan()
        {
            _lastRunTime = DateTime.Now;
        }

        /// <summary>
        /// 标记计划执行结束。
        /// </summary>
        internal void EndPlan()
        {
            try
            {
                _lastProcessTimeSpan = DateTime.Now - _lastRunTime;
            }
            catch (Exception ex)
            {
                Log.CustomWrite(ex.ToString(), "PlanEngine--PlanException");
            }
            ComputeTime(true);
        }

        public Plan Copy()
        {
            return this.MemberwiseClone() as Plan;
        }

        /// <summary>
        /// 获取任务可超时秒数
        /// </summary>
        public int TotalSeconds
        {
            get
            {
                int seconds = 0;
                if (_type == PlanType.Once)
                {
                    seconds = 10;
                }
                else
                {
                    switch (PlanUnit)
                    {
                        case PlanTimeUnit.Second:
                            seconds = this.Interval;
                            break;
                        case PlanTimeUnit.Minute:
                        case PlanTimeUnit.Hour:
                        case PlanTimeUnit.Day:
                        case PlanTimeUnit.Month:
                        case PlanTimeUnit.Year:
                            seconds = this.Interval * 60;
                            break;
                    }
                }
                return seconds;
            }
        }

        #endregion
    }
}
