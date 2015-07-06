using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}