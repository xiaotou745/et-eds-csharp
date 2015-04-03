/*
 * Jak 20140417 add 扩展方法获取对象属性值
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Task.Common
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public  static class ExtenstionMethods
    {
        /// <summary>
        /// 获取对象属性值
        /// </summary>
        /// <param name="self">对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值</returns>
        public static object GetPropertyValue(this object self, string propertyName)
        {
            if (self == null)
            {
                return self;
            }
            Type t = self.GetType();
            PropertyInfo p = t.GetProperty(propertyName);
            return p.GetValue(self, null);
        }
        /// <summary>
        ///字符类型转换为整型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            return Convert.ToInt32(str);
        }

        /// <summary>
        /// 格式化字段
        /// </summary>
        /// <returns></returns>
        public static string ToXml<T>(this T c) where T : class
        {
            var sb = new StringBuilder();
            Type t = c.GetType();
            var propertyInfos = c.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var value = t.GetProperty(propertyInfo.Name).GetValue(c, null);
                sb.AppendLine(String.Format("{0}='{1}'", propertyInfo.Name, value));
            }
            return sb.ToString();
        }
    }
}
