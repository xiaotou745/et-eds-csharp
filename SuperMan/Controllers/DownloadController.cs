using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class DownloadController : Controller
    {
        // GET: Download
        public ActionResult edsB()
        {
            string userAgent = HttpContext.Request.UserAgent;
            //微信浏览器
            if (userAgent.ToLower().Contains("micromessenger"))
            {
                ViewBag.IsWeixin = true;
            }
            else
            {
                ViewBag.IsWeixin = false;
                //如果是苹果产品
                if (CheckAgent())
                {
                    Response.Write("<script>window.location='http://fir.im/lazt';</script>");//这里要把易淘食的下载地址放过来 
                }
                else
                {
                    Response.Redirect("/Content/app/eds_B.apk?v=" + ETS.Util.TimeHelper.GetTimeStamp());
                }
                return null;
            }
            ViewBag.UserAgent = userAgent;

            return View();
        }

        public ActionResult edsC()
        {
            string userAgent = HttpContext.Request.UserAgent;
            //微信浏览器
            if (userAgent.ToLower().Contains("micromessenger"))
            {
                ViewBag.IsWeixin = true;
            }
            else
            {
                ViewBag.IsWeixin = false;
                //如果是苹果产品
                if (CheckAgent())
                {
                    Response.Write("<script>window.location='http://fir.im/vjk8';</script>");//这里要把易淘食的下载地址放过来 
                }
                else
                {
                    Response.Redirect("/Content/app/eds_C.apk?v=" + ETS.Util.TimeHelper.GetTimeStamp());
                }
                return null;
            }
            ViewBag.UserAgent = userAgent;

            return View();
        }

        /// <summary>
        /// 判断是否为iphone和ipod和ipad
        /// 窦海超
        /// 2015年7月22日 16:39:29
        /// </summary>
        /// <returns></returns>
        private bool CheckAgent()
        {
            string agent = HttpContext.Request.UserAgent;
            string[] keywords = { "iPhone", "iPod", "iPad" };
            //排除 Windows 桌面系统            
            if (!agent.Contains("Windows NT") || (agent.Contains("Windows NT") && agent.Contains("compatible; MSIE 9.0;")))
            {
                //排除 苹果桌面系统                
                if (!agent.Contains("Windows NT") && !agent.Contains("Macintosh"))
                {
                    foreach (string item in keywords)
                    {
                        if (agent.Contains(item))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public ActionResult ViewPage1()
        {
            return View();
        }
    }
}