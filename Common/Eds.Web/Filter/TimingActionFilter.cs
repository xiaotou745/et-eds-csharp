using System.Diagnostics;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Eds.Web.Attribute;

namespace Eds.Web.Filter
{
    /// <summary>
    /// 记录每个action的执行时间filter
    /// </summary>
    public class TimingActionFilter : ActionFilterAttribute 
    {
        private const string Key = "__action_duration__";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (SkipLogging(actionContext))
            {
                return;
            }
            var stopWatch = new Stopwatch();
            actionContext.Request.Properties[Key] = stopWatch;
            stopWatch.Start();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!actionExecutedContext.Request.Properties.ContainsKey(Key))
            {
                return;
            }

            var stopWatch = actionExecutedContext.Request.Properties[Key] as Stopwatch;
            if (stopWatch != null)
            {
                stopWatch.Stop();
                var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                var controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                Debug.Print(string.Format("[Execution of{0}- {1} took {2}.]", controllerName, actionName, stopWatch.Elapsed));
            }
        }

        /// <summary>
        /// 是否忽略写日志
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static bool SkipLogging(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<NoLogAttribute>().Any() ||
                    actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<NoLogAttribute>().Any();
        }
    }
}
