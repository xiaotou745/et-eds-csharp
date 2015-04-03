using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Task.Common
{
    public static class PropertyHelper
    {
        /// <summary>
        /// 给属性赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <param name="fieldValue"></param>
        public static void SetPropertyValue<T>(this PropertyInfo property, T entity, object fieldValue) where T : class, new()
        {
            if (Convert.IsDBNull(fieldValue)) return;

            //可空类型转换为基类型
            var type = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                           ? Nullable.GetUnderlyingType(property.PropertyType)
                           : property.PropertyType;

            if (type.IsEnum && Enum.IsDefined(type, fieldValue))
            {
                property.SetValue(entity, Enum.Parse(type, fieldValue.ToString()), null);
                return;
            }

            DateTime dt;
            if (type == typeof(DateTime) && DateTime.TryParse(fieldValue.ToString(), out dt))
            {
                property.SetValue(entity, dt, null);
                return;
            }
            try
            {
                property.SetValue(entity, Convert.ChangeType(fieldValue, type), null);
            }
            catch
            {
                Console.Write("Convert Failed.");
            }
        }
    }
}
