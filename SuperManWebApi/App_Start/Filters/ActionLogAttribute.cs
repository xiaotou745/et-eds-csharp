using System;
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
            actionContext.Request.Properties["actionlog"] = new ActionLog()
            {
                userID = -1,
                userName = "",
                requestType = 0,
                clientIp = actionContext.Request.RequestUri.Host,
                sourceSys = "supermanapi",
                requestUrl = actionContext.Request.RequestUri.ToString(),
                param = responseData,
                decryptMsg = responseData,
                contentType = actionContext.Request.Content.Headers.ContentType.ToString(),
                requestMethod = actionContext.Request.Method.ToString(),
                methodName = actionContext.ActionDescriptor.ActionName,
                requestTime = DateTime.Now.ToString(),
                appServer = JsonHelper.JsonConvertToString(ips)
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
  
            log.requestEndTime = DateTime.Now.ToString();
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