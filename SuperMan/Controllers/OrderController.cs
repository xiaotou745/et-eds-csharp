using SuperManBusinessLogic.Order_Logic;
using SuperManCommonModel.Entities;
using SuperManCommonModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperManCore;
using SuperManCore.Paging;

namespace SuperMan.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        //Get: /Order  订单管理
        public ActionResult Order()
        {
            var criteria = new OrderSearchCriteria() { orderStatus = -1, PagingRequest = new PagingResult(0, 15) };
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
    }
}
