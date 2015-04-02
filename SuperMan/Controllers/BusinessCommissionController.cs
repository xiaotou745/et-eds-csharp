using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel.Bussiness;
using Ets.Service.Provider.User;
using SuperManCommonModel.Entities;


namespace SuperMan.Controllers
{
    /// <summary>
    /// 商户结算控制器
    /// </summary>
    [WebHandleError]
    public class BusinessCommissionController : BaseController
    {
        /// <summary>
        /// 商户业务类
        /// </summary>
        Ets.Service.IProvider.User.IBusinessProvider iBusinessProvider = new BusinessProvider(); 

        /// <summary>
        /// 默认视图
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessCommission()
        {
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;
            DateTime t1 = new DateTime(1997,1,1,0,0,0);
            DateTime t2 =new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59); 
            var result = iBusinessProvider.GetBusinessCommission(t1, t2, "", SuperMan.App_Start.UserContext.Current.GroupId);
            return View(result);
        }
        /// <summary>
        /// 查询商户结算信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BusinessCommissions(BusinessCommissionSearchCriteria criteria)
        {  
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now;  
            date1 = string.IsNullOrEmpty(criteria.txtDateStart) ? new DateTime(2014, 1, 1,0,0,0) : DateTime.Parse(criteria.txtDateStart);
            date2 = string.IsNullOrEmpty(criteria.txtDateEnd) ? DateTime.Now : DateTime.Parse(criteria.txtDateEnd);
            date1=new DateTime(date1.Year,date1.Month,date1.Day,0,0,0);
            date2 = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);
            ViewBag.startDate = date1;
            ViewBag.endDate = date2;
            ViewBag.name = criteria.txtBusinessName;
            var result = iBusinessProvider.GetBusinessCommission(date1, date2, criteria.txtBusinessName, criteria.txtGroupId);
            return View("BusinessCommission", result);
        }

        /// <summary>
        /// 导出商户结算金额excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateBusinessCommissionsExcel(BusinessCommissionModel model)
        {
            string filname = model.Name +"_订单结算统计_"+DateTime.Now.ToShortDateString()+".xls"; 
            byte[] data = Encoding.Default.GetBytes(iBusinessProvider.CreateExcel(model));
            return File(data, "application/ms-excel", filname); 
        } 
    }
}