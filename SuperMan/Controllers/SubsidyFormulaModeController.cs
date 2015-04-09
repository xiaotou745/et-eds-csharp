﻿using System;
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
            var listprice = new GlobalConfigProvider().GetPriceSubsidies();
            ViewBag.GloglConfig = new GlobalConfigProvider().GlobalConfigMethod();
            return View(listprice);
        }

        #region 金额补贴设置

        /// <summary>
        /// 添加金额补贴设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddPriceSubsidies(GlobalConfigSubsidies model)
        {
            var list = new GlobalConfigProvider().GetPriceSubsidies();
            list.Add(model);
            var newlist = (from globalConfigTimeSubsidiese in list
                           orderby ParseHelper.ToInt(globalConfigTimeSubsidiese.Value1) ascending
                           select globalConfigTimeSubsidiese).ToList();
            string values = GetTimesValues(newlist);
            bool b = new GlobalConfigProvider().UpdatePriceSubsidies(UserContext.Current.Name, values, "添加金额补贴设置操作-设置之后的值:" + values);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除金额补贴设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DeletePriceSubsidies(int id)
        {
            var list = new GlobalConfigProvider().GetPriceSubsidies();
            var newlist = (from globalConfigTimeSubsidiese in list
                           where globalConfigTimeSubsidiese.Id != id
                           select globalConfigTimeSubsidiese).ToList();
            string values = GetTimesValues(newlist);
            bool b = new GlobalConfigProvider().UpdatePriceSubsidies(UserContext.Current.Name, values, "删除金额补贴设置-设置之后的值:" + values);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改金额补贴设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePriceSubsidies(GlobalConfigSubsidies model)
        {
            var list = new GlobalConfigProvider().GetPriceSubsidies();

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
            bool b = new GlobalConfigProvider().UpdatePriceSubsidies(UserContext.Current.Name, values, "修改金额补贴设置-设置之后的值:" + values);
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
            bool b = new GlobalConfigProvider().UpdateCommissionFormulaMode(UserContext.Current.Name, value, "修改佣金补贴策略-设置之后的值:" + value);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 是否开启动态时间补贴(0不开启,1开启)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetIsStarTimeSubsidies(string IsStarTimeSubsidies)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, IsStarTimeSubsidies, "是否开启动态时间补贴(0不开启,1开启)-设置之后的值:" + IsStarTimeSubsidies, "IsStarTimeSubsidies");
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        } 
          
        /// <summary>
        /// 设置普通补贴佣金比例和网站补贴
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult setCommonCommissionRatio(string value, string value1)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value, "修改普通补贴佣金比例-设置之后的值:" + value, "CommonCommissionRatio");
            bool b1 = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value1, "修改普通网站补贴-设置之后的值:" + value, "CommonSiteSubsidies");
            return Json(new ResultModel(b && b1, string.Empty), JsonRequestBehavior.AllowGet);
        }  
         
        /// <summary>
        /// 设置时间段补贴佣金比例和网站补贴
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult setTimeSpanCommissionRatio(string value, string value1,string value2)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value, "修改时间段佣金比例-设置之后的值:" + value, "TimeSpanCommissionRatio");
            bool b1 = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value1, "修改时间段之内补贴价钱-设置之后的值:" + value, "TimeSpanInPrice");
            bool b2 = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value2, "修改时间段之外补贴价钱 -设置之后的值:" + value, "TimeSpanOutPrice");
            return Json(new ResultModel(b && b1 && b2, string.Empty), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 设置保本补贴佣金比例和网站补贴
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult setBaoBenCommissionRatio(string value, string value1)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value, "修改保本利润比例-设置之后的值:" + value, "CommissionRatio");
            bool b1 = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value1, "修改保本网站补贴-设置之后的值:" + value, "SiteSubsidies");
            return Json(new ResultModel(b && b1, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置 满足金额补贴利润比例和网站补贴
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetPriceCommissionRatio(string value, string value1)
        {
            bool b = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value, "修改满足金额补贴利润比例-设置之后的值:" + value, "PriceCommissionRatio");
            bool b1 = new GlobalConfigProvider().UpdateSubsidies(UserContext.Current.Name, value1, "修改满足金额网站补贴-设置之后的值:" + value, "PriceSiteSubsidies");
            return Json(new ResultModel(b && b1, string.Empty), JsonRequestBehavior.AllowGet);
        }

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
    }
}