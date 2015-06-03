using Ets.Dao.GlobalConfig;
using Ets.Service.Provider.MyPush;
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
            //Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", 1761.ToString(), string.Empty);

            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
