using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ETS.Enums;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Order;
using Ets.Model.ParameterModel.Clienter;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.DeliveryManager;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.DeliveryManager;
using ETS.Util;
using SuperMan.App_Start;

namespace SuperMan.Controllers
{
    /// <summary>
    /// 物流订单管理模块相关业务
    /// </summary>
    [WebHandleError]
    public class DeliveryManagerController : BaseController
    {
        readonly IAreaProvider iAreaProvider = new AreaProvider();
        readonly IDeliveryManagerProvider iDeliveryManagerProvider=new DeliveryManagerProvider();
        /// <summary>
        /// 物流订单管理-骑士管理
        /// </summary>
        /// <returns></returns>
        public ActionResult SuperManManager()
        {
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(UserType);//获取筛选城市
            var dclist=new CompanyProvider().GetCompanyListByAccountID(UserContext.Current.Id);//获取物流公司
            ViewBag.deliveryCompanyList = dclist;
            var criteria = new ClienterSearchCriteria()
            {
                Status = -1,
                GroupId = SuperMan.App_Start.UserContext.Current.GroupId,
                AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType),
                deliveryCompany = dclist.Count>0?dclist[0].CompanyId.ToString():"-1"
            };
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return View();
            }
            var pagedList = iDeliveryManagerProvider.GetClienterList(criteria);
            return View(pagedList);
        }
        /// <summary>
        /// 物流订单管理-骑士管理-异步列表分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostSuperManManager(int pageindex = 1)
        {
            var criteria = new ClienterSearchCriteria();
            TryUpdateModel(criteria);
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;
            criteria.AuthorityCityNameListStr =
               iAreaProvider.GetAuthorityCityNameListStr(ParseHelper.ToInt(UserType));
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(UserType);
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return PartialView("_PostSuperManManager");
            }
            var pagedList = iDeliveryManagerProvider.GetClienterList(criteria);
            return PartialView("_PostSuperManManager", pagedList);
        }

        public ActionResult SuperManExport()
        {
            var criteria = new ClienterSearchCriteria();
            TryUpdateModel(criteria);
            criteria.PageIndex = 1;
            criteria.PageSize = 65534;
            
            var result=iDeliveryManagerProvider.GetClienterList(criteria).Records;
            int totalRows = result.Count;

            string excelContent = this.CreateSuperManExcel(result);
            byte[] data = Encoding.UTF8.GetBytes(excelContent);
            string filname = "e代送-骑士导出数据-"+DateTime.Now.ToString("yyyy-MM-dd")+".xls";
            return File(data, "application/ms-excel", filname);
        }

        private string CreateSuperManExcel(IList<ClienterListModel> paraModel)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
            //输出表头.
            strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            strBuilder.AppendLine("<td>编号</td>");
            strBuilder.AppendLine("<td>姓名</td>");
            strBuilder.AppendLine("<td>工作状态</td>");
            strBuilder.AppendLine("<td>电话</td>");
            strBuilder.AppendLine("<td>身份证号</td>");
            strBuilder.AppendLine("<td>申请时间</td>");
            strBuilder.AppendLine("<td>审核状态</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            foreach (var item in paraModel)
            {
                #region
                var statusView = "";
                if (item.Status == ClienteStatus.Status1.GetHashCode())
                {
                    statusView = "审核通过";
                }

                else if (item.Status == ClienteStatus.Status0.GetHashCode())
                {
                    statusView = "审核被拒绝";
                }
                else if (item.Status == ClienteStatus.Status3.GetHashCode())
                {
                    statusView = "审核中";
                }
                else if (item.Status == ClienteStatus.Status2.GetHashCode())
                {
                    statusView = "未审核";
                }
                #endregion
                strBuilder.AppendLine(string.Format("<tr><td>{0}</td>", item.Id));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.TrueName));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.WorkStatus==0?"上班":"下班"));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.PhoneNo));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.IDCard));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.InsertTime));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", statusView));
                strBuilder.AppendLine("</tr>");
            }
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
        }

        /// <summary>
        /// 物流订单管理-订单管理
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderManager()
        {
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var dclist = new CompanyProvider().GetCompanyListByAccountID(UserContext.Current.Id);//获取物流公司
            ViewBag.deliveryCompanyList = dclist;
            var criteria = new OrderSearchCriteria()
            {
                orderStatus = -1,
                //GroupId = UserContext.Current.GroupId,
                AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType),
                deliveryCompany = dclist.Count > 0 ? dclist[0].CompanyId.ToString() : "-1"
            };
            var pagedList = iDeliveryManagerProvider.GetOrderList(criteria);
            return View(pagedList);
        }
        /// <summary>
        /// 物流订单管理-订单管理-异步列表分页
        /// </summary>
        /// <returns></returns>
        public ActionResult PostOrderManager(int pageindex = 1)
        {
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var criteria = new OrderSearchCriteria();
            TryUpdateModel(criteria);
            criteria.AuthorityCityNameListStr =iAreaProvider.GetAuthorityCityNameListStr(UserType);
            criteria.deliveryCompany=string.IsNullOrWhiteSpace(criteria.deliveryCompany) ? "-1" : criteria.deliveryCompany;
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return PartialView("_PostOrderManager");
            }
            var pagedList = iDeliveryManagerProvider.GetOrderList(criteria);

            return PartialView("_PostOrderManager", pagedList);
        }
        /// <summary>
        /// 物流订单管理-订单列表-订单详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderDetail(string orderNo, int orderId)
        {
            var orderModel = iDeliveryManagerProvider.GetOrderByNo(orderNo, orderId);
            ViewBag.orderOptionLog = iDeliveryManagerProvider.GetOrderOptionLog(orderId);
            return View(orderModel);
        }

        public ActionResult OrderExport()
        {
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var criteria = new OrderSearchCriteria();
            TryUpdateModel(criteria);
            criteria.PageIndex = 1;
            criteria.PageSize = 65534;
            criteria.AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType);
            var pagedList = iDeliveryManagerProvider.GetOrderList(criteria).Records;
            string excelContent = this.CreateOrderExcel(pagedList);
            byte[] data = Encoding.UTF8.GetBytes(excelContent);
            string filname = "e代送-订单导出数据"+criteria.orderPubStart+"-"+criteria.orderPubEnd+".xls";
            return File(data, "application/ms-excel", filname);
        }

        private string CreateOrderExcel(IList<OrderListModel> pagedList)
        {
            //订单号、商户名称、骑士ID、骑士姓名、下单时间、接单时间、
            //取货时间、完成时间、配送费、订单金额、订单状态、城市、是否在线支付、是否异常订单
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
            //输出表头.
            strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            strBuilder.AppendLine("<td>订单号</td>");
            strBuilder.AppendLine("<td>商户名称</td>");
            strBuilder.AppendLine("<td>骑士ID</td>");
            strBuilder.AppendLine("<td>骑士姓名</td>");
            strBuilder.AppendLine("<td>下单时间</td>");
            strBuilder.AppendLine("<td>接单时间</td>");
            strBuilder.AppendLine("<td>取货时间</td>"); 
            strBuilder.AppendLine("<td>完成时间</td>");
            strBuilder.AppendLine("<td>配送费</td>");
            strBuilder.AppendLine("<td>订单金额</td>");
            strBuilder.AppendLine("<td>订单状态</td>");
            strBuilder.AppendLine("<td>城市</td>");
            strBuilder.AppendLine("<td>是否在线支付</td>");
            strBuilder.AppendLine("<td>是否异常订单</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            foreach (var item in pagedList)
            {
                var statusView =ETS.Extension.EnumExtenstion.GetEnumItem(((ETS.Enums.OrderStatusCommon) item.Status).GetType(),
                        (ETS.Enums.OrderStatusCommon) item.Status).Text;
                strBuilder.AppendLine(string.Format("<tr><td>{0}</td>", item.OrderNo));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.BusinessName));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.clienterId));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.ClienterName));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.PubDate));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.GrabTime));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.GrabTime));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.ActualDoneDate));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.DistribSubsidy));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.Amount));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", statusView));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.BusinessCity));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.MealsSettleMode==1?"是":"否"));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", item.IsNotRealOrder==1?"是":"否"));
                strBuilder.AppendLine("</tr>");
            }
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
        }
    }
}