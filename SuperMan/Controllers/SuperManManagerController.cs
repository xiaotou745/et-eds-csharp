using SuperMan.Models;
using SuperManBusinessLogic.B_Logic;
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
            var criteria = new ClienterSearchCriteria() { PagingRequest = new PagingResult(0, 15), Status = -1, GroupId = account.GroupId };
            var pagedList = ClienterLogic.clienterLogic().GetClienteres(criteria);
            PagingResult pr = new PagingResult() { PageIndex = 0, PageSize = 1000 };
            var business = BusiLogic.busiLogic().GetBusinesses(new BusinessSearchCriteria() { PagingRequest = pr, Status = -1 });
            ViewBag.AllBusiness = business;
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult SuperManManager(ClienterSearchCriteria criteria)
        {
            var pagedList = ClienterLogic.clienterLogic().GetClienteres(criteria);
            PagingResult pr = new PagingResult() { PageIndex = 0, PageSize = 1000 };
            var business = BusiLogic.busiLogic().GetBusinesses(new BusinessSearchCriteria() { PagingRequest = pr, Status = -1 });
            ViewBag.AllBusiness = business;
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
        /// 注册超人
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddSuperMan(ClienterModel clienterModel)
        {
            if (ClienterLogic.clienterLogic().CheckExistPhone(clienterModel.PhoneNo))  //判断该手机号是否已经注册过
                return Json(new ResultModel(false, "手机号已被注册"));
            if (string.IsNullOrWhiteSpace(clienterModel.Password))
                clienterModel.Password = "edaisong";
            clienterModel.Password = MD5Helper.MD5(clienterModel.Password);
            clienterModel.Status = ConstValues.CLIENTER_AUDITPASS;
            clienter model = new SuperManDataAccess.clienter
            {
                TrueName = clienterModel.TrueName,
                PhoneNo = clienterModel.PhoneNo,
                HealthCardID = clienterModel.HealthCardID,
                InternalDepart = clienterModel.InternalDepart,
                ProvinceCode = clienterModel.ProvinceCode,
                CityCode = clienterModel.CityCode,
                IDCard = clienterModel.IDCard,
                Password = clienterModel.Password,
                City = clienterModel.City,
                CityId = clienterModel.CityId,
                Province = clienterModel.Province,
                GroupId = clienterModel.GroupId,
                BussinessID = clienterModel.BussinessID,
                PicWithHandUrl = clienterModel.PicWithHandUrl,
                PicUrl = clienterModel.PicUrl,
                Status = ConstValues.CLIENTER_NOAUDIT
            };
            bool result = true;
            int clienterId = ClienterLogic.clienterLogic().AddClienter(model);
            if (clienterId > 0)
            {
                if (!string.IsNullOrWhiteSpace(clienterModel.CheckBusiList))
                {
                    var list = clienterModel.CheckBusiList.Split(',');
                    foreach (var item in list)
                    {
                        clienterbussinessrelation cbr = new clienterbussinessrelation
                        {
                            ClienterId = clienterId,
                            BussinessID = Convert.ToInt32(item)
                        };
                        if (!ClienterLogic.clienterLogic().AddClienterBusiRel(cbr))
                        {
                            result = false;
                        }
                    }
                }
            }
            else
            {
                result = false;
            }
            if (result)
            {
                return Json(new ResultModel(true, "成功"));
            }
            return Json(new ResultModel(result, "添加超人失败，请重试!"));
        }

        /// <summary>
        /// 修改超人信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifySuperMan(ClienterModel clienterModel)
        {
            if (!string.IsNullOrWhiteSpace(clienterModel.Password.Trim()))
            {
                clienterModel.Password = MD5Helper.MD5(clienterModel.Password);
            }

            clienter model = new SuperManDataAccess.clienter
            {
                Id=clienterModel.Id,
                TrueName = clienterModel.TrueName,
                PhoneNo = clienterModel.PhoneNo,
                HealthCardID = clienterModel.HealthCardID,
                InternalDepart = clienterModel.InternalDepart,
                ProvinceCode = clienterModel.ProvinceCode,
                CityCode = clienterModel.CityCode,
                IDCard = clienterModel.IDCard,
                Password = clienterModel.Password,
                City = clienterModel.City,
                CityId = clienterModel.CityId,
                Province = clienterModel.Province,
                GroupId = clienterModel.GroupId,
                BussinessID = clienterModel.BussinessID,
                PicWithHandUrl = clienterModel.PicWithHandUrl,
                PicUrl = clienterModel.PicUrl
            };
            ClienterLogic.clienterLogic().Update(model);
            var delcbr = new clienterbussinessrelation {ClienterId = clienterModel.Id};
            ClienterLogic.clienterLogic().DeleteClienterBusiRel(delcbr);
            if (!string.IsNullOrWhiteSpace(clienterModel.CheckBusiList))
            {
                var list = clienterModel.CheckBusiList.Split(',');
                foreach (var item in list)
                {
                    var cbr = new clienterbussinessrelation
                    {
                        ClienterId = clienterModel.Id,
                        BussinessID = Convert.ToInt32(item)
                    };
                    ClienterLogic.clienterLogic().AddClienterBusiRel(cbr);
                }
            }
            return Json(new ResultModel(true, "成功"));
        }

        /// <summary>
        /// 获取骑士和商家的对应关系 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetClienterBussinessRel(int id)
        {

            var kk = SuperManBusinessLogic.B_Logic.BusiLogic.busiLogic().GetClienterBusinessRelList(id);
            ViewBag.ClienterBusiRel = kk;
            return Json(kk, JsonRequestBehavior.DenyGet);
        }
        
    }
}