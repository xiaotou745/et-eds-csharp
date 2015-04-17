using Ets.Model.Common;
using Ets.Model.DomainModel.Group;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using ETS.Enums;
using ETS.Security;
using ETS.Util;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Http.Controllers;

namespace OpenApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    #region  普通 ActionFilter add by caoheyang 20150319

    /// <summary>
    /// sign 以及参数合法性验证过滤器 add by caoheyang 20150318
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Class)]
    public class SignOpenApiAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        System.Diagnostics.Stopwatch stop = new System.Diagnostics.Stopwatch();
        /// <summary>
        /// 重写OnActionExecuting方法   在进入控制器之前验证 sign以及 参数合法性信息 add by caoheyang 20150318
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            stop.Start();
            dynamic paramodel = actionContext.ActionArguments["paramodel"]; //当前请求的参数对象 
            lock (paramodel)
            {
                if (actionContext.ModelState.Count > 0 || paramodel == null) //参数错误，请求中止
                {
                    actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                            (actionContext.ControllerContext, ResultModel<object>.Conclude(OrderApiStatusType.ParaError, actionContext.ModelState.Keys));
                    return;
                }
                IGroupProvider groupProvider = new GroupProvider();
                GroupApiConfigModel groupCofigInfo = groupProvider.GetGroupApiConfigByAppKey(paramodel.app_key, paramodel.v);
                if (groupCofigInfo != null && groupCofigInfo.IsValid == 1)//集团可用，且有appkey信息
                {
                    string signStr = groupCofigInfo.AppSecret + "app_key" + paramodel.app_key + "timestamp"
                        + paramodel.timestamp + "v" + paramodel.v + groupCofigInfo.AppSecret;
                    string sign = MD5.Encrypt(signStr);
                    paramodel.group = ParseHelper.ToInt(groupCofigInfo.GroupId, 0);
                    actionContext.ActionArguments["paramodel"] = paramodel; ;
                    if (sign != paramodel.sign)   //sign错误，请求中止
                        actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                            (actionContext.ControllerContext, ResultModel<object>.Conclude(OrderApiStatusType.SignError));
                }
                else
                    actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                           (actionContext.ControllerContext, ResultModel<object>.Conclude(OrderApiStatusType.SignError));  //sign错误，请求中止
            }

        }
        /// <summary>
        /// 异步记录日志
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    stop.Stop();
                    LogHelper.LogWriter("接口" + actionExecutedContext.Request.RequestUri + "请求时间：" + stop.Elapsed);
                    stop.Reset();
                });
            base.OnActionExecuted(actionExecutedContext);
        }
    }

    #endregion

    #region  ExceptionFilter add by caoheyang 20150319

    /// <summary>
    /// 自定义全局异常处理类  add by caoheyang 20150319
    /// </summary>
    public class OpenApiHandleErrorAttribute : ExceptionFilterAttribute
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

    /// <summary>
    /// 自定义action异常处理类,捕获异常，返回系统错误提示信息  add by caoheyang 20150319
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Class)]
    public class OpenApiActionErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写异常处理方法 add by caoheyang 20150205
        /// </summary>
        /// <param name="filterContext">上下文对象  该类继承于ControllerContext</param>
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            filterContext.Response = filterContext.ActionContext.ActionDescriptor.ResultConverter.
              Convert(filterContext.ActionContext.ControllerContext, ResultModel<object>.Conclude(OrderApiStatusType.SystemError));
        }
    }

    #endregion
}
