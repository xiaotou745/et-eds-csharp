using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;

namespace SuperMan.App_Start
{
    /// <summary>
    ///请求记录
    /// 茹化肖
    /// 2015年8月26日15:11:43
    /// </summary>
     //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class HttpLogAttribute : ActionFilterAttribute
    {
         private readonly IHttpLogProvider httpLogProvider = new HttpLogProvider();
         /// <summary>
         /// 请求前
         /// </summary>
         /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
         {
        
                 var controllerName = filterContext.RequestContext.RouteData.Values["controller"].ToString();
                 var actionName = filterContext.RequestContext.RouteData.Values["action"].ToString();
                 HttpModel httpModel = new HttpModel()
                 {
                     Url = HttpContext.Current.Request.Url.AbsoluteUri,
                     Htype = HtypeEnum.ReqType.GetHashCode(),
                     RequestBody = httpLogProvider.FormatRequestBody(HttpContext.Current.Request),
                     ResponseBody = "",
                     ReuqestPlatForm = RequestPlatFormEnum.EdsManagePlat.GetHashCode(),
                     ReuqestMethod = controllerName + "." + actionName,
                     Status = 1,
                     Remark = "管理后台请求记录"
                 };
                 httpLogProvider.LogRequestInfo(httpModel);
     
            base.OnActionExecuting(filterContext);
        }
         /// <summary>
         /// 请求后
         /// </summary>
         /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
                 var controllerName = filterContext.RequestContext.RouteData.Values["controller"].ToString();
                 var actionName = filterContext.RequestContext.RouteData.Values["action"].ToString();
                 //var content = filterContext.RequestContext.HttpContext.Response;
                 HttpModel httpModel = new HttpModel()
                 {
                     Url = HttpContext.Current.Request.Url.AbsoluteUri,
                     Htype = HtypeEnum.ReqType.GetHashCode(),
                     RequestBody = httpLogProvider.FormatRequestBody(HttpContext.Current.Request),
                     ResponseBody = "",
                     ReuqestPlatForm = RequestPlatFormEnum.EdsManagePlat.GetHashCode(),
                     ReuqestMethod = controllerName + "." + actionName,
                     Status = 1,
                     Remark = "管理后台响应记录"
                 };
                 httpLogProvider.LogRequestInfo(httpModel);
            base.OnActionExecuted(filterContext);
        }
    }
}