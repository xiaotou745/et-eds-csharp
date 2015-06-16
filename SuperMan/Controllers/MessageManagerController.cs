using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class MessageManagerController : Controller
    {
        IAreaProvider iAreaProvider = new AreaProvider();
        // GET: MessageManager
        public ActionResult MessageManager()
        {
            return View();
        }
        /// <summary>
        /// 消息编辑（添加和修改）
        /// danny-20150615
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult MessageEdit()
        {
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(0);
            return View();
        }
        /// <summary>
        ///  批量导入电话号码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PhoneNoImport()
        {
            Stream fs = null;
            string strPhoneNo = "";
            string strMsg = "";
            bool reg = false;
            try
            {
                if (Request.Files["file1"] != null && Request.Files["file1"].FileName != "")
                {
                    HttpPostedFileBase file = Request.Files["file1"];
                    fs = file.InputStream;
                    var dsPhoneNo = NPOIHelper.ImportExceltoDs(fs);
                    if(dsPhoneNo!=null)
                    {
                        var dtPhoneNo = dsPhoneNo.Tables[0];
                        if(dtPhoneNo!=null&&dtPhoneNo.Rows.Count>0)
                        {
                            dtPhoneNo = dtPhoneNo.DefaultView.ToTable(true, new[] { "手机号码" });
                            foreach (DataRow item in dtPhoneNo.Rows)
                            {
                                if (!string.IsNullOrWhiteSpace(item[0].ToString()))
                                {
                                    strPhoneNo += item[0].ToString() + ",";
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(strPhoneNo))
                            {
                                strPhoneNo = strPhoneNo.TrimEnd(',');
                                reg = true;
                            }
                        }
                        else
                        {
                            strMsg = "未获取到电话号码！";
                        }
                        
                    }
                    else
                    {
                        strMsg = "未获取到电话号码！";
                    }
                }
                else
                {
                    strMsg = "选择文件有误！";
                }
            }
            catch (Exception ex)
            {
                fs.Close();
                strMsg = ex.Message;
            }
            return Json(new Ets.Model.Common.ResultModel(reg, reg ? strPhoneNo:strMsg), JsonRequestBehavior.DenyGet);
        }
    }
}