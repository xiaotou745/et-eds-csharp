using System.Text.RegularExpressions;
using Ets.Model.DataModel.Clienter;
using Ets.Model.ParameterModel.Business;
using Ets.Service.IProvider.Bussiness;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Bussiness;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.Subsidy;
using Ets.Service.Provider.WtihdrawRecords;
using ETS.Util;
using SuperMan.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Service.Provider.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Model.ParameterModel.Finance;
using System.Text;
using ETS.Util;  
using Ets.Model.Common;
namespace SuperMan.Controllers
{
    [WebHandleError]
    public class SuperManManagerController : BaseController
    {
        readonly Ets.Service.IProvider.Distribution.IDistributionProvider iDistributionProvider = new DistributionProvider();
        readonly ClienterProvider cliterProvider = new ClienterProvider();
        readonly IClienterFinanceProvider iClienterFinanceProvider = new ClienterFinanceProvider();
        readonly IAreaProvider iAreaProvider = new AreaProvider();

        private readonly IBusinessClienterRelationProvider iBusinessClienterRelationProvider =
            new BusinessClienterRelationProvider();
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

            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id

            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria()
            {
                Status = -1,
                GroupId = SuperMan.App_Start.UserContext.Current.GroupId,
                AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType)

            };
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return View();
            }
            //ViewBag.openCityList.Result.AreaModels;
            var pagedList = iDistributionProvider.GetClienteres(criteria);
            return View(pagedList);
        }


        [HttpPost]
        public ActionResult PostSuperManManager(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria();
            TryUpdateModel(criteria);

            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id

            criteria.AuthorityCityNameListStr =
                iAreaProvider.GetAuthorityCityNameListStr(ParseHelper.ToInt(UserType));
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(UserType);
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return PartialView("_SuperManManagerList");
            }
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
            if ((0 - Price) < 1000)
            {
                return Json(new ResultModel(false, "提现失败，提现金额需大于1000元"), JsonRequestBehavior.AllowGet);
            }
            var model = new Ets.Model.ParameterModel.WtihdrawRecords.WithdrawRecordsModel()
            {
                AdminId = UserContext.Current.Id,
                Amount = Price, //提现金额
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

        ///// <summary>
        ///// 查看骑士详细信息
        ///// danny-20150513
        ///// </summary>
        ///// <param name="clienterId">骑士Id</param>
        ///// <returns></returns>
        //public ActionResult ClienterDetail(string clienterId)
        //{

        //    var clienterWithdrawFormModel = cliterProvider.GetClienterDetailById(clienterId);
        //    var criteria = new ClienterBalanceRecordSerchCriteria()
        //    {
        //        ClienterId = Convert.ToInt32(clienterId)
        //    };
        //    ViewBag.clienterBalanceRecord = iClienterFinanceProvider.GetClienterBalanceRecordList(criteria);
        //    return View(clienterWithdrawFormModel);
        //}

        /// <summary>
        /// 查看骑士详细信息
        /// danny-20150513
        /// </summary>
        /// <param name="clienterId">骑士Id</param>
        /// <returns></returns>
        public ActionResult ClienterDetail(string clienterId)
        {

            var clienterWithdrawFormModel = cliterProvider.GetClienterDetailById(clienterId);
            var criteria = new ClienterBalanceRecordSerchCriteria()
            {
                ClienterId = Convert.ToInt32(clienterId)
            };
            ViewBag.clienterBalanceRecord = iClienterFinanceProvider.GetClienterBalanceRecordListOfPaging(criteria);
            return View(clienterWithdrawFormModel);
        }

        /// <summary>
        /// 查看骑士余额流水记录
        /// danny-20150513
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ActionResult ClienterBalanceRecord(ClienterBalanceRecordSerchCriteria criteria)
        {
            ViewBag.clienterBalanceRecord = iClienterFinanceProvider.GetClienterBalanceRecordList(criteria);
            return PartialView("_ClienterBalanceRecordList");
        }
        /// <summary>
        /// 查看骑士余额流水记录分页版
        /// danny-20150604
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostClienterBalanceRecord(int pageindex = 1)
        {
            var criteria = new ClienterBalanceRecordSerchCriteria();
            TryUpdateModel(criteria);
            ViewBag.clienterBalanceRecord = iClienterFinanceProvider.GetClienterBalanceRecordListOfPaging(criteria);
            return PartialView("_ClienterBalanceRecordList");
        }
        /// <summary>
        /// 导出骑士余额流水记录
        /// danny-20150513
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportClienterBalanceRecord()
        {
            var criteria = new ClienterBalanceRecordSerchCriteria();
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


        /// <summary>
        /// 骑士余额变更        
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessRecharge(ClienterOptionLog model)
        {
            model.OptName = UserContext.Current.Name;
            var reg = iClienterFinanceProvider.ClienterRecharge(model);
            return Json(new ResultModel(reg, reg ? "骑士余额变更成功！" : "骑士余额变更失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 根据骑士Id获取收支记录
        /// danny-20150604
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        public ActionResult GetClienterBalanceRecordListOfPaging(string clienterId)
        {
            var criteria = new ClienterBalanceRecordSerchCriteria()
            {
                ClienterId = Convert.ToInt32(clienterId)
            };
            var pagedList = iClienterFinanceProvider.GetClienterBalanceRecordListOfPaging(criteria);
            return View(pagedList);
        }
        /// <summary>
        /// 根据骑士Id获取收支记录分页版
        /// danny-20150604
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        public ActionResult PostGetClienterBalanceRecordListOfPaging(string clienterId)
        {
            var criteria = new ClienterBalanceRecordSerchCriteria()
            {
                ClienterId = Convert.ToInt32(clienterId)
            };
            var pagedList = iClienterFinanceProvider.GetClienterBalanceRecordListOfPaging(criteria);
            return View(pagedList);
        }

        /// <summary>
        /// 根据骑士id查询骑士绑定商家列表   caoheyang 20150608
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRelationByClienterId()
        {
            BCRelationGetByClienterIdPM model = new BCRelationGetByClienterIdPM();
            TryUpdateModel(model);
            ViewBag.Name = model.Name;
            ViewBag.Phone = model.Phone;
            return View(iBusinessClienterRelationProvider.GetByClienterId(model));
        }

        /// <summary>
        /// 根据骑士id查询骑士绑定商家列表   caoheyang 20150608
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostGetRelationByClienterId(BCRelationGetByClienterIdPM model)
        {
            return View(iBusinessClienterRelationProvider.GetByClienterId(model));
        }
    }
}