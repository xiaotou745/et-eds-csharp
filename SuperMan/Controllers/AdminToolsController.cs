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
                if (data.Columns.Count > 0)
                {
                    base.ViewBag.Data = data;
                    return PartialView("_AdminToolsList");
                }
                
            }
            base.ViewBag.Data = null;
            return View();
        } 

        // GET: AdminTools
        public ContentResult Edit(string strSql)
        {
            if (!string.IsNullOrEmpty(strSql))
            {
                var data = adminToolsProvider.UpdateDataInfoBySql(strSql.Trim());
                ViewBag.SQL = strSql;
                if (data > 0)
                {
                    base.ViewBag.Data = data;
                    return new ContentResult() { Content = data.ToString() };
                } 
            }
            base.ViewBag.Data = null;
            return new ContentResult() {  Content=""};
        }
    

      
    }
}
