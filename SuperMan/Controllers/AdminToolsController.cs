using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            if(!string.IsNullOrWhiteSpace(strSql))
            {
                var data = adminToolsProvider.GetDataInfoBySql(strSql.Trim());
                ViewBag.SQL = strSql;
                if (data.Rows.Count > 0)
                {
                    ViewBag.Data = data;
                    ViewBag.Quantity = data.Rows.Count;
                    return PartialView("_AdminToolsList");
                }
                else
                {
                    ViewBag.Data = null;
                    ViewBag.Quantity = data.Rows.Count;
                    return PartialView("_AdminToolsList");
                }
                
            }
            base.ViewBag.Quantity = 0;
            base.ViewBag.Data = null;
            return View();
        } 

        // GET: AdminTools
        public ActionResult Edit(string strSql)
        {
            if (!string.IsNullOrEmpty(strSql))
            {
                var data = adminToolsProvider.UpdateDataInfoBySql(strSql.Trim());
                ViewBag.SQL = strSql;
                ViewBag.Data = data;
                return new ContentResult() { Content = data.ToString() };
            }
            base.ViewBag.Data = null;
            return new ContentResult() {  Content=""};
        }
    

      
    }
}
