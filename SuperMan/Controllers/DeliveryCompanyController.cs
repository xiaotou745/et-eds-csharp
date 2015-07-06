using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.DomainModel.DeliveryCompany;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace SuperMan.Controllers
{

    /// <summary>
    /// 物流公司相关业务 
    /// </summary>
    public class DeliveryCompanyController : BaseController
    {
        // GET: DeliveryCompany
        public ActionResult Index()
        {
            return View();
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
       [HttpPost]
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
                {
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    return Json(new ResultModel(false, "文件格式不正确,支持.xls文件！", JsonRequestBehavior.DenyGet));
                }
                ISheet st = wk.GetSheetAt(0);
                int rowCount = st.LastRowNum;
                if (rowCount > 50)
                {
                    return Json(new ResultModel(false, "每次最多导入50行数据！", JsonRequestBehavior.DenyGet));
                }
            }
            return Json(list);
        }
    }
}