using SuperManBusinessLogic.B_Logic;
using SuperManCommonModel.Entities;
using SuperManCore.Common;
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
    public class BusinessManagerController : Controller
    {
        // GET: BusinessManager
        public ActionResult BusinessManager()
        {
            var criteria = new BusinessSearchCriteria() { PagingRequest = new PagingResult(0, 15) };
            criteria.Status = -1; //默认加载全部
            var pagedList = BusiLogic.busiLogic().GetBusinesses(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult BusinessManager(BusinessSearchCriteria criteria)
        {
            var pagedList = BusiLogic.busiLogic().GetBusinesses(criteria);
            var item = pagedList.businessManageList;
            return PartialView("_BusinessManageList", item);
        }

        [HttpPost]
        public JsonResult AuditOK(int id)
        {
            BusiLogic.busiLogic().UpdateAuditStatus(id,EnumStatusType.审核通过);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AuditCel(int id)
        {
            BusiLogic.busiLogic().UpdateAuditStatus(id, EnumStatusType.审核取消);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }
    }
}