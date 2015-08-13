using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.DataModel.YeePay;
using Ets.Model.ParameterModel.YeePay;
using Ets.Service.Provider.YeePay;
using SuperMan.App_Start;

namespace SuperMan.Controllers
{
    public class YeePayRunningManagerController : BaseController
    {
        private YeePayRunningProvider yeePayRunningProvider = new YeePayRunningProvider();
        
        public ActionResult YeePayRunningManager()
        { 
            YeePayRunningCriteria yeePayRunningCriteria = new YeePayRunningCriteria();
            TryUpdateModel(yeePayRunningCriteria);
            var yprList = new YeePayRunningProvider().Get(yeePayRunningCriteria); 

            return View(yprList);
        } 
        
        /// <summary>
        /// 获取数据 分页
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostYeePayRunningManager(int pageindex = 1)
        {
            YeePayRunningCriteria yeePayRunningCriteria = new YeePayRunningCriteria();
            TryUpdateModel(yeePayRunningCriteria); 
            var dcList = yeePayRunningProvider.Get(yeePayRunningCriteria);//获取物流公司
            return PartialView("_YeePayRunningList", dcList);
        }
         

        public ActionResult AddYeePayRunning(YeePayRunningAccountModel yeePayRunningAccountModel)
        {
            TryUpdateModel(yeePayRunningAccountModel);
            yeePayRunningAccountModel.Operator = UserContext.Current.Name;

            var result = yeePayRunningProvider.Add(yeePayRunningAccountModel);
            if (result.Status == 1)
            {
                return Json(new ResultModel(true, result.Message), JsonRequestBehavior.DenyGet);
            }
            return Json(new ResultModel(false, result.Message), JsonRequestBehavior.DenyGet);
        }
    }
}