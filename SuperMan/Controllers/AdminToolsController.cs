using Ets.Model.Common;
using Ets.Model.DomainModel.Common;
using Ets.Model.ParameterModel.Common;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using System.Web;
using System.Web.Mvc;
using SuperMan.App_Start;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;
namespace SuperMan.Controllers
{
    public class AdminToolsController : BaseController
    {
        readonly IBusinessGroupProvider iBusinessGroupProvider = new BusinessGroupProvider();
        readonly IAppVersionProvider iAppVersionProvider = new AppVersionProvider();
        private static IAdminToolsProvider adminToolsProvider
        {
            get { return new AdminToolsProvider(); }
        }
        // GET: AdminTools 
        public ActionResult AdminTools(string strSql)
        {
            if (!string.IsNullOrWhiteSpace(strSql))
            {
                var data = adminToolsProvider.GetDataInfoBySql(strSql.Trim());
                ViewBag.SQL = strSql;
                ViewBag.Quantity = data.Rows.Count;
                ViewBag.Data = data;
                return PartialView("_AdminToolsList");
            }
            ViewBag.Quantity = 0;
            ViewBag.Data = null;
            return View();
        }

        // GET: AdminTools
        public ContentResult Edit(string strSql)
        {
            if (!string.IsNullOrEmpty(strSql))
            {
                var data = adminToolsProvider.UpdateDataInfoBySql(strSql.Trim());
                ViewBag.SQL = strSql;
                ViewBag.Data = data;
                return new ContentResult() { Content = data.ToString() };
            }
            ViewBag.Data = null;
            return new ContentResult() { Content = "" };
        }

        /// <summary>
        /// 管理员工具页面 add by caoheyang 20150414
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RedisTools()
        {
            return View();
        }

        /// <summary>
        ///  管理员工具页面 Async add by caoheyang 20150414
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostRedisTools()
        {
            string key = HttpContext.Request.Form["RedisKey"];  
            int searchType=ETS.Util.ParseHelper.ToInt(HttpContext.Request.Form["SearchType"]);
            string searchKey = searchType == 0 ?  "*" + key + "*":key;
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            ViewBag.Keys = redis.Keys(searchKey);
            ViewBag.searchType = searchType;
            return View();
        }
        /// <summary>
        ///  管理员工具页面 Async add by caoheyang 20150414
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteRedisTools(string key)
        {
            try
            {
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                redis.Delete(key);
                return new JsonResult() { Data = new { status = "success" } };
            }
            catch {
                return new JsonResult() { Data = new { status = "error" } };
            }
        }
        /// <summary>
        /// 公共配置管理
        /// danny-20150518
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GlobalConfigManager()
        {
            ViewBag.GloglConfig = new Ets.Service.Provider.GlobalConfig.GlobalConfigProvider().GetGlobalConfig(0);
            return View();
        }
        /// <summary>
        /// 修改公共配置信息
        /// danny-20150518
        /// </summary>
        /// <param name="globalConfigModel"></param>
        /// <returns></returns>
        public ActionResult ModifyGlobalConfig(GlobalConfigModel globalConfigModel)
        {
            globalConfigModel.OptName = UserContext.Current.Name;
            var reg = iBusinessGroupProvider.ModifyGlobalConfig(globalConfigModel);
            return Json(new ResultModel(reg, reg ? "保存成功！" : "保存失败！"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 加载默认APP版本控制列表
        /// danny-20150715
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult APPVersionManager()
        {
            var criteria = new AppVerionSearchCriteria();
            var pagedList = iAppVersionProvider.GetAppVersionList(criteria);
            return View(pagedList);
        }

        /// <summary>
        /// 按条件查询APP版本控制列表
        /// danny-20150715
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostAPPVersionManager(int pageindex = 1)
        {
            var criteria = new AppVerionSearchCriteria();
            TryUpdateModel(criteria);
            var pagedList = iAppVersionProvider.GetAppVersionList(criteria);
            return PartialView("_APPVersionList", pagedList);
        }
        /// <summary>
        ///添加APP版本控制
        /// danny-20150715
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult APPVersionEdit()
        {
            return View();
        }
        /// <summary>
        /// App版本详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult APPVersionDetail(int Id)
        {
            var appVersionDetailModel = iAppVersionProvider.GetAppVersionById(Id);
            ViewBag.dealType = 1;//修改
            return View("APPVersionEdit", appVersionDetailModel);
        }
       
        /// <summary>
        /// 编辑App版本信息（新增和修改公用）
        /// danny-20150715
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditAPPVersion(AppVersionModel model)
        {
            model.OptUserName = UserContext.Current.Name;
            var reg = iAppVersionProvider.EditAppVersion(model);
            return Json(new Ets.Model.Common.ResultModel(reg.DealFlag, reg.DealMsg), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 编辑App版本信息（新增和修改公用）
        /// danny-20150715
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelAPPVersion(AppVersionModel model)
        {
            model.OptUserName = UserContext.Current.Name;
            var reg = iAppVersionProvider.CancelAppVersion(model);
            return Json(new Ets.Model.Common.ResultModel(reg, reg?"取消成功！":"取消失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// App版本详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ViewAPPVersionDetail(int Id)
        {
            var appVersionDetailModel = iAppVersionProvider.GetAppVersionById(Id);
            return View("APPVersionDetail", appVersionDetailModel);
        }
    }
}
