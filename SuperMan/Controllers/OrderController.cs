
﻿using System.Web.Mvc;
using SuperManCore.Common;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.Order;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;

namespace SuperMan.Controllers
{
    [WebHandleError]
    public class OrderController : BaseController
    {
        Ets.Service.IProvider.Distribution.IDistributionProvider iDistributionProvider = new DistributionProvider();
        Ets.Service.IProvider.Order.IOrderProvider iOrderProvider = new OrderProvider();
        IAreaProvider iAreaProvider = new AreaProvider();
        //Get: /Order  订单管理
        public ActionResult Order()
        {
            //SuperManDataAccess.account account = HttpContext.Session["user"] as SuperManDataAccess.account;
            //if (account == null)
            //{
            //    Response.Redirect("/account/login");
            //    return null;
            //}
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id
            ViewBag.openCityList = iAreaProvider.GetOpenCityInfo();
            var superManModel = iDistributionProvider.GetClienterModelByGroupID(ViewBag.txtGroupId);
            if (superManModel != null)
            {
                ViewBag.superManModel = superManModel;
            }
            var criteria = new Ets.Model.ParameterModel.Order.OrderSearchCriteria() { orderStatus = -1, GroupId = SuperMan.App_Start.UserContext.Current.GroupId };
            var pagedList = iOrderProvider.GetOrders(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult PostOrder(int pageindex = 1)
        {
            //SuperManDataAccess.account account = HttpContext.Session["user"] as SuperManDataAccess.account;
            //if (account == null)
            //{
            //    Response.Redirect("/account/login");
            //    return null;
            //}
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId; ;//集团id
            ViewBag.openCityList = iAreaProvider.GetOpenCityInfo();
            Ets.Model.ParameterModel.Order.OrderSearchCriteria criteria = new Ets.Model.ParameterModel.Order.OrderSearchCriteria();
            TryUpdateModel(criteria);
            //指派超人时  以下代码 有用，现在 注释掉  wc 
            //var superManModel = iDistributionProvider.GetClienterModelByGroupID(ViewBag.txtGroupId);
            //if (superManModel != null)
            //{
            //    ViewBag.superManModel = superManModel;
            //} 

            var pagedList = iOrderProvider.GetOrders(criteria);
            return PartialView("_PartialOrderList", pagedList);
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
            //if (SuperID == -1) //超人id验证
            //    return Json(new ResultModel(false, "超人不能为空"), JsonRequestBehavior.AllowGet);

            if (string.IsNullOrEmpty(OrderNo)) //订单号码非空验证
                return Json(new ResultModel(false, "订单不能为空"), JsonRequestBehavior.AllowGet);
            var order = iOrderProvider.GetOrderByNo(OrderNo);
            if (order == null) //查询订单是否存在
                return Json(new ResultModel(false, "订单不存在"), JsonRequestBehavior.AllowGet);
            if (order.Status !=Ets.Model.Common.ConstValues.ORDER_NEW)  //查询订单是否被抢
                return Json(new ResultModel(false, "订单已被抢或者已完成"), JsonRequestBehavior.AllowGet);
            if (SuperID == -1) //未指派超人 ，触发极光推送  ，指派超人的情况下，建立订单和超人的关系
            {
               Ets.Service.Provider.MyPush.Push.PushMessage(0, "有新订单了！", "有新的订单可以抢了！", "有新的订单可以抢了！", string.Empty, order.BusinessCity); // 极光推送
                return Json(new ResultModel(true, "有新订单可抢"), JsonRequestBehavior.AllowGet);
            }
            order.clienterId = SuperID;
            var bResult = iOrderProvider.RushOrder(order);
            return Json(bResult ? new ResultModel(true, "抢单成功") : new ResultModel(false, "抢单失败"), JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult SaveOrderInfo(Ets.Model.DataModel.Order.order model)
        {
            bool reg = iOrderProvider.UpdateOrderInfo(model);
            return Json(new ResultModel(reg, string.Empty), JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderDetail(string orderNo)
        {
            var orderModel = iOrderProvider.GetOrderByNo(orderNo);
            return View(orderModel);
        }
    }
}
