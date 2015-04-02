using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperManBusinessLogic.Order_Logic;
using SuperManCommonModel.Entities;
namespace SuperMan.Controllers
{
    [WebHandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            //var tmp = OrderLogic.userLogic.GetOrders();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}