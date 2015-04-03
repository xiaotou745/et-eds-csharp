using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace TaskPlatform.PlanEngine
{
    /// <summary>
    /// 计划类型
    /// </summary>
    [Serializable]
    public enum PlanType
    {
        /// <summary>
        /// 执行一次
        /// </summary>
        [Description("执行一次")]
        Once,
        /// <summary>
        /// 重复执行
        /// </summary>
        [Description("重复执行")]
        Repeat
    }

    /// <summary>
    /// 计划时间间隔的单位
    /// </summary>
    [Serializable]
    public enum PlanTimeUnit
    {
        /// <summary>
        /// 秒
        /// </summary>
        [Description("秒")]
        Second = 0,
        /// <summary>
        /// 分
        /// </summary>
        [Description("分钟")]
        Minute,
        /// <summary>
        /// 小时
        /// </summary>
        [Description("小时")]
        Hour,
        /// <summary>
        /// 天
        /// </summary>
        [Description("天")]
        Day,
        /// <summary>
        /// 月
        /// </summary>
        [Description("月")]
        Month,
        /// <summary>
        /// 年
        /// </summary>
        [Description("年")]
        Year
    }

    /// <summary>
    /// 每天频率的时间间隔的单位
    /// </summary>
    [Serializable]
    public enum DayFrequencyUnit
    {
        /// <summary>
        /// 秒
        /// </summary>
        [Description("秒")]
        Second = 0,
        /// <summary>
        /// 分
        /// </summary>
        [Description("分钟")]
        Minute,
        /// <summary>
        /// 小时
        /// </summary>
        [Description("小时")]
        Hour,
    }

    /// <summary>
    /// 计划时间间隔类型
    /// </summary>
    [Serializable]
    public enum PlanSpanType
    {
        /// <summary>
        /// 每天
        /// </summary>
        [Description("天")]
        Daily = 1,
        /// <summary>
        /// 每周
        /// </summary>
        [Description("周")]
        Weekly,
        /// <summary>
        /// 每月
        /// </summary>
        [Description("月")]
        Monthly
    }

    /// <summary>
    /// 指定一周的某天
    /// </summary>
    [Serializable]
    public enum DayOfWeekV2
    {
        /// <summary>
        /// 周一
        /// </summary>
        [Description("周一")]
        Monday = 1,
        /// <summary>
        /// 周二
        /// </summary>
        [Description("周二")]
        Tuesday = 2,
        /// <summary>
        /// 周三
        /// </summary>
        [Description("周三")]
        Wednesday = 3,
        /// <summary>
        /// 周四
        /// </summary>
        [Description("周四")]
        Thursday = 4,
        /// <summary>
        /// 周五
        /// </summary>
        [Description("周五")]
        Friday = 5,
        /// <summary>
        /// 周六
        /// </summary>
        [Description("周六")]
        Saturday = 6,
        /// <summary>
        /// 周日
        /// </summary>
        [Description("周日")]
        Sunday = 7,
    }

    /// <summary>
    /// 指定一月的某天
    /// </summary>
    [Serializable]
    public enum DayOfMonth
    {
        /// <summary>
        /// 1日
        /// </summary>
        [Description("1")]
        Day1 = 1,
        /// <summary>
        /// 2日
        /// </summary>
        [Description("2")]
        Day2,
        /// <summary>
        /// 3日
        /// </summary>
        [Description("3")]
        Day3,
        /// <summary>
        /// 4日
        /// </summary>
        [Description("4")]
        Day4,
        /// <summary>
        /// 5日
        /// </summary>
        [Description("5")]
        Day5,
        /// <summary>
        /// 6日
        /// </summary>
        [Description("6")]
        Day6,
        /// <summary>
        /// 7日
        /// </summary>
        [Description("7")]
        Day7,
        /// <summary>
        /// 8日
        /// </summary>
        [Description("8")]
        Day8,
        /// <summary>
        /// 9日
        /// </summary>
        [Description("9")]
        Day9,
        /// <summary>
        /// 10日
        /// </summary>
        [Description("10")]
        Day10,
        /// <summary>
        /// 11日
        /// </summary>
        [Description("11")]
        Day11,
        /// <summary>
        /// 12日
        /// </summary>
        [Description("12")]
        Day12,
        /// <summary>
        /// 13日
        /// </summary>
        [Description("13")]
        Day13,
        /// <summary>
        /// 14日
        /// </summary>
        [Description("14")]
        Day14,
        /// <summary>
        /// 15日
        /// </summary>
        [Description("15")]
        Day15,
        /// <summary>
        /// 16日
        /// </summary>
        [Description("16")]
        Day16,
        /// <summary>
        /// 17日
        /// </summary>
        [Description("17")]
        Day17,
        /// <summary>
        /// 18日
        /// </summary>
        [Description("18")]
        Day18,
        /// <summary>
        /// 19日
        /// </summary>
        [Description("19")]
        Day19,
        /// <summary>
        /// 20日
        /// </summary>
        [Description("20")]
        Day20,
        /// <summary>
        /// 21日
        /// </summary>
        [Description("21")]
        Day21,
        /// <summary>
        /// 22日
        /// </summary>
        [Description("22")]
        Day22,
        /// <summary>
        /// 23日
        /// </summary>
        [Description("23")]
        Day23,
        /// <summary>
        /// 24日
        /// </summary>
        [Description("24")]
        Day24,
        /// <summary>
        /// 25日
        /// </summary>
        [Description("25")]
        Day25,
        /// <summary>
        /// 26日
        /// </summary>
        [Description("26")]
        Day26,
        /// <summary>
        /// 27日
        /// </summary>
        [Description("27")]
        Day27,
        /// <summary>
        /// 28日
        /// </summary>
        [Description("28")]
        Day28,
        /// <summary>
        /// 29日
        /// </summary>
        [Description("29")]
        Day29,
        /// <summary>
        /// 30日
        /// </summary>
        [Description("30")]
        Day30,
        /// <summary>
        /// 31日
        /// </summary>
        [Description("31")]
        Day31,
        /// <summary>
        /// 最后一天
        /// </summary>
        [Description("最后一天")]
        LastDay
    }

    /// <summary>
    /// 指定一月的第几周
    /// </summary>
    [Serializable]
    public enum WeekNumber
    {
        /// <summary>
        /// 第一周
        /// </summary>
        [Description("第一周")]
        First = 1,
        /// <summary>
        /// 第二周
        /// </summary>
        [Description("第二周")]
        Second,
        /// <summary>
        /// 第三周
        /// </summary>
        [Description("第三周")]
        Third,
        /// <summary>
        /// 第四周
        /// </summary>
        [Description("第四周")]
        Fourth,
        /// <summary>
        /// 最后一周
        /// </summary>
        [Description("最后一周")]
        Last
    }

    /// <summary>
    /// 指定一年的某月
    /// </summary>
    [Serializable]
    public enum MonthOfYear
    {
        /// <summary>
        /// 一月
        /// </summary>
        [Description("一月")]
        January = 1,
        /// <summary>
        /// 二月
        /// </summary>
        [Description("二月")]
        February,
        /// <summary>
        /// 三月
        /// </summary>
        [Description("三月")]
        March,
        /// <summary>
        /// 四月
        /// </summary>
        [Description("四月")]
        April,
        /// <summary>
        /// 五月
        /// </summary>
        [Description("五月")]
        May,
        /// <summary>
        /// 六月
        /// </summary>
        [Description("六月")]
        June,
        /// <summary>
        /// 七月
        /// </summary>
        [Description("七月")]
        July,
        /// <summary>
        /// 八月
        /// </summary>
        [Description("八月")]
        August,
        /// <summary>
        /// 九月
        /// </summary>
        [Description("九月")]
        September,
        /// <summary>
        /// 十月
        /// </summary>
        [Description("十月")]
        October,
        /// <summary>
        /// 十一月
        /// </summary>
        [Description("十一月")]
        November,
        /// <summary>
        /// 十二月
        /// </summary>
        [Description("十二月")]
        December
    }

    /// <summary>
    /// 在 类型
    /// </summary>
    [Serializable]
    public enum MonthExecuteType
    {
        /// <summary>
        /// 天
        /// </summary>
        [Description("天")]
        Day = 1,
        /// <summary>
        /// 周
        /// </summary>
        [Description("周")]
        Week
    }

    /// <summary>
    /// 提供访问枚举信息的功能
    /// </summary>
    public static class EnumDescription
    {

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="en">要获取描述信息的枚举值</param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum en)
        {
            Type temType = en.GetType();
            MemberInfo[] memberInfos = temType.GetMember(en.ToString());
            if (memberInfos != null && memberInfos.Length > 0)
            {
                object[] objs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    return ((DescriptionAttribute)objs[0]).Description;
                }
            }
            return en.ToString();
        }

        /// <summary>
        /// 获取所有的枚举值
        /// </summary>
        /// <typeparam name="TEnum">要获取枚举值的枚举类型</typeparam>
        /// <returns></returns>
        public static List<TEnum> GetEnumValues<TEnum>() where TEnum : struct
        {
            Type type = typeof(TEnum);
            List<TEnum> list = (from f in Enum.GetNames(type)
                                select (TEnum)Enum.Parse(type, f)).ToList();
            return list;
        }

        /// <summary>
        /// 获取全部字符串
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FullString(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取日期部分字符串
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string DateString(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取时间部分字符串
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string TimeString(this DateTime datetime)
        {
            return datetime.ToString("HH:mm:ss");
        }
    }
}
