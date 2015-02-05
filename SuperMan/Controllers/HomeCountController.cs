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
    [Authorize]
    [WebHandleError]
    public class HomeCountController : Controller
    {
        // GET: HomeCount
        public ActionResult Index()
        {
            //if (!UserContext.Current.HasAuthority(AuthorityNames.OrderView))
            //{
            //    Redirect("/Account/Login");
            //};


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