using Ets.Service.Provider.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            new UserProvider().Register(new Ets.Model.UserModel());
            return View();
        }
    }
}