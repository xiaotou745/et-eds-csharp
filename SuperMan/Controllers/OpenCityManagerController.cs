using Ets.Model.Common;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class OpenCityManagerController : Controller
    {
        IAreaProvider iAreaProvider = new AreaProvider();
        // GET: OpenCityManager
        public ActionResult OpenCityManager(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
            {
                return View(new List<Ets.Model.Common.OpenCityModel>());
            }
            var openCityModel = iAreaProvider.GetOpenCityList(cityName).ToList();
            ViewBag.cityName = cityName;
            return View(openCityModel);
        }
        [HttpPost]
        public ActionResult ModifyOpenCity(string openCityCodeList,string closeCityCodeList)
        {
            var reg = iAreaProvider.ModifyOpenCityByCode(openCityCodeList, closeCityCodeList);
            iAreaProvider.ResetOpenCityListRedis();
            return Json(new ResultModel(reg, string.Empty), JsonRequestBehavior.AllowGet);
        }
    }
}