using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SuperManWebApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.Add(new Route("testroute-{para1}.cshtml", new EncryptRouteHandler())); //注册请求地址加密的处理RouteHandler 
            //routes.MapRoute(name: "testroute", url: "testroute-{para1}.cshtml"); //注册路由规则
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }


    /// <summary>
    /// 请求地址加密处理RouteHandler add by caoheyang 20150206
    /// </summary>
    public class EncryptRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.RouteData.Values["controller"] = "home";
            requestContext.RouteData.Values["action"] = "index";//requestContext.RouteData.Values["para1"].ToString().ToLower(); 
            return new MvcHandler(requestContext);
        }

    }
}
