using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Model.DataModel.Common;
using Ets.Model.ParameterModel.Common;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using ETS.Util;

namespace SuperMan.Controllers
{
    /// <summary>
    /// 到处数据管理器   add by caoheyang 20150601
    /// </summary>
    public class DataManagerController : BaseController
    {

        private readonly IExportSqlManageProvider exportSqlManageProvider = new ExportSqlManageProvider();
        
        // GET: DataManager
        /// <summary>
        /// 导出数据等相关首页  add by caoheyang 20150601
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            DataManageSearchCriteria search = new DataManageSearchCriteria();
            var models = exportSqlManageProvider.Query(search);
            return View(models);
        }

        /// <summary>
        /// 导出数据等相关首页  add by caoheyang 20150601
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostIndex(int pageindex = 1)
        {
            DataManageSearchCriteria search = new DataManageSearchCriteria();
            TryUpdateModel(search);
            var models = exportSqlManageProvider.Query(search);
            return View(models);
        }



        /// <summary>
        /// 新增，编辑模板 add by caoheyang 20150601
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(long? id = 0)
        {
            ExportSqlManage model = id == 0 ? new ExportSqlManage() : exportSqlManageProvider.GetById(ParseHelper.ToLong(id));
            return View(model);
        }


        /// <summary>
        /// 导出数据新增sql模板  add by caoheyang 20150601
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(ExportSqlManage model)
        {
            return new JsonResult()
            {
                Data = new { Result = exportSqlManageProvider.Insert(model) > 0 ? "success" : "error" }
            };
        }

        /// <summary>
        /// 修改模板提交方法  add by caoheyang 20150601
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public JsonResult DoEdit(ExportSqlManage model)
        {
            return new JsonResult()
            {
                Data = new { Result = exportSqlManageProvider.Update(model) > 0 ? "success" : "error" }
            };
        }


        // GET: DataManager
        /// <summary>
        /// 导出数据新增sql模板  add by caoheyang 20150601
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(long id)
        {
            return new JsonResult()
            {
                Data = new { Result = exportSqlManageProvider.Delete(id) > 0 ? "success" : "error" }
            };
        }
    }
}