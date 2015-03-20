using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Ets.Model.Common;
using Ets.Service.IProvider.AuthorityMenu;
using Ets.Service.Provider.Authority;

namespace SuperMan.App_Start
{
    /// <summary>
    /// 验证用户是否具有某访问权限-平扬 2015.3.20
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorityValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();
            if (UserContext.Current.Id == 0)
            {
                filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "account", action = "login" }));
            }
            else
            {
                IAuthorityMenuProvider iauthority = new AuthorityMenuProvider();
                var myMenus = iauthority.GetMenusByAccountId(UserContext.Current.Id);//获取用户的所有权限
                if (myMenus != null && myMenus.Count > 0)
                {
                    //没有访问权限则提示用户
                    if (myMenus.All(authorityMenuModel => authorityMenuModel.Url != "/" + controllerName + "/" + actionName))
                    {
                        filterContext.Result = new JsonResult { Data = new ResultModel(false, "抱歉，你不具有当前操作的权限"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
                else
                {
                    filterContext.Result = new ContentResult { Content = @"抱歉,你不具有当前操作的权限！" };
                }
            }
        }
    }

}