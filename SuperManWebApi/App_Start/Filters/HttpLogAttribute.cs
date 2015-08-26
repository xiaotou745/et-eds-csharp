using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using ETS.Util;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

//using ActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;

namespace SuperManWebApi.App_Start.Filters
{
    /// <summary>
    /// 请求返回值记录类
    /// </summary>
   // [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HttpLogAttribute : ActionFilterAttribute
    {
       private  readonly IHttpLogProvider httpLogProvider=new HttpLogProvider();


       /// <summary>
       /// 请求前
       /// </summary>
       /// <param name="actionContext"></param>
       public override void OnActionExecuting(HttpActionContext actionContext)
       {
           var controllerName = actionContext.RequestContext.RouteData.Values["controller"].ToString();
           var actionName = actionContext.RequestContext.RouteData.Values["action"].ToString();
           HttpModel httpModel = new HttpModel()
           {
               Url = HttpContext.Current.Request.Url.AbsoluteUri,
               Htype = HtypeEnum.ReqType.GetHashCode(),
               RequestBody = httpLogProvider.FormatRequestBody(HttpContext.Current.Request),
               ResponseBody = "",
               ReuqestPlatForm = RequestPlatFormEnum.WebApiPlat.GetHashCode(),
               ReuqestMethod = controllerName + "." + actionName,
               Status = 1,
               Remark = "WebApi请求记录"
           };
           httpLogProvider.LogRequestInfo(httpModel);
           base.OnActionExecuting(actionContext);
       }
       /// <summary>
       /// 请求后
       /// </summary>
       /// <param name="actionExecutedContext"></param>
       public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
       {

           var controllerName = actionExecutedContext.ActionContext.RequestContext.RouteData.Values["controller"].ToString();
           var actionName = actionExecutedContext.ActionContext.RequestContext.RouteData.Values["action"].ToString();
           dynamic content = actionExecutedContext.Response.Content;
           var result = content.Value ?? "";
           HttpModel httpModel = new HttpModel()
           {
               Url = HttpContext.Current.Request.Url.AbsoluteUri,
               Htype = HtypeEnum.RespType.GetHashCode(),
               RequestBody = httpLogProvider.FormatRequestBody(HttpContext.Current.Request),
               ResponseBody = JsonHelper.JsonConvertToString(result),
               ReuqestPlatForm = RequestPlatFormEnum.WebApiPlat.GetHashCode(),
               ReuqestMethod = controllerName + "." + actionName,
               Status = 1,
               Remark = "WebApi响应记录"
           };
           httpLogProvider.LogRequestInfo(httpModel);
           base.OnActionExecuted(actionExecutedContext);
       }
    }
}