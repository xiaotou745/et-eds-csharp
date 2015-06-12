using System;

namespace Eds.Web.Attribute
{
    /// <summary>
    /// 不写日志标志声明属性
    /// 凡是标记了此属性的controller或action，不会记录日志；
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)] 
    public class NoLogAttribute
    {
    }
}
