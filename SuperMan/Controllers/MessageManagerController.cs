using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class MessageManagerController : Controller
    {
        IAreaProvider iAreaProvider = new AreaProvider();
        // GET: MessageManager
        public ActionResult MessageManager()
        {
            return View();
        }
        /// <summary>
        /// 消息编辑（添加和修改）
        /// danny-20150615
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult MessageEdit()
        {
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(0);
            return View();
        }
    }
}