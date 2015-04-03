using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskPlatform.Commom
{
    /// <summary>
    /// 警报主体
    /// </summary>
    [Serializable]
    public class AlarmObject
    {
        /// <summary>
        /// 计划任务系统名称
        /// </summary>
        public string PlanName { get; set; }
        private int _alarmType = 1;
        /// <summary>
        /// 警报类型(1：全部开启2：只开启执行失败监测3：只开启取消执行监测4：关闭报警5:只开启必须运行报警)
        /// </summary>
        public int AlarmType
        {
            get { return _alarmType; }
            set { _alarmType = value; }
        }
        /// <summary>
        /// 计划任务友好名称
        /// </summary>
        public string TaskName { get; set; }
        private int _runCount = 20;
        /// <summary>
        /// 取样执行次数(默认20次)
        /// </summary>
        public int RunCount
        {
            get { return _runCount; }
            set { _runCount = value; }
        }
        private int _alarmWhenCancelCount = 3;
        /// <summary>
        /// 当在指定时间范围内被取消执行多少次后发出报警(默认3次)
        /// </summary>
        public int AlarmWhenCancelCount
        {
            get { return _alarmWhenCancelCount; }
            set { _alarmWhenCancelCount = value; }
        }
        private int _alarmWhenFailCount = 2;
        /// <summary>
        /// 当任务执行连续失败多少次后发出报警(默认2次)
        /// </summary>
        public int AlarmWhenFailCount
        {
            get { return _alarmWhenFailCount; }
            set { _alarmWhenFailCount = value; }
        }
        private decimal _timeOutRatio = 1;
        /// <summary>
        /// 超时倍率(默认1，且不能小于或等于0)
        /// </summary>
        public decimal TimeOutRatio
        {
            get
            {
                if (_timeOutRatio <= 0)
                    _timeOutRatio = 1;
                return _timeOutRatio;
            }
            set { _timeOutRatio = value; }
        }
        private string _alarmEmailAddress = string.Empty;
        /// <summary>
        /// 本计划任务警报邮件接收人
        /// </summary>
        public string AlarmEmailAddress
        {
            get { return _alarmEmailAddress; }
            set { _alarmEmailAddress = value; }
        }
        /// <summary>
        /// 备用字段1
        /// </summary>
        public string Field1 { get; set; }
        /// <summary>
        /// 备用字段2
        /// </summary>
        public string Field2 { get; set; }
        /// <summary>
        /// 备用字段3
        /// </summary>
        public string Field3 { get; set; }
        /// <summary>
        /// 备用字段4
        /// </summary>
        public string Field4 { get; set; }
        /// <summary>
        /// 备用时间1
        /// </summary>
        public DateTime DateTime1 { get; set; }
        /// <summary>
        /// 备用时间2
        /// </summary>
        public DateTime DateTime2 { get; set; }
        /// <summary>
        /// 备用时间3
        /// </summary>
        public DateTime DateTime3 { get; set; }
        /// <summary>
        /// 备用时间4
        /// </summary>
        public DateTime DateTime4 { get; set; }

        /// <summary>
        /// 获取报警类型
        /// </summary>
        internal static string GetAlarmTypeString(AlarmObject alarmObject)
        {
            string result = "未知";
            switch (alarmObject.AlarmType)
            { 
                case 1:
                    result = "全部开启";
                    break;
                case 2:
                    result = "只开启执行失败监测";
                    break;
                case 3:
                    result = "只开启取消执行监测";
                    break;
                case 4:
                    result = "关闭报警";
                    break;
                case 5:
                    result = "只开启必须运行报警";
                    break;
            }
            return result;
        }
    }

    public class AlarmObjectComparer : IEqualityComparer<AlarmObject>
    {
        public bool Equals(AlarmObject x, AlarmObject y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null)
            {
                return false;
            }
            else if (y == null)
            {
                return false;
            }
            else if (string.Equals(x.PlanName, y.PlanName, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(AlarmObject alarmObject)
        {
            if (Object.ReferenceEquals(alarmObject, null))
                return 0;
            else if (string.IsNullOrWhiteSpace(alarmObject.PlanName))
            {
                return 0;
            }
            else
            {
                return alarmObject.PlanName.GetHashCode();
            }
        }
    }
}
