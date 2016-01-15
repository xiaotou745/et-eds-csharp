﻿using ETS.Util;
using SuperMan.App_Start;
using System.Web;
using System.Web.Mvc;
using SuperMan.App_Start.Filters;

namespace SuperMan
{
    public class FilterConfig
    {
        /// <summary>
        /// 注册全局过滤器 add by caoheyang 20150205
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new WebHandleErrorAttribute(), 1);

            filters.Add(new ActionLogAttribute());
            filters.Add(new HttpLogAttribute());//注册全局的请求记录 暂时注释掉茹化肖
            filters.Add(new HandleErrorAttribute(), 2);
        }
    }


    /// <summary>
    /// 自定义异常处理类  add by caoheyang 20150205
    /// </summary>
    public class WebHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 重写异常处理方法 add by caoheyang 20150205
        /// </summary>
        /// <param name="filterContext">上下文对象  该类继承于ControllerContext</param>
        public override void OnException(ExceptionContext filterContext)
        {
            ETS.Util.LogHelper.LogWriterFromFilter(filterContext.Exception, UserContext.Current.Id, UserContext.Current.Name);
        }
    }
}
