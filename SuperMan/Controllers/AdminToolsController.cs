using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperMan.App_Start;

namespace SuperMan.Controllers
{
    public class AdminToolsController : Controller
    {
        private static IAdminToolsProvider adminToolsProvider
        {
            get { return new AdminToolsProvider(); }
        }
        // GET: AdminTools 
        public ActionResult AdminTools(string strSql)
        {
            if (!string.IsNullOrWhiteSpace(strSql))
            {
                var data = adminToolsProvider.GetDataInfoBySql(strSql.Trim());
                ViewBag.SQL = strSql;
                ViewBag.Quantity = data.Rows.Count;
                ViewBag.Data = data;
                return PartialView("_AdminToolsList");
            }
            ViewBag.Quantity = 0;
            ViewBag.Data = null;
            return View();
        }

        // GET: AdminTools
        public ContentResult Edit(string strSql)
        {
            if (!string.IsNullOrEmpty(strSql))
            {
                var data = adminToolsProvider.UpdateDataInfoBySql(strSql.Trim());
                ViewBag.SQL = strSql;
                ViewBag.Data = data;
                return new ContentResult() { Content = data.ToString() };
            }
            ViewBag.Data = null;
            return new ContentResult() { Content = "" };
        }

        /// <summary>
        /// 管理员工具页面 add by caoheyang 20150414
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RedisTools()
        {
            return View();
        }

        /// <summary>
        ///  管理员工具页面 Async add by caoheyang 20150414
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostRedisTools()
        {
            string key = HttpContext.Request.Form["RedisKey"];  
            int searchType=ETS.Util.ParseHelper.ToInt(HttpContext.Request.Form["SearchType"]);
            string searchKey = searchType == 0 ?  "*" + key + "*":key;
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            ViewBag.Keys = redis.Keys(searchKey);
            ViewBag.searchType = searchType;
            return View();
        }
        /// <summary>
        ///  管理员工具页面 Async add by caoheyang 20150414
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteRedisTools(string key)
        {
            try
            {
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                redis.Delete(key);
                return new JsonResult() { Data = new { status = "success" } };
            }
            catch {
                return new JsonResult() { Data = new { status = "error" } };
            }
        }
    }
}
