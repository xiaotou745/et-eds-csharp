
﻿using System.Collections.Generic;
﻿using System.Text;
﻿using System.Web.Mvc;
﻿using ETS.Data.PageData;
﻿using Ets.Model.DataModel.Order;
﻿using Ets.Model.DomainModel.Bussiness;
﻿using SuperManCore.Common;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.Order;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using SuperMan.App_Start;
using Ets.Model.ParameterModel.User;
using Ets.Model.ParameterModel.Order;

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
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity();
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
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId; ;//集团id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity();
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

        /// <summary>
        /// 导出订单数据
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PostDaoChuOrder(int pageindex = 1)
        {  
            Ets.Model.ParameterModel.Order.OrderSearchCriteria criteria = new Ets.Model.ParameterModel.Order.OrderSearchCriteria();
            TryUpdateModel(criteria);
            criteria.PageIndex = 1;
            criteria.PageSize = 65534;
            if (criteria.businessCity=="--无--")
            {
                criteria.businessCity = "";
            }
 
            var pagedList = iOrderProvider.GetOrders(criteria);

            if (pagedList != null && pagedList.Records.Count > 0)
            {
                string filname = "e代送-{0}-订单数据.xls";
                if (!string.IsNullOrWhiteSpace(criteria.businessName))
                {
                    filname = string.Format(filname, criteria.businessName);
                }
                if (!string.IsNullOrWhiteSpace(criteria.superManName))
                {
                    filname = string.Format(filname, criteria.superManName);
                }
                if (!string.IsNullOrWhiteSpace(criteria.orderPubStart))
                {
                    filname = string.Format(filname, criteria.orderPubStart+":"+criteria.orderPubEnd);
                } 
                if (pagedList.Records.Count > 3)
                {
                    byte[] data = Encoding.UTF8.GetBytes(CreateExcel(pagedList));
                    return File(data, "application/ms-excel", filname);
                }
                else
                {
                    byte[] data = Encoding.Default.GetBytes(CreateExcel(pagedList));
                    return File(data, "application/ms-excel", filname);
                }

            }
            return PartialView("_PartialOrderList", pagedList);
        }


        /// <summary>
        /// 生成excel文件
        /// 导出字段：订单号、商户信息、发布时间、完成时间、订单数量、订单总金额、订单佣金、外送费用、每单补贴、任务补贴、商家结算比例
        /// </summary>
        /// <returns></returns>
        private string CreateExcel(PageInfo<OrderListModel> paraModel)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
            //输出表头.
            strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            strBuilder.AppendLine("<td>订单号</td>");
            strBuilder.AppendLine("<td>商户信息</td>");
            strBuilder.AppendLine("<td>骑士信息</td>");
            strBuilder.AppendLine("<td>发布时间</td>");
            strBuilder.AppendLine("<td>完成时间</td>"); 
            strBuilder.AppendLine("<td>订单金额</td>"); 
            strBuilder.AppendLine("<td>订单总金额</td>");
            strBuilder.AppendLine("<td>订单佣金</td>");
            strBuilder.AppendLine("<td>订单数量</td>");
            strBuilder.AppendLine("<td>外送费用</td>");
            strBuilder.AppendLine("<td>每单补贴</td>");
            strBuilder.AppendLine("<td>任务补贴</td>");
            strBuilder.AppendLine("<td>商家结算比例(%)</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            foreach (var oOrderListModel in paraModel.Records)
            {
                strBuilder.AppendLine(string.Format("<tr><td>'{0}'</td>", oOrderListModel.OrderNo));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.BusinessName+":"+oOrderListModel.BusinessPhoneNo));
                string clineter = "";
                if (!string.IsNullOrEmpty(oOrderListModel.ClienterName))
                {
                    clineter = oOrderListModel.ClienterName;
                }
                if (!string.IsNullOrEmpty(oOrderListModel.ClienterPhoneNo))
                {
                    clineter +=":"+ oOrderListModel.ClienterPhoneNo;
                }
                strBuilder.AppendLine(string.Format("<td>{0}</td>", clineter));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.PubDate));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.ActualDoneDate)); 
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.Amount));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.Amount + oOrderListModel.OrderCount * oOrderListModel.DistribSubsidy));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.OrderCommission));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.OrderCount));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.DistribSubsidy));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.WebsiteSubsidy));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.Adjustment));
                strBuilder.AppendLine(string.Format("<td>{0}</td></tr>", oOrderListModel.BusinessCommission));
            }
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
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
        /// <summary>
        /// 查看订单明细
        /// danny-20150414
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(string orderNo,string orderId)
        {
            var orderModel = iOrderProvider.GetOrderByNo(orderNo);
            ViewBag.orderOptionLog = iOrderProvider.GetOrderOptionLog(orderId);
            return View(orderModel);
        }

        /// <summary>
        /// 王旭丹
        /// 2015年4月24日 11:32:55
        /// 取消订单
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="OrderOptionLog"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelOrder(string OrderNo, string OrderOptionLog)
        {
            OrderOptionModel orderOptionModel = new OrderOptionModel()
            {
                OptUserId = UserContext.Current.Id,
                OptUserName = UserContext.Current.Name,
                OptLog=OrderOptionLog,
                OrderNo = OrderNo
            };
            bool reg = iOrderProvider.CancelOrderByOrderNo(orderOptionModel);
            return Json(new ResultModel(reg, string.Empty), JsonRequestBehavior.AllowGet);
        }
    }
}
