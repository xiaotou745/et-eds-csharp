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

namespace OpenApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    /// <summary>
    /// sign 以及参数合法性验证过滤器 add by caoheyang 20150318
    /// </summary>
    public class SignOpenApiAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>
        /// 重写OnActionExecuting方法   在进入控制器之前验证 sign以及 参数合法性信息 add by caoheyang 20150318
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

                dynamic paramodel = actionContext.ActionArguments["paramodel"]; //当前请求的参数对象 
                lock (paramodel)
                {
                    if (actionContext.ModelState.Count > 0 || paramodel == null) //参数错误，请求中止
                        actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                                (actionContext.ControllerContext, ResultModel<dynamic>.Conclude(OrderApiStatusType.ParaError));
                    IGroupProvider groupProvider = new GroupProvider();
                    GroupApiConfigModel groupCofigInfo = groupProvider.GetGroupApiConfigByAppKey(paramodel.app_key, paramodel.v).Data;
                    if (groupCofigInfo != null && groupCofigInfo.IsValid == 1)
                    {
                        string signStr = groupCofigInfo.AppSecret + "app_key=" + paramodel.app_key + "timestamp"
                            + paramodel.timestamp + "v=" + paramodel.v + groupCofigInfo.AppSecret;
                        string sign = MD5.Encrypt(signStr);
                        paramodel.group = ParseHelper.ToInt(groupCofigInfo.GroupId, 0);
                        actionContext.ActionArguments["paramodel"] = paramodel; ;
                        if (sign != paramodel.sign)   //sign错误，请求中止
                            actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                                (actionContext.ControllerContext, ResultModel<dynamic>.Conclude(OrderApiStatusType.SignError));
                    }
                    else
                        actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                               (actionContext.ControllerContext, ResultModel<dynamic>.Conclude(OrderApiStatusType.SignError));  //sign错误，请求中止
                }
    
     
        }
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
