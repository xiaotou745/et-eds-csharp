using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SuperManWebApi.App_Start.Filters
{/// <summary>
    /// 用于记录接口请求总耗时 add by caoheyang 
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method | AttributeTargets.Class)]
    public class ExecuteTimeLogAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private const string Key = "__action_duration__";
        /// <summary>
        /// 重写OnActionExecuting方法
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Stopwatch stop = new Stopwatch();
            actionContext.Request.Properties[Key] = stop;
            stop.Start();
            base.OnActionExecuting(actionContext);
        }
        /// <summary>
        /// 异步记录日志
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!actionExecutedContext.Request.Properties.ContainsKey(Key))
            {
                return;
            }
            var stop = actionExecutedContext.Request.Properties[Key] as Stopwatch;
            if (stop != null)
            {
                Task.Factory.StartNew(() =>
                {
                    stop.Stop();
                    ETS.Util.LogHelper.LogWriter("接口" + actionExecutedContext.Request.RequestUri + "请求时间：" + stop.ElapsedMilliseconds);
                    stop.Reset();
                });
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}