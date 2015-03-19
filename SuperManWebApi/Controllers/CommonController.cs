using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SuperManWebApi.Controllers
{
    public class CommonController : ApiController
    {
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
    }
}