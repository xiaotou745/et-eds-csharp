using System.Threading.Tasks;
using ETS.Data.PageData;
using Ets.Model.DataModel.Message;
using Ets.Model.ParameterModel.Message;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.Message;
using Ets.Service.Provider.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Service.Provider.Message;

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
            PageInfo<MessageModel> models = messageProvider.WebList(new WebListSearch());
            return View(models);
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

        ///// <summary>
        /////  批量导入电话号码
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult PhoneNoImport()
        //{
        //    Stream fs = null;
        //    try
        //    {
        //        if (Request.Files["file1"] != null && Request.Files["file1"].FileName != "")
        //        {
        //            HttpPostedFileBase file = Request.Files["file1"];
        //            fs = file.InputStream;
        //            var dtPhoneNo=ExcelHelperNew.GetExcelDataToDataTable(fs);
        //            if(dtPhoneNo!=null&&dtPhoneNo.Rows.Count>0)
        //            {
        //                //dtPhoneNo.Columns[0] == "手机号码";
        //                //if(dtPhoneNo[0][0]!="手机号码")
        //                //{

        //                //}
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        fs.Close();
        //    }
        //    return Json("");

        //}
    }
}