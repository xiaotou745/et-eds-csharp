
﻿using SuperManBusinessLogic.C_Logic;
using SuperManBusinessLogic.Order_Logic;
﻿using SuperManBusinessLogic.Order_Logic;
using SuperManBusinessLogic.Subsidy_Logic;
using SuperManCommonModel.Entities;
using SuperManCommonModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperManCore;
using SuperManCore.Common;
using SuperManCore.Paging;
using SuperManDataAccess;

namespace SuperMan.Controllers
{
    [Authorize]
    [WebHandleError]
    public class OrderController : Controller
    {
        //Get: /Order  订单管理
        public ActionResult Order()
        {
            account account = HttpContext.Session["user"] as account;
            if (account == null)
            {
                Response.Redirect("/account/login");
                return null;
            }
            ViewBag.txtGroupId = account.GroupId;//集团id

            var superManModel = SuperManBusinessLogic.C_Logic.ClienterLogic.clienterLogic().GetClienterModelByGroupID(ViewBag.txtGroupId);
            if (superManModel != null)
            {
                ViewBag.superManModel = superManModel;
            } 
            var criteria = new OrderSearchCriteria() { orderStatus = -1, PagingRequest = new PagingResult(0, 15), GroupId = account.GroupId };
            var pagedList = OrderLogic.orderLogic().GetOrders(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult OrderList(OrderSearchCriteria criteria)
        {
            var pagedList = OrderLogic.orderLogic().GetOrders(criteria);
            var item = pagedList.orderManageList; 
            return PartialView("_PartialOrderList",item);
        }

        //Get: /OrderCount  订单统计
        public ActionResult OrderCount()
        {
            return View();

        }   
        /// <summary>
        /// 超人抢单--平扬 2015.3.2
        /// </summary>
        /// <param name="SuperID">超人id</param>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult RushOrder(int SuperID, string OrderNo)
        {
            if (SuperID == 0) //超人id验证
                return Json(new ResultModel(false, "超人不能为空"), JsonRequestBehavior.AllowGet);
            if (string.IsNullOrEmpty(OrderNo)) //订单号码非空验证
                return Json(new ResultModel(false, "订单不能为空"), JsonRequestBehavior.AllowGet);
            if (ClienterLogic.clienterLogic().GetOrderByNo(OrderNo) == null) //查询订单是否存在
                return Json(new ResultModel(false, "订单不存在"), JsonRequestBehavior.AllowGet);
            if (!ClienterLogic.clienterLogic().CheckOrderIsAllowRush(OrderNo))  //查询订单是否被抢
                return Json(new ResultModel(false, "订单已被抢或者已完成"), JsonRequestBehavior.AllowGet);
            var bResult = ClienterLogic.clienterLogic().RushOrder(SuperID, OrderNo);
            return Json(bResult ? new ResultModel(true, "抢单成功") : new ResultModel(false, "抢单失败"), JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult SaveOrderInfo(order model)
        {
            bool reg = OrderLogic.orderLogic().UpdateOrderInfo(model);
            return Json(new ResultModel(reg, string.Empty), JsonRequestBehavior.AllowGet);
        }
    }
}
