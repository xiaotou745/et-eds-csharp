using Ets.Dao.GlobalConfig;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.MyPush;
using ETS.Util;
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

            GlobalConfigModel globalSetting = new GlobalConfigProvider().GlobalConfigMethod(0);
            //取到任务的接单时间、从缓存中读取完成任务时间限制，判断要用户点击完成时间>接单时间+限制时间 
            int limitFinish = ParseHelper.ToInt(globalSetting.CompleteTimeSet, 0);
            LogHelper.LogWriter("完成时间:" + limitFinish);
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
