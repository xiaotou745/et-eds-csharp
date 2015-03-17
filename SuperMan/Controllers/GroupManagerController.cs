using System;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.DataModel.Group;
using Ets.Model.ParameterModel.Group;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using SuperManDataAccess;
using Ets.Model.DomainModel.Group;
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
        /// <summary>
        /// 集团管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupManager()
        {
            account account = HttpContext.Session["user"] as account;
            if (account == null)
            {
                Response.Redirect("/account/login");
                return null;
            }
            var criteria = new GroupParaModel() { PagingRequest = new NewPagingResult(0, 6) };
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
            if (HttpContext.Session != null && HttpContext.Session["user"] != null)
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

        /// <summary>
        /// 修改集团信息
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateGroup(int id,string groupname)
        {
            if (string.IsNullOrEmpty(groupname))
            {
                return Json(new ResultModel(false, "集团名称不能为空")); 
            }
            if (HttpContext.Session != null && HttpContext.Session["user"] !=null)
            {
                var account = HttpContext.Session["user"] as account;
                var mode=new GroupModel {Id=id,GroupName = groupname.Trim(), CreateName = account.LoginName, CreateTime = DateTime.Now};
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
                var res = iGroupServices.UpdateGroupName(mode);
                return Json(res.Result ? new ResultModel(true, "成功") : new ResultModel(false, "服务器异常"));
            }
            else
            {
                Response.Redirect("/account/login");
                return null;
            }
        } 

        /// <summary>
        /// 添加集团Api配置信息
        /// </summary>
        /// <param name="model">集团配置实体</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddGroupConfig(GroupApiConfigModel model)
        {
            if (model.GroupId==0)
            {
                return Json(new ResultModel(false, "集团ID不能为空"));
            } 
            if (string.IsNullOrEmpty(model.AppKey))
            {
                return Json(new ResultModel(false, "集团AppKey不能为空"));
            }
            if (string.IsNullOrEmpty(model.AppVersion))
            {
                return Json(new ResultModel(false, "集团AppVersion不能为空"));
            }
            var res = iGroupServices.CreateGroupApiConfig(model);
            if (res.Result)
            {
                return Json(res.Data ? new ResultModel(true, "成功") : new ResultModel(false, "已存在该appkey"));
            }
            return Json(new ResultModel(false, "服务器异常"));
        }

     
        /// <summary>
        /// 更新集团状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateStatus(int id,byte status)
        {
            if (id == 0)
            {
                return Json(new ResultModel(false, "集团ID不能为空"));
            }  
            var mod=new GroupModel {Id = id, IsValid =(byte?)(status > 0 ? 0 : 1)};
            var res = iGroupServices.UpdateGroupStatus(mod);
            if (res.Result)
            {
                return Json(res.Data ? new ResultModel(true, "成功") : new ResultModel(false, "服务器异常"));
            }
            return Json(new ResultModel(false, "服务器异常"));
        }

        

    }
}