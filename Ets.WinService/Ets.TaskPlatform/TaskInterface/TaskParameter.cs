#region 类信息
/* ==============================================================================
 * 功能描述：TaskConfig  
 * 创建机器：PC00168
 * 创 建 者：Administrator 
 * 创建日期：2013-10-04 16:22:49  5 
 * ==============================================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace TaskPlatform.TaskInterface
{
    /// <summary>
    /// 计划任务运行时参数信息
    /// </summary>
    [Serializable]
    public class TaskParameter
    {
        private string _key = string.Empty;
        /// <summary>
        /// 参数Key
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        private string _description = string.Empty;
        /// <summary>
        /// 参数描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private ParameterType _valueType = ParameterType.String;
        /// <summary>
        /// 参数值类型
        /// </summary>
        public ParameterType ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }
        private bool _platformCanChangeValueType = false;
        /// <summary>
        /// 平台是否可更改值参数值类型(默认不允许修改)
        /// </summary>
        public bool PlatformCanChangeValueType
        {
            get { return _platformCanChangeValueType; }
            set { _platformCanChangeValueType = value; }
        }
        private string _value = string.Empty;
        /// <summary>
        /// 参数值
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private List<string> _options = null;
        /// <summary>
        /// 参数候选值
        /// </summary>
        public List<string> Options
        {
            get
            {
                if (_options == null)
                {
                    _options = new List<string>();
                }
                return _options;
            }
            set { _options = value; }
        }
        private bool _canNotInOptions = false;
        /// <summary>
        /// 参数是否可以不在候选值之列
        /// </summary>
        public bool CanNotInOptions
        {
            get { return _canNotInOptions; }
            set { _canNotInOptions = value; }
        }

        /// <summary>
        /// 得到深度拷贝副本
        /// </summary>
        /// <returns></returns>
        public TaskParameter Copy()
        {
            return this.MemberwiseClone() as TaskParameter;
        }
    }

    /// <summary>
    /// 参数值类型
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        [Description("string")]
        String = 0,
        /// <summary>
        /// int
        /// </summary>
        [Description("int")]
        Int32,
        /// <summary>
        /// bool
        /// </summary>
        [Description("bool")]
        Boolean,
        /// <summary>
        /// double
        /// </summary>
        [Description("double")]
        Double,
        /// <summary>
        /// long
        /// </summary>
        [Description("long")]
        Int64
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
