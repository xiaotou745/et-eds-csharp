
﻿using System.Collections.Generic;
﻿using System.Linq;
﻿using System.Text;
﻿using System.Threading.Tasks;
﻿using System.Web.Mvc;
﻿using ETS.Data.PageData;
﻿using Ets.Model.DataModel.Order;
﻿using Ets.Model.DomainModel.Business;
﻿using Ets.Service.IProvider.AuthorityMenu;
﻿using Ets.Service.Provider.Authority;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.Order;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
﻿using SuperMan.App_Start;
using Ets.Model.ParameterModel.User;
using Ets.Model.ParameterModel.Order;
using ETS.Util;
using Ets.Model.Common;
﻿using Ets.Model.DomainModel.Order;
﻿using Ets.Service.Provider.Clienter;
using ETS.Enums;
using Ets.Service.Provider.Business;
using Ets.Service.IProvider.Business;
﻿using ETS.Const;
﻿using SuperMan.Common;

using Ets.Model.DomainModel.Area;namespace SuperMan.Controllers
{
    public class OrderController : BaseController
    {
        Ets.Service.IProvider.Distribution.IDistributionProvider iDistributionProvider = new DistributionProvider();
        Ets.Service.IProvider.Order.IOrderProvider iOrderProvider = new OrderProvider();
        IAreaProvider iAreaProvider = new AreaProvider();
        IAuthorityMenuProvider iAuthorityMenuProvider = new AuthorityMenuProvider();
        IBusinessProvider iBusinessProvider = new BusinessProvider();
        //Get: /Order  订单管理
        public ActionResult Order()
        {
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id
            ViewBag.GroupList = iBusinessProvider.GetGroups();

            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var superManModel = iDistributionProvider.GetClienterModelByGroupID(ViewBag.txtGroupId);
            if (superManModel != null)
            {
                ViewBag.superManModel = superManModel;
            }
            var criteria = new OrderSearchCriteria()
            {
                orderStatus = -1,
                UserType = UserType,
                //GroupId = UserContext.Current.GroupId,
                AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType)
            };
            if (!string.IsNullOrEmpty(Request.QueryString["phoneNo"]))
            {
                criteria.orderPubStart = Request.QueryString["startDate"];
                criteria.orderPubEnd = Request.QueryString["endDate"];

                ViewBag.orderPubStart = Request.QueryString["startDate"];
                ViewBag.orderPubEnd = Request.QueryString["endDate"];
                if (Request.QueryString["userType"] == "0")
                {
                    criteria.superManPhone = Request.QueryString["phoneNo"];
                    criteria.superManName = Request.QueryString["userName"];
                    ViewBag.superManPhone = Request.QueryString["phoneNo"];
                    ViewBag.superManName = Request.QueryString["userName"];
                }
                else
                {
                    criteria.businessPhone = Request.QueryString["phoneNo"];
                    criteria.businessName = Request.QueryString["userName"];
                    ViewBag.businessPhone = Request.QueryString["phoneNo"];
                    ViewBag.businessName = Request.QueryString["userName"];
                }
            }

            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return View();
            }
            var pagedList = iOrderProvider.GetOrders(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult PostOrder(int pageindex = 1)
        {
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId; ;//集团id
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var criteria = new OrderSearchCriteria();
            TryUpdateModel(criteria);
            criteria.AuthorityCityNameListStr =
                iAreaProvider.GetAuthorityCityNameListStr(UserType);
            criteria.UserType = UserType;
            //指派超人时  以下代码 有用，现在 注释掉  wc 
            //var superManModel = iDistributionProvider.GetClienterModelByGroupID(ViewBag.txtGroupId);
            //if (superManModel != null)
            //{
            //    ViewBag.superManModel = superManModel;
            //} 
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return PartialView("_PartialOrderList");
            }
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
            OrderSearchCriteria criteria = new OrderSearchCriteria();
            TryUpdateModel(criteria);
            criteria.PageIndex = 1;
            criteria.PageSize = 65534;
            if (criteria.businessCity == "--无--")
            {
                criteria.businessCity = "";
            } 
            var pagedList = iOrderProvider.GetOrders(criteria);
            if (pagedList != null && pagedList.Records.Count > 0)
            {
                string[] title = ExcelUtility.GetDescription(new OrderExcel());
                string filname = "e代送-{0}-订单数据";
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
                    filname = string.Format(filname, ParseHelper.ToDatetime(criteria.orderPubStart).ToString("yyyy-MM-dd") + "到" + ParseHelper.ToDatetime(criteria.orderPubEnd).ToString("yyyy-MM-dd"));
                }
                ExcelIO.CreateFactory().Export(ConvertOrderToOrderExcel(pagedList), ExportFileFormat.excel, filname, title);
                return null;
            }
            Response.Write(SystemConst.NoExportData); 
            return null;
        }
        /// <summary>
        /// 转换为导出的Excel所需Model
        /// wc
        /// </summary>
        /// <param name="paraModel"></param>
        /// <returns></returns>
        private IList<OrderExcel> ConvertOrderToOrderExcel(PageInfo<OrderListModel> paraModel)
        {
            var orderExcels = new List<OrderExcel>();
            foreach (var oOrderListModel in paraModel.Records)
            {
                var clienterInfo = "";
                var orderExcel = new OrderExcel();
                orderExcel.OrderNo = oOrderListModel.OrderNo;
                if (!string.IsNullOrEmpty(oOrderListModel.ClienterName))
                {
                    clienterInfo = oOrderListModel.ClienterName;
                }
                if (!string.IsNullOrEmpty(oOrderListModel.ClienterPhoneNo))
                {
                    clienterInfo += ":" + oOrderListModel.ClienterPhoneNo;
                }
                orderExcel.BusinessInfo = oOrderListModel.BusinessName + ":" + oOrderListModel.BusinessPhoneNo;
                orderExcel.ClienterInfo = clienterInfo;
                orderExcel.PubDate = oOrderListModel.PubDate;
                orderExcel.ActualDoneDate = oOrderListModel.ActualDoneDate;
                orderExcel.Amount = oOrderListModel.Amount.Value;
                orderExcel.TotalAmount = oOrderListModel.Amount.Value + oOrderListModel.OrderCount * oOrderListModel.DistribSubsidy.Value;
                orderExcel.OrderCommission = oOrderListModel.OrderCommission.Value;
                orderExcel.OrderCount = oOrderListModel.OrderCount;
                orderExcel.DistribSubsidy = oOrderListModel.DistribSubsidy.Value;
                orderExcel.WebsiteSubsidy = oOrderListModel.WebsiteSubsidy.Value;
                orderExcel.Adjustment = oOrderListModel.Adjustment;
                orderExcel.BusiSettlement = oOrderListModel.CommissionType == 1
                    ? oOrderListModel.BusinessCommission + "%"
                    : oOrderListModel.CommissionFixValue.ToString();

                orderExcels.Add(orderExcel);
            }
            return orderExcels;
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
            strBuilder.AppendLine("<td>商家结算</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            foreach (var oOrderListModel in paraModel.Records)
            {
                strBuilder.AppendLine(string.Format("<tr><td>'{0}'</td>", oOrderListModel.OrderNo));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.BusinessName + ":" + oOrderListModel.BusinessPhoneNo));
                string clineter = "";
                if (!string.IsNullOrEmpty(oOrderListModel.ClienterName))
                {
                    clineter = oOrderListModel.ClienterName;
                }
                if (!string.IsNullOrEmpty(oOrderListModel.ClienterPhoneNo))
                {
                    clineter += ":" + oOrderListModel.ClienterPhoneNo;
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
                strBuilder.AppendLine(string.Format("<td>{0}</td></tr>", oOrderListModel.CommissionType == 1 ? oOrderListModel.BusinessCommission + "%" : oOrderListModel.CommissionFixValue.ToString()));
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
            if (order.Status != OrderStatus.Status0.GetHashCode())  //查询订单是否被抢
                return Json(new ResultModel(false, "订单已被抢或者已完成"), JsonRequestBehavior.AllowGet);
            if (SuperID == -1) //未指派超人 ，触发极光推送  ，指派超人的情况下，建立订单和超人的关系
            {
                //异步回调第三方，推送通知
                Task.Factory.StartNew(() =>
                {
                    Ets.Service.Provider.MyPush.Push.PushMessage(0, "有新订单了！", "有新的订单可以抢了！", "有新的订单可以抢了！", string.Empty,
                        order.BusinessCity); // 极光推送
                });
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
        public ActionResult OrderDetail(string orderNo, int orderId)
        {
            var orderModel = iOrderProvider.GetOrderByNo(orderNo, orderId);
            ViewBag.orderOptionLog = iOrderProvider.GetOrderOptionLog(orderId);
            ViewBag.IsShowAuditBtn = IsShowAuditBtn(orderModel);//是否显示审核按钮
            return View(orderModel);
        }

        /// <summary>
        /// 王旭丹
        /// 2015年4月24日 11:32:55
        /// 取消订单
        /// 该方法在订单超时列表页也调用
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="OrderOptionLog"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelOrder(int orderId, string OrderOptionLog)
        {
            OrderOptionModel orderOptionModel = new OrderOptionModel()
            {
                OptUserId = UserContext.Current.Id,
                OptUserName = UserContext.Current.Name,
                OptLog = "客服取消" + OrderOptionLog,
                OrderId = orderId
            };
            var reg = iOrderProvider.CancelOrderByOrderNo(orderOptionModel);
            return Json(new ResultModel(reg.DealFlag, reg.DealMsg), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查看订单地图
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public JsonResult OrderMap(long OrderId)
        {
            OrderMapDetail mapDetail = iOrderProvider.GetOrderMapDetail(OrderId);
            return Json(mapDetail);
        }

        /// <summary>
        /// 审核拒绝
        /// 彭宜
        /// 修改人：胡灵波
        /// 2015年8月3日 11:32:55        
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="OrderOptionLog"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuditRefuse(int orderId, string OrderOptionLog)
        {
            OrderOptionModel orderOptionModel = new OrderOptionModel()
            {
                OptUserId = UserContext.Current.Id,
                OptUserName = UserContext.Current.Name,
                OptLog = OrderOptionLog,
                OrderId = orderId
            };
            var reg = iOrderProvider.AuditRefuse(orderOptionModel);
            return Json(new ResultModel(reg.DealFlag, reg.DealMsg), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 审核通过
        /// 胡灵波
        /// 2015年8月12日 10:39:16
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuditOK(int orderId)
        {
            OrderOptionModel orderOptionModel = new OrderOptionModel()
            {
                OptUserId = UserContext.Current.Id,
                OptUserName = UserContext.Current.Name,
                OrderId = orderId
            };
            var reg = iOrderProvider.AuditOK(orderOptionModel);
            return Json(new ResultModel(reg.DealFlag, reg.DealMsg), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 财务管理-订单审核管理
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderAudit()
        {
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id

            ViewBag.GroupList = iBusinessProvider.GetGroups();
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var superManModel = iDistributionProvider.GetClienterModelByGroupID(ViewBag.txtGroupId);
            if (superManModel != null)
            {
                ViewBag.superManModel = superManModel;
            }
            var criteria = new OrderSearchCriteria();
            TryUpdateModel(criteria);
            criteria.AuditStatus = 0;
            criteria.UserType = UserType;
            //criteria.GroupId = UserContext.Current.GroupId;
            criteria.AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType);

            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return View();
            }
            var pagedList = iOrderProvider.GetOrders(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult PostOrderAudit(int pageindex = 1)
        {
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId; ;//集团id
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var criteria = new OrderSearchCriteria();
            TryUpdateModel(criteria);
            criteria.AuthorityCityNameListStr =
                iAreaProvider.GetAuthorityCityNameListStr(UserType);
            criteria.UserType = UserType;
            //指派超人时  以下代码 有用，现在 注释掉  wc 
            //var superManModel = iDistributionProvider.GetClienterModelByGroupID(ViewBag.txtGroupId);
            //if (superManModel != null)
            //{
            //    ViewBag.superManModel = superManModel;
            //} 
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return PartialView("_PartialOrderAuditList");
            }
            var pagedList = iOrderProvider.GetOrders(criteria);

            return PartialView("_PartialOrderAuditList", pagedList);
        }
        [HttpPost]
        public JsonResult BatchAuditCancel(string orderIds, string remark)
        {
            string totalMsg = "";
            if (!string.IsNullOrEmpty(orderIds))
            {
                string[] ids = orderIds.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, DealResultInfo> result = new Dictionary<string, DealResultInfo>();
                foreach (var item in ids)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] infos = item.Split(',');
                        OrderOptionModel orderOptionModel = new OrderOptionModel()
                        {
                            OptUserId = UserContext.Current.Id,
                            OptUserName = UserContext.Current.Name,
                            OptLog = remark,
                            OrderId = int.Parse(infos[0])
                        };
                        var reg = iOrderProvider.AuditRefuse(orderOptionModel);
                        result.Add(infos[1], reg);
                    }
                }
                int successedNum = result.Where(p => p.Value.DealFlag == true).Count();
                totalMsg = string.Format("共{0}个订单，其中{1}个操作成功;", ids.Length, successedNum);
                if (successedNum < ids.Length)
                {
                    var failedList = result.Where(p => p.Value.DealFlag == false);
                    var failedMsgList = failedList.Select(p => p.Key + ":" + p.Value.DealMsg);
                    string failedMsg = string.Join("\n", failedMsgList);
                    totalMsg += string.Format("{0}个操作失败:\n{1}", failedList.Count(), failedMsg);
                }
            }
            return Json(new ResultModel(true, totalMsg), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BatchAuditOK(string orderIds)
        {
            string totalMsg = "";
            if (!string.IsNullOrEmpty(orderIds))
            {
                string[] ids = orderIds.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, DealResultInfo> result = new Dictionary<string, DealResultInfo>();
                foreach (var item in ids)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] infos = item.Split(',');
                        OrderOptionModel orderOptionModel = new OrderOptionModel()
                        {
                            OptUserId = UserContext.Current.Id,
                            OptUserName = UserContext.Current.Name,
                            OrderId = int.Parse(infos[0])
                        };
                        var reg = iOrderProvider.AuditOK(orderOptionModel);
                        result.Add(infos[1], reg);
                    }
                }
                int successedNum = result.Where(p => p.Value.DealFlag == true).Count();
                totalMsg = string.Format("共{0}个订单，其中{1}个操作成功;", ids.Length, successedNum);
                if (successedNum < ids.Length)
                {
                    var failedList = result.Where(p => p.Value.DealFlag == false);
                    var failedMsgList = failedList.Select(p => p.Key + ":" + p.Value.DealMsg);
                    string failedMsg = string.Join("\n", failedMsgList);
                    totalMsg += string.Format("{0}个操作失败:\n{1}", failedList.Count(), failedMsg);
                }
            }
            return Json(new ResultModel(true, totalMsg), JsonRequestBehavior.AllowGet);

        }

        #region 用户自定义方法

        private bool IsShowAuditBtn(OrderListModel orderModel)
        {
            //只有在已完成订单并且已上传完小票的情况下显示该按钮
            if (orderModel != null && /*已完成*/ orderModel.FinishAll == 1 && /*订单未分账*/ orderModel.IsJoinWithdraw == 0
                && orderModel.IsEnable == 1)
            {
                return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 超时订单页面
        /// 茹化肖
        /// 2015年8月28日10:34:46
        /// </summary>
        /// <returns></returns>
        public ActionResult OverTimeOrder()
        {
            OverTimeOrderPM model=new OverTimeOrderPM();
            model.PageIndex = 1;
            var list= iOrderProvider.GetOverTimeOrderList<OverTimeOrderModel>(model);
            return View(list);
        }

        /// <summary>
        /// 超时订单页面(异步列表)
        /// 茹化肖
        /// 2015年8月28日10:34:46
        /// </summary>
        /// <returns></returns>
        public ActionResult PostOverTimeOrder(int pageindex = 1)
        {
            OverTimeOrderPM model = new OverTimeOrderPM();
            TryUpdateModel(model);
            model.PageIndex = pageindex;
            var list = iOrderProvider.GetOverTimeOrderList<OverTimeOrderModel>(model);
            return PartialView("_PostOverTimeOrder",list);
        }

        /// <summary>
        /// 获取城市列表 仿google下拉列表框
        /// 胡灵波
        /// 2015年8月31日 10:40:51
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public ContentResult GetCity(string cityName)
        {
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id

            string cityNameZ = Server.UrlDecode(cityName);
            IList<AreaModel> aMoldeList = iAreaProvider.GetOpenCity(ParseHelper.ToInt(UserType)).AreaModels.Where(p => p.Name.Contains(cityNameZ)).ToList();
            string callback = "{\"citylist\":[";  
            for(int i=0;i<aMoldeList.Count;i++)
            {
                if (i == aMoldeList.Count - 1)
                {
                    callback += "{\"id\":" + i + ",\"city\":\"" + aMoldeList[i].Name + "\"}";                
                }
                else
                {
                    callback += "{\"id\":" + i + ",\"city\":\"" + aMoldeList[i].Name + "\"},";                
                }
            }
            callback += "]}";

            return Content(callback);
        } 

    }
}
