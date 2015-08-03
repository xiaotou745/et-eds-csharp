using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Service.Provider.Common;

namespace SuperManWebApi.App_Start.Filters
{
    /// <summary>
    /// token 过滤器 add by caoheyang  20150731
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class TokenAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
      
        /// <summary>
        /// 验证   add by caoheyang  20150731
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var token = HttpContext.Current.Request.QueryString["token"];
            var ssid = HttpContext.Current.Request.QueryString["ssid"];
            var appkey = HttpContext.Current.Request.QueryString["appkey"];
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(ssid) || string.IsNullOrWhiteSpace(appkey))
            {
                actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                    (actionContext.ControllerContext, null);
            }
            else
            {
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                var model = redis.Get<string>(ssid + "_" + appkey) ?? redis.Get<string>(ssid + "_" + appkey+"_old");
                if (model == null || model != token)
                {
                    actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                    (actionContext.ControllerContext, null);
                }
            }

        }
    }
}