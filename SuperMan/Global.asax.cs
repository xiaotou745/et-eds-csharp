using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SuperManCore;


namespace SuperMan
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// 注册全局过滤器 add by 曹赫洋 20150205
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AppHandleErrorAttribute(), 1);
            filters.Add(new HandleErrorAttribute(), 2);
        }
    }


    /// <summary>
    /// 自定义异常处理类  add by caoheyang 20150205
    /// </summary>
    public class AppHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 重写异常处理方法 add by caoheyang 20150205
        /// </summary>
        /// <param name="filterContext">上下文对象  该类继承于ControllerContext</param>
        public override void OnException(ExceptionContext filterContext)
        {
            Exception error = filterContext.Exception; //异常类
            string Message = error.Message;//错误信息
            string Url = HttpContext.Current.Request.RawUrl;//错误发生地址
            filterContext.ExceptionHandled = true;
            string message = string.Format("消息类型：{0}\r\n消息内容：{1}\r\n引发异常的方法：{2}\r\n引发异常源：{3}"
              , filterContext.Exception.GetType().Name
              , filterContext.Exception.Message
               , filterContext.Exception.TargetSite
               , filterContext.Exception.Source + filterContext.Exception.StackTrace
               );
            LogHelper.LogWriter(error);
        }
    }
}
