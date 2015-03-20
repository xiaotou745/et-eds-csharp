using Ets.Model.DataModel.Clienter;
using Ets.Service.Provider.Distribution;
using SuperManBusinessLogic.C_Logic;
using SuperManCommonModel;
using SuperManCommonModel.Entities;
using SuperManCore;
using SuperManCore.Common;
using SuperManCore.Paging;
using SuperManDataAccess;
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
        Ets.Service.IProvider.Distribution.IDistributionProvider iDistributionProvider = new DistributionProvider(); 
        // GET: BusinessManager
        public ActionResult SuperManManager()
        {
            account account = HttpContext.Session["user"] as account;
            if (account == null)
            {
                Response.Redirect("/account/login");
                return null;
            }
               
            ViewBag.txtGroupId = account.GroupId;//集团id
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria() { PagingRequest = new Ets.Model.Common.NewPagingResult(1, 15), Status = -1, GroupId = account.GroupId };
            //var pagedList = ClienterLogic.clienterLogic().GetClienteres(criteria);
            var pagedList = iDistributionProvider.GetClienteres(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult SuperManManager(Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria criteria)
        {
            //var pagedList = ClienterLogic.clienterLogic().GetClienteres(criteria);
            var pagedList = iDistributionProvider.GetClienteres(criteria);
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
            //ClienterLogic.clienterLogic().UpdateAuditStatus(id, EnumStatusType.审核通过);
            iDistributionProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核通过);
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
            //ClienterLogic.clienterLogic().UpdateAuditStatus(id, EnumStatusType.审核取消);
            iDistributionProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核取消);
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
            //bool b = ClienterLogic.clienterLogic().ClearSuperManAmount(id);
            bool b = iDistributionProvider.ClearSuperManAmount(id);
            if(b)
            {
                return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultModel(false, "清零失败"), JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 添加骑士
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddSuperMan(ClienterListModel clienter)
        {
            //if (ClienterLogic.clienterLogic().CheckExistPhone(clienter.PhoneNo))  //判断该手机号是否已经注册过
            if (iDistributionProvider.CheckExistPhone(clienter.PhoneNo))  //判断该手机号是否已经注册过
                return Json(new ResultModel(false, "手机号已被注册"));
            if (string.IsNullOrWhiteSpace(clienter.Password))
                clienter.Password = "edaisong";
            clienter.Password = MD5Helper.MD5(clienter.Password);
            clienter.Status = ConstValues.CLIENTER_AUDITPASS;
            return Json(new ResultModel(iDistributionProvider.AddClienter(clienter), ""));
            //return Json(new ResultModel(ClienterLogic.clienterLogic().Add(clienter), ""));
        }
    }
}