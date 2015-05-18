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
using Ets.Service.Provider.Common;
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
            ViewBag.openCityList = new AreaProvider().GetOpenCityOfSingleCity();
            DateTime t1 = new DateTime(2014, 1, 1, 0, 0, 0);
            DateTime t2 =new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            var result = iBusinessProvider.GetBusinessCommission(t1, t2, "", "", SuperMan.App_Start.UserContext.Current.GroupId, "");
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
            ViewBag.openCityList = new AreaProvider().GetOpenCityOfSingleCity();
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now;  
            date1 = string.IsNullOrEmpty(criteria.txtDateStart) ? new DateTime(2014, 1, 1,0,0,0) : DateTime.Parse(criteria.txtDateStart);
            date2 = string.IsNullOrEmpty(criteria.txtDateEnd) ? DateTime.Now : DateTime.Parse(criteria.txtDateEnd);
            date1=new DateTime(date1.Year,date1.Month,date1.Day,0,0,0);
            date2 = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);
            ViewBag.startDate = criteria.txtDateStart;
            ViewBag.endDate = criteria.txtDateEnd;
            ViewBag.name = criteria.txtBusinessName;
            ViewBag.phoneno = criteria.txtBusinessPhoneNo;
            ViewBag.BusinessCity = criteria.BusinessCity;
            if (criteria.BusinessCity == "所有城市")
            {
                criteria.BusinessCity = "";
            }
            var result = iBusinessProvider.GetBusinessCommission(date1, date2, criteria.txtBusinessName, criteria.txtBusinessPhoneNo, criteria.txtGroupId, criteria.BusinessCity);
            return View("BusinessCommission", result);
        }

        /// <summary>
        /// 导出商户结算金额excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateCommissionsExcel(BusinessCommissionSearchCriteria criteria)
        {
            ViewBag.openCityList = new AreaProvider().GetOpenCityOfSingleCity();
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now;
            date1 = string.IsNullOrEmpty(criteria.txtDateStart) ? new DateTime(2014, 1, 1, 0, 0, 0) : DateTime.Parse(criteria.txtDateStart);
            date2 = string.IsNullOrEmpty(criteria.txtDateEnd) ? DateTime.Now : DateTime.Parse(criteria.txtDateEnd);
            date1 = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);
            date2 = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);
            ViewBag.startDate = criteria.txtDateStart;
            ViewBag.endDate = criteria.txtDateEnd;
            ViewBag.name = criteria.txtBusinessName;
            ViewBag.BusinessCity = criteria.BusinessCity;
            if (criteria.BusinessCity == "所有城市")
            {
                criteria.BusinessCity = "";
            }
            var result = iBusinessProvider.GetBusinessCommission(date1, date2, criteria.txtBusinessName, criteria.txtBusinessPhoneNo, criteria.txtGroupId, criteria.BusinessCity);
            if (result.Result && result.Data.Count > 0)
            {
                string filname = "e代送商户订单结算_" + date1.ToShortDateString() + "-" + date2.ToShortDateString() + ".xls";
                if (result.Data.Count > 3)
                {
                    byte[] data = Encoding.UTF8.GetBytes(CreateExcel(result.Data));
                    return File(data, "application/ms-excel", filname);
                }
                else
                {
                    byte[] data = Encoding.Default.GetBytes(CreateExcel(result.Data));
                    return File(data, "application/ms-excel", filname);
                }
                
            }
            return View("BusinessCommission", result);
        }

        /// <summary>
        /// 生成商户结算excel文件
        /// </summary>
        /// <returns></returns>
        private string CreateExcel(IList<BusinessCommissionModel> paraModel)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
            //输出表头.
            strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            strBuilder.AppendLine("<td>商户名称</td>");
            strBuilder.AppendLine("<td>订单金额</td>");
            strBuilder.AppendLine("<td>订单数量</td>");
            strBuilder.AppendLine("<td>结算比例(%)</td>");
            strBuilder.AppendLine("<td>开始时间</td>");
            strBuilder.AppendLine("<td>结束时间</td>");
            strBuilder.AppendLine("<td>结算金额</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            foreach (var businessCommissionModel in paraModel)
            {
                strBuilder.AppendLine(string.Format("<tr><td>{0}</td>", businessCommissionModel.Name));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", businessCommissionModel.Amount));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", businessCommissionModel.OrderCount));
                strBuilder.AppendLine(string.Format("<td>{0}%</td>", businessCommissionModel.BusinessCommission));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", businessCommissionModel.T1.ToShortDateString()));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", businessCommissionModel.T2.ToShortDateString()));
                strBuilder.AppendLine(string.Format("<td>{0}</td></tr>", businessCommissionModel.TotalAmount));
            } 
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
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