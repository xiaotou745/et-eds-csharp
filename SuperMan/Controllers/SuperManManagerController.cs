using System.Text.RegularExpressions;
using Ets.Model.DataModel.Clienter;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.Subsidy;
using Ets.Service.Provider.WtihdrawRecords;
using SuperMan.App_Start;
using SuperManBusinessLogic.C_Logic;
using SuperManCommonModel;
using SuperManCommonModel.Entities;
using SuperManCore;
using SuperManCore.Common;
using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Service.Provider.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Model.ParameterModel.Finance;
using System.Text;

namespace SuperMan.Controllers
{
    [WebHandleError]
    public class SuperManManagerController : BaseController
    {
        Ets.Service.IProvider.Distribution.IDistributionProvider iDistributionProvider = new DistributionProvider();
        ClienterProvider cliterProvider = new ClienterProvider();
        IClienterFinanceProvider iClienterFinanceProvider = new ClienterFinanceProvider();
        IAreaProvider iAreaProvider = new AreaProvider();
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
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity();
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria() {  Status = -1, GroupId = SuperMan.App_Start.UserContext.Current.GroupId };
            var pagedList = iDistributionProvider.GetClienteres(criteria);
            return View(pagedList);
        }
           
 
        [HttpPost]
        public ActionResult PostSuperManManager(int pageindex=1)
        {
            Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria();
            TryUpdateModel(criteria);
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(); 
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
            if (cliterProvider.CheckClienterExistPhone(clienter.PhoneNo))  //判断该手机号是否已经注册过
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
            if (Price >= 0)
            {
                return Json(new ResultModel(false, "提现失败，提现金额错误"), JsonRequestBehavior.AllowGet);
            }
            if ((0-Price) < 1000)
            {
                return Json(new ResultModel(false, "提现失败，提现金额需大于1000元"), JsonRequestBehavior.AllowGet);
            }
            var model = new Ets.Model.ParameterModel.WtihdrawRecords.WithdrawRecordsModel()
            {
                AdminId = UserContext.Current.Id,
                Amount = Price,
                Balance = 0,
                Platform = 1,
                UserId = UserId
            };
            bool checkWithdraw = new WtihdrawRecordsProvider().AddWtihdrawRecords(model);
            return Json(checkWithdraw ? new ResultModel(true, "提现成功") : new ResultModel(false, "提现失败"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取当前配送员的跨店奖励信息
        /// 平扬
        /// 2015年4月23日
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public ActionResult CrossShopLog(int UserId)
        {
            var pagedList = new SubsidyProvider().GetCrossShopListByCid(UserId); 
            ViewBag.pagedList = pagedList;
            return View();
        }

        /// <summary>
        /// 查看骑士详细信息
        /// danny-20150513
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult ClienterDetail(string clienterId)
        {

            var clienterWithdrawFormModel = cliterProvider.GetClienterDetailById(clienterId);
            var criteria = new ClienterBalanceRecordSerchCriteria()
            {
                ClienterId = Convert.ToInt32(clienterId)
            };
            ViewBag.clienterBalanceRecord = iClienterFinanceProvider.GetClienterBalanceRecordList(criteria);
            return View(clienterWithdrawFormModel);
        }

        /// <summary>
        /// 查看骑士余额流水记录
        /// danny-20150513
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult ClienterBalanceRecord(ClienterBalanceRecordSerchCriteria criteria)
        {
            ViewBag.clienterBalanceRecord = iClienterFinanceProvider.GetClienterBalanceRecordList(criteria);
            return PartialView("_ClienterBalanceRecordList");
        }
        /// <summary>
        /// 导出骑士余额流水记录
        /// danny-20150513
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult ExportClienterBalanceRecord()
        {
            var criteria = new Ets.Model.ParameterModel.Finance.ClienterBalanceRecordSerchCriteria();
            TryUpdateModel(criteria);
            var dtClienterBalanceRecord = iClienterFinanceProvider.GetClienterBalanceRecordListForExport(criteria);
            if (dtClienterBalanceRecord != null && dtClienterBalanceRecord.Count > 0)
            {

                string filname = "骑士提款流水记录{0}.xls";
                if (!string.IsNullOrWhiteSpace(criteria.OperateTimeStart))
                {
                    filname = string.Format(filname, criteria.OperateTimeStart + "~" + criteria.OperateTimeEnd);
                }
                byte[] data = Encoding.UTF8.GetBytes(iClienterFinanceProvider.CreateClienterBalanceRecordExcel(dtClienterBalanceRecord.ToList()));
                return File(data, "application/ms-excel", filname);
            }
            else
            {

                var clienterWithdrawFormModel = cliterProvider.GetClienterDetailById(criteria.ClienterId.ToString());
                var criteriaNew = new ClienterBalanceRecordSerchCriteria()
                {
                    ClienterId = Convert.ToInt32(criteria.ClienterId)
                };
                ViewBag.clienterBalanceRecord = iClienterFinanceProvider.GetClienterBalanceRecordList(criteriaNew);
                return View("ClienterDetail", clienterWithdrawFormModel);
            }
        }

    }
}