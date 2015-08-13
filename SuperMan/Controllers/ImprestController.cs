using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ETS.Data.PageData;
using ETS.Enums;
using Ets.Model.DataModel.Finance;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Finance;
using ETS.Util;
using NPOI.SS.Formula.Functions;
using SuperMan.App_Start;

namespace SuperMan.Controllers
{
    /// <summary>
    /// 备用金controller 
    /// </summary>
    public class ImprestController : BaseController
    {
        private readonly IImprestBalanceRecordProvider imprestProvider = new ImprestBalanceRecordProvider();
        private readonly IImprestRechargeProvider imprestRechargeProvider = new ImprestRechargeProvider();
        /// <summary>
        /// 备用金充值列表页  add by caoheyang  20150812
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImprestRechargeList()
        {
            ViewBag.ImprestRecharge = imprestRechargeProvider.GetRemainingAmountLock();
            ViewBag.PageModels = imprestProvider.GetImprestBalanceRecordList(new ImprestBalanceRecordSearchCriteria()
            {
                OptType = ImprestBalanceRecordOptType.Recharge.GetHashCode()
            });
            return View();
        }

        /// <summary>
        /// 备用金充值列表页  异步加载区域 add by caoheyang  20150812
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoImprestRechargeList(int pageindex = 1)
        {
            var criteria = new ImprestBalanceRecordSearchCriteria();
            TryUpdateModel(criteria);
            criteria.OptType = ImprestBalanceRecordOptType.Recharge.GetHashCode();
            return PartialView(imprestProvider.GetImprestBalanceRecordList(criteria));
        }

        /// <summary>
        /// 备用金充值功能页面 add by caoheyang  20150812
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImprestRecharge()
        {
            return PartialView();
        }

        /// <summary>
        /// 备用金充值功能 保存按钮  add by caoheyang  20150812
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AjaxImprestRecharge(ImprestBalanceRecord model)
        {
            model.OptName = UserContext.Current.Name;
            model.OptType = ImprestBalanceRecordOptType.Recharge.GetHashCode();
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                Data = imprestProvider.AjaxImprestRecharge(model)
            };
        }

        /// <summary>
        /// 备用金支出列表页 
        /// 彭宜
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImprestPaymentList()
        {
            var criteria = new ImprestBalanceRecordSearchCriteria()
            {
                OptType = ImprestBalanceRecordOptType.Payment.GetHashCode(),
            };
            var pagedList = imprestProvider.GetImprestBalanceRecordList(criteria);
            return View(pagedList);
        }

        /// <summary>
        /// 备用金支出列表页     异步加载区域
        /// 彭宜
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoImprestPaymentList(int pageindex = 1)
        {
            var criteria = new ImprestBalanceRecordSearchCriteria();
            TryUpdateModel(criteria);
            criteria.OptType = ImprestBalanceRecordOptType.Payment.GetHashCode();
            var pagedList = imprestProvider.GetImprestBalanceRecordList(criteria);
            return PartialView(pagedList);
        }

        /// <summary>
        /// 导出备用金支出列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpGet]
        public void PostDaoChuImprestPayment(int pageindex = 1)
        {
            ImprestBalanceRecordSearchCriteria criteria = new ImprestBalanceRecordSearchCriteria();
            TryUpdateModel(criteria);
            criteria.PageIndex = 1;
            criteria.PageSize = 65534;

            var pagedList = imprestProvider.GetImprestBalanceRecordList(criteria);

            var buffer = new byte[0] {};
            Response.ContentType = "application/msexcel";
            Response.Clear();
            Response.BufferOutput = true;
            string filname = "无数据.xls";
            if (pagedList != null && pagedList.Records.Count > 0)
            {
                filname = "e代送-{0}-备用金支出数据.xls";
                if (!string.IsNullOrWhiteSpace(criteria.ClienterPhoneNo))
                {
                    filname = string.Format(filname, criteria.ClienterPhoneNo);
                }
                if (!string.IsNullOrWhiteSpace(criteria.OptDateStart))
                {
                    filname = string.Format(filname, criteria.OptDateStart + ":" + criteria.OptDateEnd);
                }
                if (pagedList.Records.Count > 3)
                {
                    byte[] data = Encoding.UTF8.GetBytes(CreateExcel(pagedList));
                    buffer = data;
                }
                else
                {
                    byte[] data = Encoding.Default.GetBytes(CreateExcel(pagedList));
                    buffer = data;
                }
            }
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}",filname));
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.End();
            //return File(new byte[0] { }, "application/msexcel", "无数据.xls");
        }

        /// <summary>
        /// 支出备用金验证骑士手机号获取信息
        /// 2015年8月12日16:55:56
        /// 茹化肖
        /// </summary>
        /// <returns></returns>
         [HttpPost]
        public JsonResult CheckPhoneNum()
        {
            string phonenum = System.Web.HttpContext.Current.Request["PhoneNum"];
            ImprestClienterModel model = imprestProvider.ClienterPhoneCheck(phonenum);
            return Json(model);
        }

         /// <summary>
         /// 支出备用金验证骑士手机号获取信息
         /// 2015年8月12日16:55:56
         /// 茹化肖
         /// </summary>
         /// <returns></returns>
         [HttpPost]
         public JsonResult ClienterWithdrawOk()
         {
             ImprestWithdrawModel parModel=new ImprestWithdrawModel();
             TryUpdateModel(parModel);
             parModel.OprName = UserContext.Current.Name;
             ImprestPayoutModel model = imprestProvider.ClienterWithdrawOk(parModel);
             return Json(model);
         }

         /// <summary>
         /// 生成excel文件
         /// 导出字段：骑士姓名、电话、支出金额、日期、操作人、备注
         /// 彭宜
         /// </summary>
         /// <returns></returns>
         private string CreateExcel(PageInfo<ImprestBalanceRecord> paraModel)
         {
             StringBuilder strBuilder = new StringBuilder();
             strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
             //输出表头.
             strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
             strBuilder.AppendLine("<td>骑士姓名</td>");
             strBuilder.AppendLine("<td>电话</td>");
             strBuilder.AppendLine("<td>支出金额</td>");
             strBuilder.AppendLine("<td>日期</td>");
             strBuilder.AppendLine("<td>操作人</td>");
             strBuilder.AppendLine("<td>备注</td>");
             strBuilder.AppendLine("</tr>");
             //输出数据.
             foreach (var record in paraModel.Records)
             {
                 strBuilder.AppendLine(string.Format("<tr><td>{0}</td>", record.ClienterName));
                 strBuilder.AppendLine(string.Format("<td>{0}</td>", record.ClienterPhoneNo));
                 strBuilder.AppendLine(string.Format("<td>{0}</td>", record.Amount));
                 strBuilder.AppendLine(string.Format("<td>{0}</td>", record.OptTime));
                 strBuilder.AppendLine(string.Format("<td>{0}</td>", record.OptName));
                 strBuilder.AppendLine(string.Format("<td>{0}</td>", record.Remark));
             }
             strBuilder.AppendLine("</table>");
             return strBuilder.ToString();
         }
    }
}