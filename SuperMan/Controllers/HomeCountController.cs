using Ets.Service.IProvider.Clienter;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Order;
using SuperMan.App_Start;
using SuperMan.Authority;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class HomeCountController : BaseController
    {
        IOrderProvider iOrderProvider = new OrderProvider();
        IClienterProvider iClienterProvider = new ClienterProvider();
        // GET: HomeCount
        public ActionResult Index()
        {
            //当前用户没有后台首页的权限，但是有物流公司订单管理权限，则认为是物流公司登陆，返回物流公司的订单管理页面
            if (!UserContext.Current.HasAuthority(50))
            {
                if (UserContext.Current.HasAuthority(70))
                {
                    return new ContentResult() { Content = "<script>window.location='/DeliveryManager/OrderManager'</script>" };
                }
                return new ContentResult() { Content = "<script>window.location='/order/order'</script>" };
            }
            Ets.Service.Provider.Common.HomeCountProvider homeCountProvider = new Ets.Service.Provider.Common.HomeCountProvider();
            var criteria = new Ets.Model.ParameterModel.Order.OrderSearchCriteria();
            ViewBag.homeCountTitleToAllData = homeCountProvider.GetHomeCountTitleToAllData();  //总数据统计
            ViewBag.homeCountTitleModel = homeCountProvider.GetCurrentDateModel();  //当日数据统计
            IList<Ets.Model.DomainModel.Business.BusinessesDistributionModel> clienteStorerGrabStatistical = iClienterProvider.GetClienteStorerGrabStatisticalInfo();
            ViewBag.clienteStorerGrabStatistical = clienteStorerGrabStatistical.ToList();
            var pagedList = iOrderProvider.GetCurrentDateCountAndMoney(criteria);  //每天数据统计
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
            var pagedList = iOrderProvider.GetCurrentDateCountAndMoney(criteria);  //每天数据统计
            return PartialView("_PartialIndex", pagedList);
        }
        //[HttpPost]
        //public ActionResult orderCount(Ets.Model.DomainModel.Subsidy.HomeCountCriteria criteria)
        //{
        //    var pagedList = OrderLogic.orderLogic().GetOrderCount(criteria);
        //    var item = pagedList;
        //    return PartialView("_PartialOrderCount", item);
        //}
        //[HttpPost]
        //public ActionResult supermanCount(Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria criteria)
        //{
        //    var pagedList = ClienterLogic.clienterLogic().GetClienteresCount(criteria);
        //    var item = pagedList.clienterCountManageList;
        //    return PartialView("_PartialSuperManCount", item);
        //}
        //[HttpPost]
        //public ActionResult busiCount(Ets.Model.ParameterModel.Business.BusinessSearchCriteria criteria)
        //{
        //    var pagedList = BusiLogic.busiLogic().GetBusinessesCount(criteria);
        //    var item = pagedList.businessCountManageList;
        //    return PartialView("_PartialBisuCount", item);
        //}
    }
}