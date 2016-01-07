using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace SuperManWebApi.App_Start.Filters
{
    /// <summary>
    /// 自定义全局异常处理类  add by caoheyang 20150205
    /// </summary>
    public class ApiHandleErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写异常处理方法 add by caoheyang 20150205
        /// </summary>
        /// <param name="filterContext">上下文对象  该类继承于ControllerContext</param>
        public async override void OnException(HttpActionExecutedContext filterContext)
        {
            ETS.Util.LogHelper.LogWriterFromFilter(filterContext.Exception);
        }
    }
}