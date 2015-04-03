using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Task.Common
{
    public class ExpressionInvoke
    {
        public delegate object GetMethodInvokeHandler<T>(T target);

        public delegate void SetMethodInvokeHandler<T>(T target, Object value);

        /// <summary>
        ///     通过Expression Tree的方式创建用于属性操作的委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SetMethodInvokeHandler<T> CreateSetterDelegate<T>(MethodInfo m, Type type)
        {
            var paramObj = Expression.Parameter(typeof(T), "obj");
            var paramVal = Expression.Parameter(typeof(object), "val");
            var bodyVal = Expression.Convert(paramVal, type);
            var body = Expression.Call(paramObj, m, bodyVal);
            var lambda = Expression.Lambda<SetMethodInvokeHandler<T>>(body, paramObj, paramVal);
            return lambda.Compile();
        }

        /// <summary>
        ///     通过Expression Tree的方式创建用于获取属性的委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static GetMethodInvokeHandler<T> CreateGetterDelegate<T>(MethodInfo m)
        {
            var paramObj = Expression.Parameter(typeof(T), "obj");
            var body = Expression.Property(paramObj, m);
            var bodyVal = Expression.Convert(body, typeof(object));
            var lambda = Expression.Lambda<GetMethodInvokeHandler<T>>(bodyVal, paramObj);
            return lambda.Compile();
        }
    }
}
