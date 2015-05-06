using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.Provider.Common;
using ETS.Util;
using SuperMan.App_Start;
using Ets.Service.IProvider.Subsidy;
using Ets.Service.Provider.Subsidy;
namespace SuperMan.Controllers
{
    public class SubsidyFormulaModeController : BaseController
    {
        // GET: SubsidyFormulaMode
        public ActionResult SubsidyFormulaMode()
        {
            var listprice = new GlobalConfigProvider().GetPriceSubsidies(0);
            ViewBag.GloglConfig = new GlobalConfigProvider().GlobalConfigMethod(0);
            return View(listprice);
        }
		[HttpGet]
        public ActionResult SubsidyFormulaMode(int GroupId = 0, int StrategyId = 0, string GroupName = "")
        {
            var listprice = new GlobalConfigProvider().GetPriceSubsidies(GroupId);
            ViewBag.GroupId = GroupId;
            ViewBag.StrategyId = StrategyId;
            ViewBag.GroupName = GroupName;
            ViewBag.GloglConfig = new GlobalConfigProvider().GlobalConfigMethod(GroupId);
            return View(listprice);
        }

        #region 金额补贴设置

        /// <summary>
        /// 添加金额补贴设置
        /// 胡灵波 添加分组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddPriceSubsidies(GlobalConfigSubsidies model)
        {
            var list = new GlobalConfigProvider().GetPriceSubsidies(0);
            list.Add(model);
            var newlist = (from globalConfigTimeSubsidiese in list
                           orderby ParseHelper.ToInt(globalConfigTimeSubsidiese.Value1) ascending
                           select globalConfigTimeSubsidiese).ToList();
            string values = GetTimesValues(newlist);
            bool b = new GlobalConfigProvider().UpdatePriceSubsidies(UserContext.Current.Name, values, "添加金额补贴设置操作-设置之后的值:" + values,0,-1);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除金额补贴设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DeletePriceSubsidies(int id)
        {
            var list = new GlobalConfigProvider().GetPriceSubsidies(0);
            var newlist = (from globalConfigTimeSubsidiese in list
                           where globalConfigTimeSubsidiese.Id != id
                           select globalConfigTimeSubsidiese).ToList();
            string values = GetTimesValues(newlist);
            bool b = new GlobalConfigProvider().UpdatePriceSubsidies(UserContext.Current.Name, values, "删除金额补贴设置-设置之后的值:" + values,0,-1);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改金额补贴设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePriceSubsidies(GlobalConfigSubsidies model)
        {
            var list = new GlobalConfigProvider().GetPriceSubsidies(0);

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
            bool b = new GlobalConfigProvider().UpdatePriceSubsidies(UserContext.Current.Name, values, "修改金额补贴设置-设置之后的值:" + values,0,-1);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        } 
        #endregion

        /// <summary>
        /// 修改佣金补贴策略
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetCommissionFormulaMode(string value)
        {
            bool b = new GlobalConfigProvider().UpdateCommissionFormulaMode(UserContext.Current.Name, value, "修改佣金补贴策略-设置之后的值:" + value,0,-1);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 是否开启动态时间补贴(0不开启,1开启)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetIsStarTimeSubsidies(string IsStarTimeSubsidies)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, IsStarTimeSubsidies, "是否开启动态时间补贴(0不开启,1开启)-设置之后的值:" + IsStarTimeSubsidies, "IsStarTimeSubsidies",0,-1);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }


     

        #region 设置4大佣金补贴策略 

        /// <summary>
        /// 设置普通补贴佣金比例和网站补贴
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult setCommonCommissionRatio(string value, string value1)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value, "修改普通补贴佣金比例-设置之后的值:" + value, "CommonCommissionRatio",0,-1);
            bool b1 = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value1, "修改普通网站补贴-设置之后的值:" + value, "CommonSiteSubsidies",0,-1);
            return Json(new ResultModel(b && b1, string.Empty), JsonRequestBehavior.AllowGet);
        }  
         
        /// <summary>
        /// 设置时间段补贴佣金比例和网站补贴
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult setTimeSpanCommissionRatio(string value, string value1,string value2)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value, "修改时间段佣金比例-设置之后的值:" + value, "TimeSpanCommissionRatio", 0, -1);
            bool b1 = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value1, "修改时间段之内补贴价钱-设置之后的值:" + value, "TimeSpanInPrice", 0, -1);
            bool b2 = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value2, "修改时间段之外补贴价钱 -设置之后的值:" + value, "TimeSpanOutPrice", 0, -1);
            return Json(new ResultModel(b && b1 && b2, string.Empty), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 设置保本补贴佣金比例和网站补贴
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult setBaoBenCommissionRatio(string value, string value1)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value, "修改保本利润比例-设置之后的值:" + value, "CommissionRatio", 0, -1);
            bool b1 = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value1, "修改保本网站补贴-设置之后的值:" + value, "SiteSubsidies", 0, -1);
            return Json(new ResultModel(b && b1, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置 满足金额补贴利润比例和网站补贴
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetPriceCommissionRatio(string value, string value1)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value, "修改满足金额补贴利润比例-设置之后的值:" + value, "PriceCommissionRatio", 0, -1);
            bool b1 = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value1, "修改满足金额网站补贴-设置之后的值:" + value, "PriceSiteSubsidies", 0, -1);
            return Json(new ResultModel(b && b1, string.Empty), JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// 获取补贴字符串
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


        #region 跨店抢单设置

        /// <summary>
        /// 跨店抢单
        /// </summary>
        /// <returns></returns>
        public ActionResult OverStoreSubsidies()
        {
            var listprice = new GlobalConfigProvider().GetOverStoreSubsidies();
            return View(listprice);
        }

        /// <summary>
        /// 是否开启跨店抢单补贴(0不开启,1开启)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetIsStartOverStoreSubsidies(string IsStartOverStoreSubsidies)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, IsStartOverStoreSubsidies, "是否开启跨店抢单补贴(0不开启,1开启)-设置之后的值:" + IsStartOverStoreSubsidies, "IsStartOverStoreSubsidies", 0, -1);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }

        
        /// <summary>
        /// 添加跨店抢单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOverStoreSubsidies(GlobalConfigSubsidies model)
        {
            var list = new GlobalConfigProvider().GetOverStoreSubsidies();
            if (list.Exists(m => m.Value1 == model.Value1))
            {
                return Json(new ResultModel(false, "跨店数量不能重复"), JsonRequestBehavior.AllowGet);
            }
            list.Add(model);
            var newlist = (from globalConfigTimeSubsidiese in list
                           orderby ParseHelper.ToDouble(globalConfigTimeSubsidiese.Value1) ascending 
                select globalConfigTimeSubsidiese).ToList();
            string values = GetTimesValues(newlist);
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, values, "添加跨店抢单-设置之后的值:" + values, "OverStoreSubsidies", 0, -1);
            return Json(new ResultModel(b, b==true?string.Empty:"添加失败!"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除跨店抢单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DeleteOverStoreSubsidies(int id)
        {
            //读取全部配置
            var list = new GlobalConfigProvider().GetOverStoreSubsidies();
            //移除某一配置
            var newlist = (from globalConfigTimeSubsidiese in list
                           where globalConfigTimeSubsidiese.Id != id
                           select globalConfigTimeSubsidiese).ToList();
            string values = GetTimesValues(newlist);
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, values, "删除跨店抢单-设置之后的值:" + values, "OverStoreSubsidies", 0, -1);
            return Json(new ResultModel(b, b == true ? string.Empty : "删除失败!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改跨店抢单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateOverStoreSubsidies(GlobalConfigSubsidies model)
        {
            //读取全部时间配置
            var list = new GlobalConfigProvider().GetOverStoreSubsidies();
            if (list.Exists(m => m.Id!=model.Id &&  m.Value1 == model.Value1))
            {
                return Json(new ResultModel(false, "跨店不能重复"), JsonRequestBehavior.AllowGet);
            }
            var mm = list.FirstOrDefault(subsidies => subsidies.Id == model.Id);
            if (mm != null &&!(mm.Value1 == model.Value1 && mm.Value2 == model.Value2))
            {
                mm.Value1 = model.Value1;
                mm.Value2 = model.Value2;
                var newlist = (from globalConfigTimeSubsidiese in list
                               orderby ParseHelper.ToDouble(globalConfigTimeSubsidiese.Value1) ascending
                               select globalConfigTimeSubsidiese).ToList();
                string values = GetTimesValues(newlist);
                bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, values, "修改跨店抢单-设置之后的值:" + values, "OverStoreSubsidies", 0, -1);
                return Json(new ResultModel(b, b == true ? string.Empty : "修改失败!"), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel(true,string.Empty), JsonRequestBehavior.AllowGet); 
        }

        
        #endregion 
       
    }
}