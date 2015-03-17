using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OpenApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{Id}",//routeTemplate: "api/{controller}/{id}",   更改原有路由,否则时能用HTTP Verbs写  edit by caoheyang 20150316
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
