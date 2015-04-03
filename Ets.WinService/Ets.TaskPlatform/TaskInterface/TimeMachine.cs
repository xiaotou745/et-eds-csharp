#region 类信息
/* ==============================================================================
 * 功能描述：TimeMachine  
 * 创建机器：PC00168
 * 创 建 者：Administrator 
 * 创建日期：2013-10-02 17:37:40  5 
 * ==============================================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.TaskInterface
{
    /// <summary>
    /// 实例化时间管理
    /// </summary>
    public class TimeMachine
    {

        #region 成员属性

        /// <summary>
        /// 基线时间，实际开始时间不会晚于该时间
        /// </summary>
        private DateTime _baseTime = DateTime.Now;

        /// <summary>
        /// 基线时间，实际开始时间不会晚于该时间
        /// </summary>
        public DateTime BaseTime
        {
            get { return _baseTime; }
            set
            {
                _baseTime = TrimTime(value);
            }
        }
        /// <summary>
        /// 开始时间(期望)
        /// </summary>
        private DateTime _startTime = DateTime.Now;

        /// <summary>
        /// 开始时间(期望)
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return TrimTime(_startTime);
            }
            set
            {
                _startTime = TrimTime(value);
            }
        }
        /// <summary>
        /// 时间开始时间
        /// </summary>
        private DateTime _factStartTime = DateTime.Now;

        /// <summary>
        /// 时间开始时间
        /// </summary>
        public DateTime FactStartTime
        {
            get { return TrimTime(_factStartTime); }
            private set { _factStartTime = TrimTime(value); }
        }
        /// <summary>
        /// 时间向左偏移的秒数
        /// </summary>
        private int _offsetLeft = 1;

        /// <summary>
        /// 时间向左偏移的秒数
        /// </summary>
        public int OffsetLeft
        {
            get { return _offsetLeft; }
            set { _offsetLeft = value; }
        }
        /// <summary>
        /// 时间向左偏移的秒数
        /// </summary>
        private int _offsetRight = 10;

        /// <summary>
        /// 时间向右偏移的秒数
        /// </summary>
        public int OffsetRight
        {
            get { return _offsetRight; }
            set { _offsetRight = value; }
        }
        /// <summary>
        /// 时间段跨度(秒)
        /// </summary>
        private int _interval = 10;

        /// <summary>
        /// 时间段跨度(秒)
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }
        /// <summary>
        /// 时间段最大跨度(秒)
        /// </summary>
        private int _maxTimeSpan = 86400;

        /// <summary>
        /// 时间段最大跨度(秒)
        /// </summary>
        public int MaxTimeSpan
        {
            get { return _maxTimeSpan; }
            set { _maxTimeSpan = value; }
        }
        /// <summary>
        /// 最大时间跨度使用次数(连续使用超过该次数后，时间段会向右累积偏移)
        /// </summary>
        private int _overMaxTimeCount = 5;

        /// <summary>
        /// 最大时间跨度使用次数(连续使用超过该次数后，时间段会向右累积偏移)
        /// </summary>
        public int OverMaxTimeCount
        {
            get { return _overMaxTimeCount; }
            set { _overMaxTimeCount = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime _endTime = DateTime.Now;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return TrimTime(_endTime); }
            private set { _endTime = TrimTime(value); }
        }
        /// <summary>
        /// 是否开启智能计算
        /// </summary>
        private bool _enableSmartCompute = true;

        /// <summary>
        /// 是否开启智能计算
        /// </summary>
        public bool EnableSmartCompute
        {
            get { return _enableSmartCompute; }
            set
            {
                if (_enableSmartCompute != value)
                {
                    ResetSmartCompute();
                }
                _enableSmartCompute = value;
            }
        }
        /// <summary>
        /// 智能计算后开始时间是否可晚于基线时间
        /// </summary>
        private bool _smartComputeCanOverBaseTime = true;

        /// <summary>
        /// 智能计算后开始时间是否可晚于基线时间
        /// </summary>
        public bool SmartComputeCanOverBaseTime
        {
            get { return _smartComputeCanOverBaseTime; }
            set
            {
                if (_smartComputeCanOverBaseTime != value)
                {
                    ResetSmartCompute();
                }
                _smartComputeCanOverBaseTime = value;
            }
        }

        #region 内部用成员属性

        /// <summary>
        /// 前一个时间段Key
        /// </summary>
        private string preTimeKey = string.Empty;
        /// <summary>
        /// 同一时间段用过的次数
        /// </summary>
        private int _timeUseCount = 0;
        /// <summary>
        /// 同一时间段用过的次数
        /// </summary>
        public int TimeUseCount
        {
            get { return _timeUseCount; }
            private set { _timeUseCount = value; }
        }
        /// <summary>
        /// 向右偏移的次数
        /// </summary>
        private int _offsetRightCount = 0;
        /// <summary>
        /// 向右偏移的次数
        /// </summary>
        public int OffsetRightCount
        {
            get { return _offsetRightCount; }
            private set { _offsetRightCount = value; }
        }

        #endregion

        #endregion

        #region 方法

        /// <summary>
        /// 实例化时间管理
        /// </summary>
        public TimeMachine()
        { }

        /// <summary>
        /// 将时间的毫秒修剪掉
        /// </summary>
        /// <param name="time">要修剪的时间</param>
        /// <returns></returns>
        private DateTime TrimTime(DateTime time)
        {
            if (time.Millisecond > 0)
            {
                time = time.AddMilliseconds(0 - time.Millisecond);
            }
            return time;
        }

        /// <summary>
        /// 以当前时间为结束时间结束计算
        /// </summary>
        private void SetTimeBaseNow(DateTime timeNow)
        {
            EndTime = timeNow;
            FactStartTime = EndTime.AddSeconds(0 - Interval - OffsetLeft);
        }

        /// <summary>
        /// 重置智能计算标记
        /// </summary>
        private void ResetSmartCompute()
        {
            TimeUseCount = 0;
            OffsetRightCount = 0;
        }

        /// <summary>
        /// 计算时间段
        /// </summary>
        /// <returns></returns>
        public void ComputeTime()
        {
            DateTime timeNow = DateTime.Now;

            // 不允许晚于基线时间
            if (StartTime > BaseTime)
            {
                StartTime = BaseTime;
            }

            // 如果开始时间等于(或晚于)当前时间，则以当前时间为结束时间结束计算
            if (StartTime >= timeNow)
            {
                SetTimeBaseNow(timeNow);
            }
            else
            {
                // 设置开始时间
                int leftOffset = 0 - OffsetLeft;
                FactStartTime = StartTime.AddSeconds(leftOffset);
                // 设置结束时间
                EndTime = StartTime.AddSeconds(Interval);
                // 如果结束时间晚于当前时间，则以当前时间为结束时间结束计算
                // 如果刚好等于，那也没事哈
                if (EndTime > timeNow)
                {
                    SetTimeBaseNow(timeNow);
                }
                else
                {
                    ComputeTimeOffsetRight();
                    // 向右偏移后不得晚于当前时间
                    if (EndTime > timeNow)
                    {
                        SetTimeBaseNow(timeNow);
                    }
                }
            }
        }

        /// <summary>
        /// 计算是否需要向右偏移
        /// </summary>
        private void ComputeTimeOffsetRight()
        {
            #region 计算是否需要向右偏移

            string nowTimeKey = FactStartTime.ToString("MM-dd HH:mm:ss") + "|" + EndTime.ToString("MM-dd HH:mm:ss");
            if (preTimeKey != nowTimeKey)
            {
                preTimeKey = nowTimeKey;
                // 设置该时间用过的次数
                _timeUseCount = 1;
                // 清掉向右偏移的量
                _offsetRightCount = 0;
            }
            else
            {
                // 累计该时间用过的次数
                _timeUseCount++;
                // 如果本次超过了最大使用次数，则累计向右偏移
                if (_timeUseCount > OverMaxTimeCount)
                {
                    // 累计向右偏移
                    _offsetRightCount++;
                    // 清除该时间段的本次偏移计算计数器
                    _timeUseCount = 0;
                }
                if (_offsetRightCount > 0)
                {
                    // 将时间段向右偏移
                    int rightSeconds = OffsetRight * _offsetRightCount;
                    EndTime = EndTime.AddSeconds(rightSeconds);
                    // 时间跨度超限后开始时间跟随结束时间计算
                    if ((EndTime - FactStartTime).TotalSeconds > MaxTimeSpan)
                    {
                        int timeSpan = 0 - MaxTimeSpan;
                        FactStartTime = EndTime.AddSeconds(timeSpan);
                        if (!SmartComputeCanOverBaseTime)
                        {
                            FactStartTime = BaseTime;
                            EndTime = FactStartTime.AddSeconds(MaxTimeSpan);
                        }
                    }
                }
            }

            #endregion
        }

        #endregion

    }
}
