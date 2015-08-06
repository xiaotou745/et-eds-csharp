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
    /// token ¹ýÂËÆ÷ add by caoheyang  20150731
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class TokenAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
      
        /// <summary>
        /// ÑéÖ¤   add by caoheyang  20150731
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var token = HttpContext.Current.Request.Headers["token"];
            var ssid = HttpContext.Current.Request.Headers["ssid"];
            var appkey = HttpContext.Current.Request.Headers["appkey"];
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(ssid) || string.IsNullOrWhiteSpace(appkey))
            {
                actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                    (actionContext.ControllerContext, null);
            }
            else
            {
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                var model = redis.Get<string>(ssid + "_" + appkey);
                var oldemodel=redis.Get<string>(ssid + "_" + appkey+"_old");

                if ((string.IsNullOrWhiteSpace(model) && string.IsNullOrWhiteSpace(oldemodel))
                    ||(token != model && oldemodel != model))
                {
                    actionContext.Response = actionContext.ActionDescriptor.ResultConverter.Convert
                        (actionContext.ControllerContext, null);
                }
             
            }

        }
    }
}