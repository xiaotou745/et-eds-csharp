using SuperManBusinessLogic.Order_Logic;
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
        [HttpPost]
        public JsonResult SaveOrderInfo(order model)
        {
            bool reg = OrderLogic.orderLogic().UpdateOrderInfo(model);
            return Json(new ResultModel(reg, string.Empty), JsonRequestBehavior.AllowGet);
        }
    }
}
