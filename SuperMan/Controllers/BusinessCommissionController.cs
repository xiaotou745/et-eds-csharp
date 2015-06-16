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
using Ets.Model.DataModel.Business;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel.Business;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.User;
using SuperMan.App_Start;


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
        IAreaProvider iAreaProvider = new AreaProvider();

        /// <summary>
        /// 默认视图
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessCommission()
        {
            ViewBag.txtGroupId = UserContext.Current.GroupId;
            var userType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = new AreaProvider().GetOpenCityOfSingleCity(userType);
            var t1 = new DateTime(DateTime.Now.AddDays(-7).Year, DateTime.Now.AddDays(-7).Month, DateTime.Now.AddDays(-7).Day, 0, 0, 0);
            var t2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            var authorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(userType);
            var criteria =
               new Ets.Model.ParameterModel.Business.BusinessCommissionSearchCriteria
               {
                   T1 = t1.ToString(),
                   T2 = t2.ToString(),
                   AuthorityCityNameListStr = authorityCityNameListStr
               };
            if (userType > 0 && string.IsNullOrWhiteSpace(authorityCityNameListStr))
            {
                return View();
            }
            var result = iBusinessProvider.GetBusinessCommissionOfPaging(criteria);
            return View(result);
        }
        ///// <summary>
        ///// 查询商户结算信息
        ///// </summary>
        ///// <param name="criteria"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult BusinessCommissions(BusinessCommissionSearchCriteria criteria)
        //{
        //    ViewBag.openCityList = new AreaProvider().GetOpenCityOfSingleCity(0);
        //    int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
        //    DateTime date1 = DateTime.Now;
        //    DateTime date2 = DateTime.Now;  
        //    date1 = string.IsNullOrEmpty(criteria.txtDateStart) ? new DateTime(2014, 1, 1,0,0,0) : DateTime.Parse(criteria.txtDateStart);
        //    date2 = string.IsNullOrEmpty(criteria.txtDateEnd) ? DateTime.Now : DateTime.Parse(criteria.txtDateEnd);
        //    date1=new DateTime(date1.Year,date1.Month,date1.Day,0,0,0);
        //    date2 = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);
        //    ViewBag.startDate = criteria.txtDateStart;
        //    ViewBag.endDate = criteria.txtDateEnd;
        //    ViewBag.name = criteria.txtBusinessName;
        //    ViewBag.phoneno = criteria.txtBusinessPhoneNo;
        //    ViewBag.BusinessCity = criteria.BusinessCity;
        //    if (criteria.BusinessCity == "--无--")
        //    {
        //        criteria.BusinessCity = "";
        //    }
        //    string authorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType);
        //    if (UserType > 0 && string.IsNullOrWhiteSpace(authorityCityNameListStr))
        //    {
        //        return View("BusinessCommission");
        //    }
        //    var result = iBusinessProvider.GetBusinessCommission(date1, date2, criteria.txtBusinessName, criteria.txtBusinessPhoneNo, criteria.txtGroupId, criteria.BusinessCity, authorityCityNameListStr);
        //    return View("BusinessCommission", result);
        //}


        ///// <summary>
        ///// 查询商户结算信息
        ///// </summary>
        ///// <param name="criteria"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult BusinessCommissions()
        //{
        //    ViewBag.txtGroupId = UserContext.Current.GroupId;
        //    var userType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
        //    ViewBag.openCityList = new AreaProvider().GetOpenCityOfSingleCity(userType);
        //    var t1 = new DateTime(2014, 1, 1, 0, 0, 0);
        //    var t2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        //    var authorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(userType);
        //    var criteria =
        //       new Ets.Model.ParameterModel.Bussiness.BusinessCommissionSearchCriteria
        //       {
        //           T1 = t1,
        //           T2 = t2,
        //           AuthorityCityNameListStr = authorityCityNameListStr
        //       };
        //    if (userType > 0 && string.IsNullOrWhiteSpace(authorityCityNameListStr))
        //    {
        //        return View();
        //    }
        //    var result = iBusinessProvider.GetBusinessCommissionOfPaging(criteria);
        //    return View(result);


        //}
        /// <summary>
        /// 查询商户结算信息
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostBusinessCommissions(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Business.BusinessCommissionSearchCriteria();
            TryUpdateModel(criteria);
            ViewBag.openCityList = new AreaProvider().GetOpenCityOfSingleCity(0);
            int userType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now;
            date1 = string.IsNullOrEmpty(criteria.T1) ? new DateTime(DateTime.Now.AddDays(-7).Year, DateTime.Now.AddDays(-7).Month, DateTime.Now.AddDays(-7).Day, 0, 0, 0) : DateTime.Parse(criteria.T1.ToString());
            date2 = string.IsNullOrEmpty(criteria.T2) ? DateTime.Now : DateTime.Parse(criteria.T2.ToString());
            date1 = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);
            date2 = new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59);
            if (criteria.BusinessCity == "--无--")
            {
                criteria.BusinessCity = "";
            }
            string authorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(userType);
            criteria.T1 = date1.ToString();
            criteria.T2 = date2.ToString();
            criteria.AuthorityCityNameListStr = authorityCityNameListStr;
            
            if (userType > 0 && string.IsNullOrWhiteSpace(authorityCityNameListStr))
            {
                return PartialView("_BusinessCommissionList");
            }
            var result = iBusinessProvider.GetBusinessCommissionOfPaging(criteria);
            return PartialView("_BusinessCommissionList", result);
        }
        /// <summary>
        /// 导出商户结算金额excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateCommissionsExcel(BusinessCommissionSearchCriteria criteria)
        {
            ViewBag.openCityList = new AreaProvider().GetOpenCityOfSingleCity(0);
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
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
            if (criteria.BusinessCity == "--无--")
            {
                criteria.BusinessCity = "";
            }
            string authorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType);
            if (UserType > 0 && string.IsNullOrWhiteSpace(authorityCityNameListStr))
            {
                return View("BusinessCommission");
            }
            var result = iBusinessProvider.GetBusinessCommission(date1, date2, criteria.txtBusinessName, criteria.txtBusinessPhoneNo, criteria.txtGroupId, criteria.BusinessCity, authorityCityNameListStr);
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
        private string CreateExcel(IList<BusinessCommissionDM> paraModel)
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
                //strBuilder.AppendLine(string.Format("<td>{0}</td>", businessCommissionModel.T1.ToShortDateString()));
                //strBuilder.AppendLine(string.Format("<td>{0}</td>", businessCommissionModel.T2.ToShortDateString()));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", businessCommissionModel.T1));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", businessCommissionModel.T2));
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