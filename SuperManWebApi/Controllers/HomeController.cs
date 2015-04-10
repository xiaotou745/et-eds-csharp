using Ets.Dao.GlobalConfig;
using SuperManCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SuperManWebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Ets.Dao.GlobalConfig.GlobalConfigDao home = new Ets.Dao.GlobalConfig.GlobalConfigDao();

            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
