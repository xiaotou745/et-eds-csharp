using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PushServiceWCF
{
    /// <summary>
    /// PushMessage 的摘要说明
    /// </summary>
    public class PushMessage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "application /x-www-form-urlencoded";
            string msginfo = context.Request["msginfo"];
            if (string.IsNullOrEmpty(msginfo))
            {
                context.Response.Write("false");
                context.Response.End();
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    IHubContext chat = GlobalHost.ConnectionManager.GetHubContext<PushHubMoblie>();
                    chat.Clients.All.notice(msginfo);
                });
                context.Response.Write("success");
                context.Response.End();          
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}