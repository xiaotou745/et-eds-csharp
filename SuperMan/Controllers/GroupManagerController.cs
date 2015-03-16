using System;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.DataModel.Group;
using Ets.Model.ParameterModel.Group;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using Newtonsoft.Json.Linq;
using SuperManDataAccess;
namespace SuperMan.Controllers
{
    [Authorize]
    [WebHandleError]
    public class GroupManagerController : Controller
    {
        /// <summary>
        /// 集团业务操作类
        /// </summary>
        readonly IGroupProvider iGroupServices=new GroupProvider();
        // GET: BusinessManager
        public ActionResult GroupManager()
        {
            account account = HttpContext.Session["user"] as account;
            if (account == null)
            {
                Response.Redirect("/account/login");
                return null;
            }
            var criteria = new GroupParaModel() { PagingRequest = new PagingResult(1, 15) };
            var pagedList = iGroupServices.GetGroupList(criteria);
            return View(pagedList.Data);
        }

        /// <summary>
        /// 查询集团信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GroupManager(GroupParaModel criteria)
        {
            var result = iGroupServices.GetGroupList(criteria);
            if (result.Result)
            {
                return PartialView("_GroupManageList", result.Data);
            }
            return PartialView("_GroupManageList", null);
        }

        /// <summary>
        /// 添加集团信息
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddGroup(string groupname)
        {
            if (string.IsNullOrEmpty(groupname))
            {
                return Json(new ResultModel(false, "集团名称不能为空")); 
            }
            if (HttpContext.Session != null)
            {
                var account = HttpContext.Session["user"] as account;
                var mode=new GroupModel {GroupName = groupname.Trim(), CreateName = account.LoginName, CreateTime = DateTime.Now};
                var result = iGroupServices.HasExistsGroup(mode);
                if (result.Result)
                {
                    if (result.Data)
                    {
                        return Json(new ResultModel(false, "集团名称已经存在"));
                    }
                }
                else
                {
                    return Json(new ResultModel(false, "服务器异常"));
                }
                mode.IsValid = 1;
                var res = iGroupServices.AddGroup(mode);
                return Json(res.Result ? new ResultModel(true, "成功") : new ResultModel(false, "服务器异常"));
            }
            else
            {
                Response.Redirect("/account/login");
                return null;
            }
        }

    }
}