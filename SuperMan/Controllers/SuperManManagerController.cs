using SuperManBusinessLogic.C_Logic;
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
    public class SuperManManagerController : Controller
    {
        // GET: BusinessManager
        public ActionResult SuperManManager()
        {
            var criteria = new ClienterSearchCriteria() { PagingRequest = new PagingResult(0, 15) };
            criteria.Status = -1;
            var pagedList = ClienterLogic.clienterLogic().GetClienteres(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult SuperManManager(ClienterSearchCriteria criteria)
        {
            var pagedList = ClienterLogic.clienterLogic().GetClienteres(criteria);
            var item = pagedList.clienterManageList;
            return PartialView("_SuperManManagerList", item);
        }

        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuditOK(int id)
        {
            ClienterLogic.clienterLogic().UpdateAuditStatus(id, EnumStatusType.审核通过);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取消审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuditCel(int id)
        {
            ClienterLogic.clienterLogic().UpdateAuditStatus(id, EnumStatusType.审核取消);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 帐户清零
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AmountClear(int id)
        {
            bool b = ClienterLogic.clienterLogic().ClearSuperManAmount(id);
            if(b)
            {
                return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultModel(false, "清零失败"), JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}