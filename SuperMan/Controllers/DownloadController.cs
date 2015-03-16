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
                Response.Redirect("/Content/app/eds_B.apk");
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
                Response.Redirect("/Content/app/eds_C.apk");
                return null;
            }
            ViewBag.UserAgent = userAgent;

            return View();
        }
    }
}