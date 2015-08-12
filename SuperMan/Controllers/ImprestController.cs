using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETS.Data.PageData;
using Ets.Model.DataModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Finance;
using NPOI.SS.Formula.Functions;

namespace SuperMan.Controllers
{
    /// <summary>
    /// 备用金controller 
    /// </summary>
    public class ImprestController : Controller
    {
        private readonly IImprestBalanceRecordProvider imprestProvider = new ImprestBalanceRecordProvider();
        /// <summary>
        /// 备用金充值列表页  add by caoheyang  20150812
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImprestRechargeList()
        {
            ViewBag.ImprestRecharge = new ImprestRecharge();
            ViewBag.PageModels = new PageInfo<ImprestBalanceRecord>(0, 1, new List<ImprestBalanceRecord>(), 100, 15); 
            return View();
        }

        /// <summary>
        /// 备用金充值列表页  异步加载区域 add by caoheyang  20150812
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoImprestRechargeList(int pageindex = 1)
        {
            PageInfo<ImprestBalanceRecord> models = new PageInfo<ImprestBalanceRecord>(0, 1, new List<ImprestBalanceRecord>(), 100, 15);
            return PartialView();
        }

        /// <summary>
        /// 备用金充值功能 add by caoheyang  20150812
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImprestRecharge()
        {
            return PartialView();
        }

        /// <summary>
        /// 备用金充值功能 保存按钮  add by caoheyang  20150812
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AjaxImprestRecharge()
        {
            return PartialView();
        }

        /// <summary>
        /// 备用金支出列表页 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImprestPaymentList()
        {
            return View();
        }

        /// <summary>
        /// 备用金支出列表页     异步加载区域
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult DoImprestPaymentList()
        {
            return PartialView();
        }

        /// <summary>
        /// 支出备用金验证骑士手机号获取信息
        /// 2015年8月12日16:55:56
        /// 茹化肖
        /// </summary>
        /// <returns></returns>
         [HttpPost]
        public JsonResult CheckPhoneNum()
        {
            string phonenum = System.Web.HttpContext.Current.Request["PhoneNum"];
            ImprestClienterModel model = imprestProvider.ClienterPhoneCheck(phonenum);
            return Json(model);
        }

    }
}