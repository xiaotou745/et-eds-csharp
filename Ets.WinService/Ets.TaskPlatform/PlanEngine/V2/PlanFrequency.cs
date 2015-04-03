using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.PlanEngine.V2
{
    /// <summary>
    /// 表示计划频率
    /// </summary>
    public class PlanFrequency
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

        private PlanSpanType _spanType = PlanSpanType.Weekly;
        /// <summary>
        /// 执行频率
        /// </summary>
        public PlanSpanType SpanType
        {
            get { return _spanType; }
            set
            {
                _spanType = value;
                OnUpdateProperty();
            }
        }

        private int _spanValue = 1;
        /// <summary>
        /// 执行间隔值
        /// </summary>
        public int SpanValue
        {
            get { return _spanValue; }
            set
            {
                if (value < 1)
                {
                    _spanValue = 1;
                }
                else
                {
                    _spanValue = value;
                }
                OnUpdateProperty();
            }
        }

        private MonthExecuteType _monthTimeType = MonthExecuteType.Day;
        /// <summary>
        /// 每月执行类型
        /// </summary>
        public MonthExecuteType AtTimeType
        {
            get { return _monthTimeType; }
            set
            {
                _monthTimeType = value;
                OnUpdateProperty();
            }
        }

        private List<MonthOfYear> _monthOfYearList = new List<MonthOfYear>();
        /// <summary>
        /// 一年月份列表
        /// </summary>
        public List<MonthOfYear> MonthOfYearList
        {
            get { return _monthOfYearList; }
            set
            {
                _monthOfYearList = value;
                OnUpdateProperty();
            }
        }

        private List<DayOfWeekV2> _dayOfWeekList;
        /// <summary>
        /// 一周的天列表
        /// </summary>
        public List<DayOfWeekV2> DayOfWeekList
        {
            get
            {
                if (_dayOfWeekList == null)
                {
                    _dayOfWeekList = new List<DayOfWeekV2>();
                    _dayOfWeekList.Add(DayOfWeekV2.Sunday);
                }
                return _dayOfWeekList;
            }
            set
            {
                _dayOfWeekList = value;
                OnUpdateProperty();
            }
        }

        private List<DayOfWeekV2> _dayOfWeekListMonth;
        /// <summary>
        /// 一周的天列表(用于月)
        /// </summary>
        public List<DayOfWeekV2> DayOfWeekListMonth
        {
            get
            {
                if (_dayOfWeekListMonth == null)
                {
                    _dayOfWeekListMonth = new List<DayOfWeekV2>();
                }
                return _dayOfWeekListMonth;
            }
            set
            {
                _dayOfWeekListMonth = value;
                OnUpdateProperty();
            }
        }

        private List<DayOfMonth> _dayOfMonthList;
        /// <summary>
        /// 一月的天列表
        /// </summary>
        public List<DayOfMonth> DayOfMonthList
        {
            get
            {
                if (_dayOfMonthList == null)
                    _dayOfMonthList = new List<DayOfMonth>();
                return _dayOfMonthList;
            }
            set
            {
                _dayOfMonthList = value;
                OnUpdateProperty();
            }
        }

        private List<WeekNumber> _weekNumberList;
        /// <summary>
        /// 周次列表
        /// </summary>
        public List<WeekNumber> WeekNumberList
        {
            get
            {
                if (_weekNumberList == null)
                    _weekNumberList = new List<WeekNumber>();
                return _weekNumberList;
            }
            set
            {
                _weekNumberList = value;
                OnUpdateProperty();
            }
        }

        /// <summary>
        /// 计划频率的描述
        /// </summary>
        public string Description
        {
            get
            {
                StringBuilder sbDescription = new StringBuilder();
                if (SpanType == PlanSpanType.Monthly)
                {
                    #region 每月执行

                    if (MonthOfYearList.Count > 0)
                    {
                        var descList = (from f in MonthOfYearList
                                        select f.GetEnumDescription()).ToList();
                        sbDescription.Append("在 ");
                        sbDescription.Append(string.Join("、", descList));
                        sbDescription.Append(" ");
                    }
                    else
                    {
                        sbDescription.Append("每月");
                    }

                    if (AtTimeType == MonthExecuteType.Day)
                    {
                        sbDescription.Append("的 ");
                        if (DayOfMonthList.Count > 0)
                        {
                            var descList = (from f in DayOfMonthList
                                            where f != DayOfMonth.LastDay
                                            select f.GetEnumDescription()).ToList();
                            if (descList.Count > 0)
                            {
                                sbDescription.Append(string.Join("、", descList)).Append("日");
                            }
                            if (DayOfMonthList.Contains(DayOfMonth.LastDay))
                            {
                                if (descList.Count > 0)
                                {
                                    sbDescription.Append("或");
                                }
                                sbDescription.Append(DayOfMonth.LastDay.GetEnumDescription());
                            }
                        }
                        else
                        {
                            sbDescription.Append("每天");
                        }
                        sbDescription.Append(" ");
                    }
                    else if (AtTimeType == MonthExecuteType.Week)
                    {
                        sbDescription.Append("的 ");
                        if (WeekNumberList.Count > 0)
                        {
                            var descList = (from f in WeekNumberList
                                            select f.GetEnumDescription()).ToList();
                            sbDescription.Append(string.Join("、", descList));
                        }
                        else
                        {
                            sbDescription.Append("每一周");
                        }
                        sbDescription.Append(" ");
                        if (DayOfWeekListMonth.Count > 0)
                        {
                            var descList = (from f in DayOfWeekListMonth
                                            select f.GetEnumDescription()).ToList();
                            sbDescription.Append("的 ");
                            sbDescription.Append(string.Join("、", descList));
                            sbDescription.Append(" ");
                        }
                        else
                        {
                            sbDescription.Append("的 每天 ");
                        }
                    }

                    #endregion
                }
                else
                {
                    #region 如果不是按照每月执行

                    if (SpanValue > 1)
                    {
                        sbDescription.Append("每 ");
                        sbDescription.Append(SpanValue);
                        sbDescription.Append(" ");
                    }
                    else
                    {
                        sbDescription.Append("在每");
                    }
                    sbDescription.Append(SpanType.GetEnumDescription());
                    if (SpanType == PlanSpanType.Weekly)
                    {
                        if (DayOfWeekList.Count > 0)
                        {
                            var descList = (from f in DayOfWeekList
                                            select f.GetEnumDescription()).ToList();
                            if (SpanValue > 1)
                            {
                                sbDescription.Append("在 ");
                            }
                            else
                            {
                                sbDescription.Append("的 ");
                            }
                            sbDescription.Append(string.Join("、", descList));
                            sbDescription.Append(" ");
                        }
                        else
                        {
                            sbDescription.Append("每天 ");
                        }
                    }

                    #endregion
                }
                return sbDescription.ToString();
            }
        }

        /// <summary>
        /// 获取该计划频率的深度副本
        /// </summary>
        /// <returns></returns>
        public PlanFrequency Clone()
        {
            PlanFrequency planFrequency = new PlanFrequency();
            planFrequency.AtTimeType = this.AtTimeType;
            this.DayOfMonthList.ForEach(item =>
            {
                planFrequency.DayOfMonthList.Add(item);
            });
            this.DayOfWeekList.ForEach(item =>
            {
                planFrequency.DayOfWeekList.Add(item);
            });
            this.MonthOfYearList.ForEach(item =>
            {
                planFrequency.MonthOfYearList.Add(item);
            });
            planFrequency.SpanType = this.SpanType;
            planFrequency.SpanValue = this.SpanValue;
            this.WeekNumberList.ForEach(item =>
            {
                planFrequency.WeekNumberList.Add(item);
            });
            return planFrequency;
        }
    }
}
