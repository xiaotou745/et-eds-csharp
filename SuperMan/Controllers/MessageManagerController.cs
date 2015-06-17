using System.Threading.Tasks;
using ETS.Data.PageData;
using ETS.Enums;
using ETS.Extension;
using Ets.Model.DataModel.Message;
using Ets.Model.ParameterModel.Message;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.Message;
using Ets.Service.Provider.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Service.Provider.Message;
using Ets.Model.DomainModel.Message;
using SuperMan.App_Start;

namespace SuperMan.Controllers
{
    public class MessageManagerController : Controller
    {
        IAreaProvider iAreaProvider = new AreaProvider();
        private readonly IMessageProvider messageProvider = new MessageProvider();
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
        /// 列表页 add by caoheyang 20150616
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> List()
        {
            ListSetSelect();
            //默认全部
            PageInfo<MessageModel> models = messageProvider.WebList(new WebListSearch(){ MessageType=-1,SendType=-1,SentStatus=-1,PushWay=-1});
            return View(models);
        }

        public async void ListSetSelect()
        {
            EnumItem item = new EnumItem()
            {
                Text = "全部",
                Value = -1
            };
            //推送方式
            var pushWaySelect = EnumExtenstion.GetEnumItems(typeof(MessagePushWay)).ToList();
            pushWaySelect.Insert(0,item);
            ViewData["PushWaySelect"] = new SelectList(pushWaySelect, "Value", "Text");

            //消息类型
            var messageTypeSelect = EnumExtenstion.GetEnumItems(typeof(MessageMessageType)).ToList();
            messageTypeSelect.Insert(0,item);
            ViewData["MessageTypeSelect"] = new SelectList(messageTypeSelect, "Value", "Text");

            //状态
            var sentStatusSelect = EnumExtenstion.GetEnumItems(typeof(MessageSentStatus)).ToList();
            sentStatusSelect.Insert(0,item);
            ViewData["SentStatusSelect"] = new SelectList(sentStatusSelect, "Value", "Text");

            //推送类型
            var sendTypeSelect = EnumExtenstion.GetEnumItems(typeof(MessageSendType)).ToList();
            sendTypeSelect.Insert(0,item);
            ViewData["SendTypeSelect"] = new SelectList(sendTypeSelect, "Value", "Text");
        }

        /// <summary>
        /// 列表页异步加载区域 add by caoheyang 20150616
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostList(int pageindex = 1)
        {
            WebListSearch search = new WebListSearch();
            TryUpdateModel(search);
            PageInfo<MessageModel> models = messageProvider.WebList(search);
            return View(models);
        }

        /// <summary>
        ///  批量导入电话号码
        ///  danny-20150617
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
                    if (dsPhoneNo != null)
                    {
                        var dtPhoneNo = dsPhoneNo.Tables[0];
                        if (dtPhoneNo != null && dtPhoneNo.Rows.Count > 0)
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
            return Json(new Ets.Model.Common.ResultModel(reg, reg ? strPhoneNo : strMsg), JsonRequestBehavior.DenyGet);
        }

        

        /// <summary>
        /// 编辑消息任务（新增和修改公用）
        /// danny-20150617
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditMessageTask(MessageModelDM model)
        {
            model.OptUserName = UserContext.Current.Name;
            var reg = messageProvider.EditMessageTask(model);
            return Json(new Ets.Model.Common.ResultModel(reg.DealFlag, reg.DealMsg), JsonRequestBehavior.DenyGet);
        }
    }
}