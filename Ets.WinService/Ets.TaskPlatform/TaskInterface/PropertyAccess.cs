using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace TaskPlatform.TaskInterface
{
    /// <summary>
    /// 动态访问对象
    /// </summary>
    /// <typeparam name="T">要访问的对象的数据类型</typeparam>
    public class PropertyAccess<T>
    {
        private static Func<object, string, object> _getValueDelegate;
        private static Action<object, string, object> _setValueDelegate;

        /// <summary>
        /// 获取指定对象的指定属性的值
        /// </summary>
        /// <param name="instance">要获取属性值的对象</param>
        /// <param name="memberName">要获取的属性的名称</param>
        /// <returns></returns>
        public object GetValue(T instance, string memberName)
        {
            return _getValueDelegate(instance, memberName);
        }

        /// <summary>
        /// 为指定对象的指定属性设置值。
        /// </summary>
        /// <param name="instance">要设置属性的对象</param>
        /// <param name="memberName">要设置的属性名</param>
        /// <param name="newValue">要设置的值</param>
        public void SetValue(T instance, string memberName, object newValue)
        {
            _setValueDelegate(instance, memberName, newValue);
        }

        /// <summary>
        /// 实例化 <see cref="PropertyAccess&lt;T&gt;"/> 类
        /// </summary>
        static PropertyAccess()
        {
            _getValueDelegate = GenerateGetValue();
            _setValueDelegate = GenerateSetValue();
        }

        /// <summary>
        /// 构建读取值函数
        /// </summary>
        /// <returns></returns>
        private static Func<object, string, object> GenerateGetValue()
        {
            var type = typeof(T);
            var instance = Expression.Parameter(typeof(object), "instance");
            var memberName = Expression.Parameter(typeof(string), "memberName");
            var nameHash = Expression.Variable(typeof(int), "nameHash");
            var calHash = Expression.Assign(nameHash, Expression.Call(memberName, typeof(object).GetMethod("GetHashCode")));
            var cases = new List<SwitchCase>();
            foreach (var propertyInfo in type.GetProperties())
            {
                var property = Expression.Property(Expression.Convert(instance, typeof(T)), propertyInfo.Name);
                var propertyHash = Expression.Constant(propertyInfo.Name.GetHashCode(), typeof(int));

                cases.Add(Expression.SwitchCase(Expression.Convert(property, typeof(object)), propertyHash));
            }
            var switchEx = Expression.Switch(nameHash, Expression.Constant(null), cases.ToArray());
            var methodBody = Expression.Block(typeof(object), new[] { nameHash }, calHash, switchEx);

            return Expression.Lambda<Func<object, string, object>>(methodBody, instance, memberName).Compile();
        }

        /// <summary>
        /// 构建设置属性值函数
        /// </summary>
        /// <returns></returns>
        private static Action<object, string, object> GenerateSetValue()
        {
            var type = typeof(T);
            var instance = Expression.Parameter(typeof(object), "instance");
            var memberName = Expression.Parameter(typeof(string), "memberName");
            var newValue = Expression.Parameter(typeof(object), "newValue");
            var nameHash = Expression.Variable(typeof(int), "nameHash");
            var calHash = Expression.Assign(nameHash, Expression.Call(memberName, typeof(object).GetMethod("GetHashCode")));
            var cases = new List<SwitchCase>();
            foreach (var propertyInfo in type.GetProperties())
            {
                // 属性必须可写
                if (!propertyInfo.CanWrite)
                {
                    continue;
                }
                var property = Expression.Property(Expression.Convert(instance, typeof(T)), propertyInfo.Name);
                var setValue = Expression.Assign(property, Expression.Convert(newValue, propertyInfo.PropertyType));
                var propertyHash = Expression.Constant(propertyInfo.Name.GetHashCode(), typeof(int));

                cases.Add(Expression.SwitchCase(Expression.Convert(setValue, typeof(object)), propertyHash));
            }
            var switchEx = Expression.Switch(nameHash, Expression.Constant(null), cases.ToArray());
            var methodBody = Expression.Block(typeof(object), new[] { nameHash }, calHash, switchEx);

            return Expression.Lambda<Action<object, string, object>>(methodBody, instance, memberName, newValue).Compile();
        }
    }
}
