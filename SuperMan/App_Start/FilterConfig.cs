using System.Web;
using System.Web.Mvc;

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
            //filters.Add(new HandleErrorAttribute(), 2);
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
            ETS.Util.LogHelper.LogWriterFromFilter(filterContext.Exception);
        }
    }
}
