﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using ETS.Enums;
using ETS.Library.ActiveMq;
using Ets.Model.Common;
using Ets.Service.Provider.Common;
using ETS.Util;

namespace SuperManWebApi.App_Start.Filters
{
    /// <summary>
    /// 对内接口的参数合法性验证 add by caoheyang 20150512
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ActionLogAttribute : System.Web.Http.Filters.ActionFilterAttribute
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
        public async override void OnActionExecuting(HttpActionContext actionContext)
        {
            //获取入参
            Stream stream = await actionContext.Request.Content.ReadAsStreamAsync();
            Encoding encoding = Encoding.UTF8;
            stream.Position = 0;
            string responseData = "";
            using (StreamReader reader = new StreamReader(stream, encoding))
            {
                responseData = reader.ReadToEnd().ToString();
            }

            List<string> ips = new List<string>();
            ips.Add(SystemHelper.GetLocalIP());
            ips.Add(SystemHelper.GetGateway());
            var controllerName = actionContext.RequestContext.RouteData.Values["controller"].ToString();
            var actionName = actionContext.RequestContext.RouteData.Values["action"].ToString();
            actionContext.Request.Properties["actionlog"] = new ActionLog()
            {
                userID = -1,
                userName = "",
                requestType = 0,
                clientIp = getClientIp(),
                sourceSys = "supermanapi",
                requestUrl = actionContext.Request.RequestUri.ToString(),
                param = responseData,
                decryptMsg = responseData,
                contentType = actionContext.Request.Content.Headers.ContentType.ToString(),
                requestMethod = actionContext.Request.Method.ToString(),
                methodName = controllerName + "." + actionName,
                requestTime =DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                appServer = JsonHelper.JsonConvertToString(ips),
                header = JsonHelper.JsonConvertToString(actionContext.Request.Headers)
            };

            Stopwatch stop = new Stopwatch();
            actionContext.Request.Properties["actionlogTime"] = stop;
            stop.Start();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            ActionLog log = actionContext.Request.Properties["actionlog"] as ActionLog;
            if (actionContext.Exception == null)
            {
                var response =
                    actionContext.Response.Content.ReadAsAsync(
                        actionContext.ActionContext.ActionDescriptor.ReturnType);
                log.resultJson = JsonHelper.JsonConvertToString(response.Result);
            }
            else
            {
                log.exception = actionContext.Exception.Message;
                log.stackTrace = actionContext.Exception.StackTrace;
              
            }
  
            log.requestEndTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var stop = actionContext.Request.Properties["actionlogTime"] as Stopwatch;
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