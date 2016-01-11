using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ETS.Enums;
using ETS.Library.ActiveMq;
using Ets.Model.Common;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using ETS.Util;

namespace SuperMan.App_Start.Filters
{
    public class ActionLogAttribute : ActionFilterAttribute
    {
        private static string getClientIp()
        {
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
            else
                return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="actionContext"></param>
        public async override void OnActionExecuting(ActionExecutingContext actionContext)
        {

            string responseData = HttpContext.Current.Request.QueryString.AllKeys.Aggregate("", (current, key) => current + key + "=" + HttpContext.Current.Request.QueryString[key] + "&");
            responseData = HttpContext.Current.Request.Form.AllKeys.Aggregate(responseData, (current, key) => current + key + "=" + HttpContext.Current.Request.Form[key] + "&");

            var ips = new List<string>();
            ips.Add(SystemHelper.GetLocalIP());
            ips.Add(SystemHelper.GetGateway());
            var log = new ActionLog()
            {
                userID = UserContext.Current.Id,
                userName = UserContext.Current.Name,
                requestType = actionContext.HttpContext.Request.IsAjaxRequest()?1:0,
                clientIp = getClientIp(),
                sourceSys = "superman",
                requestUrl = actionContext.HttpContext.Request.Url.AbsoluteUri,
                param = responseData,
                decryptMsg = responseData,
                contentType = actionContext.HttpContext.Request.ContentType ?? "",
                requestMethod = actionContext.HttpContext.Request.HttpMethod,
                methodName = actionContext.Controller.ControllerContext.Controller + "."+actionContext.ActionDescriptor.ActionName,
                requestTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                appServer = JsonHelper.JsonConvertToString(ips),
                header = JsonHelper.JsonConvertToString(actionContext.HttpContext.Request.Headers)
            };

            actionContext.Controller.ViewData["actionlog"] = log;
            Stopwatch stop = new Stopwatch();
            actionContext.Controller.ViewData["actionlogTime"] = stop;
            stop.Start();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuted(ActionExecutedContext actionContext)
        {
            ActionLog log = actionContext.Controller.ViewData["actionlog"] as ActionLog;
            if (actionContext.Exception == null)
            {
                log.resultJson = "";
                log.exception = "";
                log.stackTrace = "";
            }
            else
            {
                log.exception = actionContext.Exception.Message;
                log.stackTrace = actionContext.Exception.StackTrace;
            }

            log.requestEndTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var stop = actionContext.Controller.ViewData["actionlogTime"] as Stopwatch;
            stop.Stop();
            log.executeTime = stop.ElapsedMilliseconds;
            stop.Reset();

            Task.Factory.StartNew(() =>
            {
                try
                {
                    ActiveMqHelper.SendMessage(JsonHelper.JsonConvertToString(log));
                }
                catch (Exception ex)
                {
                    LogHelper.LogWriterFromFilter(actionContext.Exception);//发送错误邮件
                }

            });
        }

    }
}