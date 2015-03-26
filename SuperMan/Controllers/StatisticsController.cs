using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using SuperMan.App_Start;
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
    public class StatisticsController : Controller
    {
        IOrderProvider iOrderProvider = new OrderProvider();
        public ActionResult OrderCount()
        {
            var criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria() { searchType = 1 };
            var pagedList = iOrderProvider.GetOrderCount(criteria);
            return View(pagedList);
        }
        
        [HttpPost]
        public ActionResult PostOrderCountNew(int pageindex = 1)
        {
            var criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria();
            TryUpdateModel(criteria);
            var pagedList = iOrderProvider.GetOrderCount(criteria);
            return PartialView("_PartialOrderCountNew", pagedList);
        }
        public ActionResult BusinessCount()
        {
            if (UserContext.Current.Id == 0)
            {
                Response.Redirect("/account/login");
                return null;
            }

            HomeCountManage homeCountManage = new HomeCountManage();
            var criteria = new HomeCountCriteria() { PagingRequest = new PagingResult(0, 15), searchType = 1 };
            var busiCriteria = new BusinessSearchCriteria() { PagingRequest = new PagingResult(0, 15), searchType = 1 };
            var clientCriteria = new ClienterSearchCriteria() { PagingRequest = new PagingResult(0, 15), searchType = 1 };
            homeCountManage.orderCountManageList = OrderLogic.orderLogic().GetOrderCount(criteria);
            homeCountManage.busiCountManagerList = BusiLogic.busiLogic().GetBusinessesCount(busiCriteria);
            homeCountManage.clientCountManagerList = ClienterLogic.clienterLogic().GetClienteresCount(clientCriteria);

            HomeCountTitleModel homeCountTitleModel = OrderLogic.orderLogic().GetHomeCountTitle();
            ViewBag.homeCountTitleModel = homeCountTitleModel;
            return View(homeCountManage);
        }
        public ActionResult SuperManCount()
        {
            if (UserContext.Current.Id == 0)
            {
                Response.Redirect("/account/login");
                return null;
            }

            HomeCountManage homeCountManage = new HomeCountManage();
            var criteria = new HomeCountCriteria() { PagingRequest = new PagingResult(0, 15), searchType = 1 };
            var busiCriteria = new BusinessSearchCriteria() { PagingRequest = new PagingResult(0, 15), searchType = 1 };
            var clientCriteria = new ClienterSearchCriteria() { PagingRequest = new PagingResult(0, 15), searchType = 1 };
            homeCountManage.orderCountManageList = OrderLogic.orderLogic().GetOrderCount(criteria);
            homeCountManage.busiCountManagerList = BusiLogic.busiLogic().GetBusinessesCount(busiCriteria);
            homeCountManage.clientCountManagerList = ClienterLogic.clienterLogic().GetClienteresCount(clientCriteria);

            HomeCountTitleModel homeCountTitleModel = OrderLogic.orderLogic().GetHomeCountTitle();
            ViewBag.homeCountTitleModel = homeCountTitleModel;
            return View(homeCountManage);
        }
    }
}