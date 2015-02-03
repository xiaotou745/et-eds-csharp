using SuperManCore;
using SuperManDataAccess;
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
            ViewBag.Title = "Home Page";
            //using (var db = new supermanEntities())
            //{
            //    db.Configuration.ValidateOnSaveEnabled = false;
            //    db.clienter.ToList<clienter>().ForEach(item => item.Password = MD5Helper.MD5(item.Password));
            //    db.business.ToList<business>().ForEach(item => item.Password = MD5Helper.MD5(item.Password));
            //    db.account.ToList<account>().ForEach(item => item.Password = MD5Helper.MD5(item.Password));
            //    int res = db.SaveChanges();
            //    db.Configuration.ValidateOnSaveEnabled = true;
            //}
            return View();
        }
    }
}
