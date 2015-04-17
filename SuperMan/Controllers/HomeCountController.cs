using Ets.Service.IProvider.Clienter;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Order;
using SuperMan.App_Start;
using SuperMan.Authority;
using SuperManBusinessLogic.B_Logic;
using SuperManBusinessLogic.C_Logic;
using SuperManBusinessLogic.Order_Logic;
using SuperManCommonModel.Entities;
using SuperManCommonModel.Models;
using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    [WebHandleError]
    public class HomeCountController : BaseController
    {
        IOrderProvider iOrderProvider = new OrderProvider();
        IClienterProvider iClienterProvider = new ClienterProvider();
        // GET: HomeCount
        public ActionResult Index()
        {
            Ets.Service.Provider.Common.HomeCountProvider homeCountProvider = new Ets.Service.Provider.Common.HomeCountProvider();
            var criteria = new Ets.Model.ParameterModel.Order.OrderSearchCriteria();
            ViewBag.homeCountTitleToAllData = homeCountProvider.GetHomeCountTitleToAllData();
            //ViewBag.homeCountTitleToList = homeCountProvider.GetHomeCountTitleToList(21);
            ViewBag.homeCountTitleModel = homeCountProvider.GetHomeCountTitle();
            ViewBag.clienteStorerGrabStatistical = iClienterProvider.GetClienteStorerGrabStatisticalInfo().ToList();
            var pagedList = iOrderProvider.GetCurrentDateCountAndMoney(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult PostIndex(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Order.OrderSearchCriteria();
            TryUpdateModel(criteria);
            Ets.Service.Provider.Common.HomeCountProvider homeCountProvider = new Ets.Service.Provider.Common.HomeCountProvider();
            ViewBag.homeCountTitleToAllData = homeCountProvider.GetHomeCountTitleToAllData();//获取总统计数据
            ViewBag.homeCountTitleModel = homeCountProvider.GetHomeCountTitle();//当前统计
            ViewBag.clienteStorerGrabStatistical = iClienterProvider.GetClienteStorerGrabStatisticalInfo();
            var pagedList = iOrderProvider.GetCurrentDateCountAndMoney(criteria);
            return PartialView("_PartialIndex", pagedList);
        }
        [HttpPost]
        public ActionResult orderCount(HomeCountCriteria criteria)
        {
            var pagedList = OrderLogic.orderLogic().GetOrderCount(criteria);
            var item = pagedList;
            return PartialView("_PartialOrderCount", item);
        }
        [HttpPost]
        public ActionResult supermanCount(ClienterSearchCriteria criteria)
        {
            var pagedList = ClienterLogic.clienterLogic().GetClienteresCount(criteria);
            var item = pagedList.clienterCountManageList;
            return PartialView("_PartialSuperManCount", item);
        }
        [HttpPost]
        public ActionResult busiCount(BusinessSearchCriteria criteria)
        {
            var pagedList = BusiLogic.busiLogic().GetBusinessesCount(criteria);
            var item = pagedList.businessCountManageList;
            return PartialView("_PartialBisuCount", item);
        }
    }
}