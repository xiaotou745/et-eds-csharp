﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc.Async;
using Ets.Model.Common;
using Ets.Service.Provider.Common;
using SuperManCore;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace SuperManWebApi
{
    public class FilterConfig
    {
        /// <summary>
        /// 注册全局过滤器 add by caoheyang 20150205
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    /// <summary>
    /// 用于接口统计--平扬.2015.4.14
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class ApiVersionStatisticAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>
        /// 重写OnActionExecuting方法
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Task.Factory.StartNew(() =>
            {
                var verSion = actionContext.ActionArguments["Version"] as string;
                var model = new ApiVersionStatisticModel
                {
                    APIName = actionContext.Request.RequestUri.AbsolutePath,
                    CreateTime = DateTime.Now,
                    Version = verSion
                };
                new ApiVersionProvider().AddApiRecords(model);
            });
        }

    }

    /// <summary>
    /// 用于记录接口请求总耗时 add by caoheyang 
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method|AttributeTargets.Class)]
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
                    LogHelper.LogWriter("接口" + actionExecutedContext.Request.RequestUri + "请求时间：" + stop.ElapsedMilliseconds);
                    stop.Reset();
                });
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }

    /// <summary>
    /// 自定义全局异常处理类  add by caoheyang 20150205
    /// </summary>
    public class ApiHandleErrorAttribute : ExceptionFilterAttribute
    {
        private const string Key = "__action_duration__";
        /// <summary>
        /// 重写异常处理方法 add by caoheyang 20150205
        /// </summary>
        /// <param name="filterContext">上下文对象  该类继承于ControllerContext</param>
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            var stop = filterContext.Request.Properties[Key] as Stopwatch;
            if (stop != null)
            {
                Task.Factory.StartNew(() =>
                {
                    stop.Stop();
                    ETS.Util.LogHelper.LogWriter("接口" + filterContext.Request.RequestUri + "请求时间：" + stop.ElapsedMilliseconds);
                    stop.Reset();
                });
            }
            LogHelper.LogWriterFromFilter(filterContext.Exception);
        }
    }
}
