using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.Provider.Common;
using ETS.Util;
using SuperMan.App_Start;
using SuperManCore.Common;
using Ets.Service.IProvider.Subsidy;
using Ets.Service.Provider.Subsidy;

namespace SuperMan.Controllers
{
    [WebHandleError]
    public class SubsidyManagerController : BaseController
    {
        ISubsidyProvider iSubsidyProvider = new SubsidyProvider();
        // GET: SubsidyManager
        public ActionResult SubsidyManager()
        {
            //account account = HttpContext.Session["user"] as account;
            //if (account == null)
            //{
            //    Response.Redirect("/account/login");
            //    return null;
            //}  
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id
            var criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria() { GroupId = SuperMan.App_Start.UserContext.Current.GroupId };
            var pagedList = iSubsidyProvider.GetSubsidyList(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult PostSubsidyManager(int pageindex = 1)
        {
            Ets.Model.DomainModel.Subsidy.HomeCountCriteria criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria();
            TryUpdateModel(criteria);
            var pagedList = iSubsidyProvider.GetSubsidyList(criteria);
            return PartialView("_SubsidyManagerList", pagedList);
        }

        [HttpPost]
        public JsonResult Save(Ets.Model.ParameterModel.Subsidy.SubsidyModel model)
        {
            bool b = iSubsidyProvider.SaveData(model);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 结算功能
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SettlementFunction()
        {
            //account account = HttpContext.Session["user"] as account;
            //if (account == null)
            //{
            //    Response.Redirect("/account/login");
            //    return null;
            //}
            
            return View();
        }
        /// <summary>
        /// 时间补贴设置
        /// </summary>
        /// <returns></returns>
        public ActionResult TimeSubsidies()
        {
            var list = new GlobalConfigProvider().GetTimeSubsidies(0); 
            return View(list);
        }
        /// <summary>
        /// 添加时间补贴设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddTimeSubsidies(GlobalConfigSubsidies model)
        {
            var list = new GlobalConfigProvider().GetTimeSubsidies(0); 
            list.Add(model);
            var newlist = (from globalConfigTimeSubsidiese in list
                           orderby ParseHelper.ToInt(globalConfigTimeSubsidiese.Value1) ascending 
                select globalConfigTimeSubsidiese).ToList();
            string values = GetTimesValues(newlist);
            bool b = new GlobalConfigProvider().UpdateTimeSubsidies(UserContext.Current.Name, values, "添加时间补贴设置操作-设置之后的值:" + values, 0, -1);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除时间补贴设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DeleteTimeSubsidies(int id)
        {
            //读取全部时间配置
            var list = new GlobalConfigProvider().GetTimeSubsidies(0); 
            //移除某一时间配置
            var newlist = (from globalConfigTimeSubsidiese in list
                           where globalConfigTimeSubsidiese.Id!=id
                           select globalConfigTimeSubsidiese).ToList();
            string values = GetTimesValues(newlist);
            bool b = new GlobalConfigProvider().UpdateTimeSubsidies(UserContext.Current.Name, values, "删除时间补贴设置-设置之后的值:" + values, 0, -1);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }
          
        /// <summary>
        /// 修改时间补贴设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateTimeSubsidies(GlobalConfigSubsidies model)
        {
            //读取全部时间配置
            var list = new GlobalConfigProvider().GetTimeSubsidies(0);

            var mm = list.FirstOrDefault(subsidies => subsidies.Id == model.Id);
            if (mm != null)
            {
                mm.Value1 = model.Value1;
                mm.Value2 = model.Value2;
            }
            var newlist = (from globalConfigTimeSubsidiese in list
                           orderby ParseHelper.ToInt(globalConfigTimeSubsidiese.Value1) ascending
                           select globalConfigTimeSubsidiese).ToList();
            string values = GetTimesValues(newlist);
            bool b = new GlobalConfigProvider().UpdateTimeSubsidies(UserContext.Current.Name, values, "修改时间补贴设置-设置之后的值:" + values, 0, -1);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取时间补贴字符串
        /// </summary>
        /// <param name="newlist"></param>
        /// <returns></returns>
        private string GetTimesValues(IEnumerable<GlobalConfigSubsidies> newlist)
        {
            string values = "";
            int i = 1;
            foreach (var globalConfigTimeSubsidiese in newlist)
            {
                string aa = globalConfigTimeSubsidiese.Value1 + "," + globalConfigTimeSubsidiese.Value2 + "," + i;
                values += aa + ";";
                i++;
            }
            return values;
        }

    }
}