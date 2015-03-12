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
using SuperManDataAccess;


namespace SuperMan.Controllers
{
    /// <summary>
    /// 商户结算控制器
    /// </summary>
    [Authorize]
    [WebHandleError]
    public class BusinessCommissionController : Controller
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
            account account = HttpContext.Session["user"] as account;
            if (account == null)
            {
                Response.Redirect("/account/login");
                return null;
            } 
            ViewBag.txtGroupId = account.GroupId;//集团id
            DateTime t1=new DateTime(2014,1,1);
            DateTime t2 = DateTime.Now;
            var result = iBusinessProvider.GetBusinessCommission(t1, t2, "", account.GroupId??0);
            if (!result.Result)
            {
                return View();
            }
            return View(result.Data);
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
            date1 = string.IsNullOrEmpty(criteria.txtDateStart) ? new DateTime(2014, 1, 1) : DateTime.Parse(criteria.txtDateStart);
            date2 = string.IsNullOrEmpty(criteria.txtDateEnd) ? DateTime.Now : DateTime.Parse(criteria.txtDateEnd);
            var result = iBusinessProvider.GetBusinessCommission(date1, date2, criteria.txtBusinessName, criteria.txtGroupId);
            return View("BusinessCommission", result.Data);
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