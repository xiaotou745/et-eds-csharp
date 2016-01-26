using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ETS.Library.ActiveMq;
using Ets.Model.Common;
using ETS.Util;
using Ets.Model.ParameterModel.Common;
using ETS.Security;

namespace OpenApi.App_Start.Filters
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
            //try
            //{
                string responseData = "";
                string decryptData = "";
                if (actionContext.Request.Method == HttpMethod.Get)
                {
                    foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
                    {
                        responseData = responseData + key + "=" + HttpContext.Current.Request.QueryString[key] + "&";
                    }
                }
                else if (actionContext.Request.Method == HttpMethod.Post)
                {
                    var parameterDescriptors = actionContext.ActionDescriptor.GetParameters();
                    if (parameterDescriptors.Count > 0 && (parameterDescriptors[0].ParameterType == typeof(ParamModel)))
                    {
                        object obj = actionContext.ActionArguments[actionContext.ActionArguments.Keys.ToList()[0]];
                        if (obj != null)
                        {
                            responseData = ((ParamModel)obj).data;
                            if (!string.IsNullOrEmpty(responseData))
                            {
                                decryptData = AESApp.AesDecrypt(responseData.Replace(' ', '+')/*TODO 暂时用Replace*/);
                            }
                        }
                    }
                    else
                    {
                        var task = actionContext.Request.Content.ReadAsStreamAsync();
                        var content = string.Empty;
                        var sm = task.Result;
                        sm.Seek(0, SeekOrigin.Begin);//设置流的开始位置
                        var bytes = sm.ToByteArray();
                        responseData = bytes.ToStr();
                    }
                }
                if (decryptData == "")
                {
                    decryptData = responseData == null ? "" : responseData;
                }

                List<string> ips = new List<string>();
                ips.Add(SystemHelper.GetLocalIP());
                ips.Add(SystemHelper.GetGateway());
                ActionLog log = new ActionLog()
                {
                    userID = -1,
                    userName = "",
                    requestType = 0,
                    clientIp = getClientIp(),
                    sourceSys = "netopenapi",
                    requestUrl = actionContext.Request.RequestUri.ToString().IndexOf("?") > 0 ?
                    actionContext.Request.RequestUri.ToString().Substring(0, actionContext.Request.RequestUri.ToString().IndexOf("?")) :
                                 actionContext.Request.RequestUri.ToString(),
                    param = responseData,
                    decryptMsg = decryptData,
                    contentType = actionContext.Request.Content.Headers.ContentType == null ? "" :
                    actionContext.Request.Content.Headers.ContentType.ToString(),
                    requestMethod = actionContext.Request.Method.ToString(),
                    methodName =
                    actionContext.ControllerContext.ControllerDescriptor.ControllerType + "."
                    + actionContext.ControllerContext.ControllerDescriptor.ControllerName + "." +
                    actionContext.ActionDescriptor.ActionName,

                    requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    appServer = JsonHelper.JsonConvertToString(ips),
                    header = JsonHelper.JsonConvertToString(actionContext.Request.Headers)
                };

                actionContext.Request.Properties["actionlog"] = log;

                Stopwatch stop = new Stopwatch();
                actionContext.Request.Properties["actionlogTime"] = stop;
                stop.Start();
            //}
            //catch (Exception ex) { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            //try
            //{
                ActionLog log = actionContext.Request.Properties["actionlog"] as ActionLog;
                if (actionContext.Exception == null)
                {
                    var response =
                        actionContext.Response.Content.ReadAsAsync(
                            actionContext.ActionContext.ActionDescriptor.ReturnType);
                    log.resultJson = JsonHelper.JsonConvertToString(response.Result);
                    log.exception = "";
                    log.stackTrace = "";
                }
                else
                {
                    log.exception = actionContext.Exception.Message;
                    log.stackTrace = actionContext.Exception.StackTrace;

                }

                log.requestEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var stop = actionContext.Request.Properties["actionlogTime"] as Stopwatch;
                stop.Stop();
                log.executeTime = stop.ElapsedMilliseconds;
                stop.Reset();

                //调用线程池，异步发送mq消息
                ActiveMqHelper.AsynSendMessage(JsonHelper.JsonConvertToString(log));
            //}
            //catch (Exception ex) { }
        }
    }
}