using System;
using System.Web.Http.Controllers;
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
    /// 用于接口统计
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class ApiTongJiAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>
        /// 重写OnActionExecuting方法
        /// </summary>
        /// <param name="actionContext"></param>
        //public override void OnActionExecuting(HttpActionContext actionContext)
        //{
        //    var verSion = actionContext.ActionArguments["Version"] as string;
        //    var model=new ApiVersionStatisticModel
        //    {
        //        APIName = actionContext.Request.RequestUri.AbsolutePath,
        //        CreateTime =DateTime.Now,
        //        Version = verSion
        //    };
        //    new ApiVersionProvider().AddApiRecords(model);

        //}
    }

    /// <summary>
    /// 自定义异常处理类  add by caoheyang 20150205
    /// </summary>
    public class ApiHandleErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写异常处理方法 add by caoheyang 20150205
        /// </summary>
        /// <param name="filterContext">上下文对象  该类继承于ControllerContext</param>
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            LogHelper.LogWriterFromFilter(filterContext.Exception);
        }
    }
}
