using Ets.Model.DataModel.Clienter;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.WtihdrawRecords;
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
    public class SuperManManagerController : BaseController
    {
        Ets.Service.IProvider.Distribution.IDistributionProvider iDistributionProvider = new DistributionProvider();

        ClienterProvider cliterProvider = new ClienterProvider();
        // GET: BusinessManager
        public ActionResult SuperManManager()
        {
            //account account = HttpContext.Session["user"] as account;
            //if (account == null)
            //{
            //    Response.Redirect("/account/login");
            //    return null;
            //}
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId; ;//集团id
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria() {  Status = -1, GroupId = SuperMan.App_Start.UserContext.Current.GroupId };
            var pagedList = iDistributionProvider.GetClienteres(criteria);
            return View(pagedList);
        }

        [HttpPost]
        public ActionResult PostSuperManManager(int pageindex=1)
        {
            Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria();
            TryUpdateModel(criteria);
            var pagedList = iDistributionProvider.GetClienteres(criteria);
            return PartialView("_SuperManManagerList", pagedList);
        }

        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuditOK(int id)
        {
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
            bool b = iDistributionProvider.ClearSuperManAmount(id);
            if (b)
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
            if (iDistributionProvider.CheckExistPhone(clienter.PhoneNo))  //判断该手机号是否已经注册过
                return Json(new ResultModel(false, "手机号已被注册"));
            if (string.IsNullOrWhiteSpace(clienter.Password))
                clienter.Password = "edaisong";
            clienter.Password = MD5Helper.MD5(clienter.Password);
            clienter.Status = ConstValues.CLIENTER_AUDITPASS;
            return Json(new ResultModel(iDistributionProvider.AddClienter(clienter), ""));
        }


        /// <summary>
        /// 获取当前配送员的流水信息
        /// 窦海超
        /// 2015年3月20日 17:12:11
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public ActionResult WtihdrawRecords(int UserId)
        {
            //account maccount = HttpContext.Session["user"] as account;
            //if (maccount == null)
            //{
            //    Response.Redirect("/account/login");
            //    return null;
            //}
            var pagedList = cliterProvider.WtihdrawRecords(UserId);
            ViewBag.pagedList = pagedList;
            ViewBag.UserId = UserId;
            return View();
        }

        /// <summary>
        /// 提现，并增加流水日志
        /// 窦海超
        /// 2015年3月23日 08:58:11
        /// </summary>
        /// <param name="Price">金额</param>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WtihdrawRecords(decimal Price, int UserId)
        {
            account maccount = HttpContext.Session["user"] as account;
            if (maccount == null)
            {
                return Json(new ResultModel(false, "提现失败，需要重新登录"), JsonRequestBehavior.AllowGet);
            }
            if (Price >= 0)
            {
                return Json(new ResultModel(false, "提现失败，金额不足"), JsonRequestBehavior.AllowGet);
            }
            if ((0-Price) < 1000)
            {
                return Json(new ResultModel(false, "提现失败，提现金额需大于1000元"), JsonRequestBehavior.AllowGet);
            }
            int adminId = maccount == null ? 0 : maccount.Id;
            Ets.Model.ParameterModel.WtihdrawRecords.WithdrawRecordsModel model = new Ets.Model.ParameterModel.WtihdrawRecords.WithdrawRecordsModel()
            {
                AdminId = adminId,
                Amount = Price,
                Balance = 0,
                Platform = 1,
                UserId = UserId
            };

            WtihdrawRecordsProvider withdrawRecords = new WtihdrawRecordsProvider();
            bool checkWithdraw = withdrawRecords.AddWtihdrawRecords(model);
            if (checkWithdraw)
            {
                return Json(new ResultModel(true, "提现成功"), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel(false, "提现失败"), JsonRequestBehavior.AllowGet);
        }
    }
}