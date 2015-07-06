using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebEdaiSong.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {           
            return View();
        }

        public ActionResult Business()
        {            
            return View();
        }

        public ActionResult Clienter()
        {
            return View();
        }

    }
}
