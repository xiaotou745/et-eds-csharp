using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Model.DomainModel.DeliveryCompany;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.DeliveryCompany;
using ETS.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SuperMan.App_Start;

namespace SuperMan.Controllers
{

    /// <summary>
    /// 物流公司相关业务 
    /// </summary>
    [WebHandleError]
    public class DeliveryCompanyController : BaseController
    {

        public ActionResult DeliveryCompany()
        { 
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id

             
            ViewBag.deliveryCompanyList = new DeliveryCompanyProvider().Get();//获取物流公司
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria()
            {
                Status = -1,
                GroupId = SuperMan.App_Start.UserContext.Current.GroupId 

            };
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return View();
            }
            return View();
            //ViewBag.openCityList.Result.AreaModels;
            //var pagedList = iDistributionProvider.GetClienteres(criteria);
            //return System.Web.UI.WebControls.View();
        }

        [HttpPost]
        public ActionResult PostDeliveryCompany(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria();
            TryUpdateModel(criteria);

            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
             
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return PartialView("_DeliveryCompanyList");
            } 
            return PartialView("_DeliveryCompanyList", null);
        }


        /// <summary>
        /// 批量导入骑士页面 add by caoheyang 20150706
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <returns></returns>
        public ActionResult BatchImportClienter(int companyId)
        {
            return View();
        }

        /// <summary>
        /// 批量导入骑士  excel  处理  add by caoheyang 20150706
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <returns></returns>
        public ActionResult BatchImportClienterExcel(int companyId)
        {
            string businessId = Request.Params["BusinessId"].ToString();
            List<BatchImportClienterExcelDM> list = new List<BatchImportClienterExcelDM>();
            Stream fs = null;
            IWorkbook wk = null;

            if (Request.Files["file1"] != null && Request.Files["file1"].FileName != "")
            {
                HttpPostedFileBase file = Request.Files["file1"];
                fs = file.InputStream;
                if (Path.GetExtension(Request.Files["file1"].FileName) == ".xls")
                    wk = new HSSFWorkbook(fs);
                else
                    wk = new XSSFWorkbook(fs);

                ISheet st = wk.GetSheetAt(0);
                int rowCount = st.LastRowNum;
                if (rowCount > 50)
                {
                    rowCount = 50;
                    return Json(new Ets.Model.Common.ResultModel(false, "每次最多导入50行数据！", JsonRequestBehavior.DenyGet));
                }
                //list = GetList(st, rowCount);
            }
            return Json(list);
        }
    }
}