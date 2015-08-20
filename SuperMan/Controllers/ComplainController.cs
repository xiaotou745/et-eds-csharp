using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Model.ParameterModel.Complain;
using Ets.Model.ParameterModel.DeliveryCompany;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Complain;
using Ets.Service.Provider.DeliveryCompany;
using ETS.Util;
using SuperMan.App_Start;

namespace SuperMan.Controllers
{
    public class ComplainController : Controller
    {
        private readonly IAreaProvider areaProvider = new AreaProvider();
        /// <summary>
        /// 获取投诉数据
        /// wc
        /// </summary>
        /// <returns></returns>
        public ActionResult Complain()
        {
            ComplainCriteria complainCriteria = new ComplainCriteria();
            TryUpdateModel(complainCriteria);
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = areaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var list = new ComplainProvider().Get(complainCriteria); 
            return View(list);
        }
        /// <summary>
        /// 获取投诉数据 分页
        /// wc
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostComplain(int pageindex = 1)
        {
            ComplainCriteria complainCriteria = new ComplainCriteria();
            TryUpdateModel(complainCriteria);
            int userType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = areaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(userType));
            var list = new ComplainProvider().Get(complainCriteria);
            return PartialView("_ComplainList", list);
        } 
    }
}