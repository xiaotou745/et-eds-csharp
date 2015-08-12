using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETS.Data.PageData;
using Ets.Model.DataModel.Finance;

namespace SuperMan.Controllers
{
    /// <summary>
    /// 备用金controller 
    /// </summary>
    public class ImprestController : Controller
    {
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
        public JsonResult ImprestRecharge()
        {
            return new JsonResult();
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

    }
}