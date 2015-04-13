using System.Collections.Generic;
using SuperMan.App_Start;

namespace SuperMan.Controllers
{
    using System.Web.Mvc;
    using Ets.Model.Common;
    using Ets.Service.IProvider.AuthorityMenu;
    using Ets.Service.Provider.Authority;
    using Ets.Model.DataModel.Authority;

    /// <summary>
    /// 权限业务操作类--平扬 2015.3.18
    /// </summary>
    [WebHandleError]
    public class MenuManagerController : BaseController
    {
        /// <summary>
        /// 菜单权限服务操作类
        /// </summary>
        readonly IAuthorityMenuProvider _iAuhority = new AuthorityMenuProvider();

        #region 菜单操作 
        
        /// <summary>
        /// 返回权限菜单视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Menu(int? id)
        {
            //if (UserContext.Current.Id == 0)
            //{
            //    Response.Redirect("/account/login");
            //    return null;
            //}
            int parid = id ?? 0;
            ViewBag.ParId = parid;
            if (parid > 0)
            {
                ViewBag.PartMenu = _iAuhority.GetMenuById(parid);
            } 
            var list = _iAuhority.GetMenuList(parid);
            return View(list);
        } 

        /// <summary>
        /// 增加菜单
        /// </summary>
        /// <param name="AuthorityMenuModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMenu(AuthorityMenuModel mod)
        { 
            bool b = _iAuhority.AddMenu(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "添加失败!"), JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="AuthorityMenuModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateMenu(AuthorityMenuModel mod)
        { 
            bool b = _iAuhority.UpdateMenu(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "修改失败!"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 角色管理操作 
        
        /// <summary>
        /// 返回角色视图
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleManager()
        { 
            var list = _iAuhority.GetListRoles();
            ViewBag.AllMenu = _iAuhority.GetAllMenuList();
            return View(list);
        }

        /// <summary>
        /// 返回角色权限菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMenusByRole(int roleid)
        {
            var list = _iAuhority.GetMenuIdsByRoloId(roleid)??new List<int>();
            return Json(new ResultModel(true, "", list), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddRole(AuthorityRoleModel mod)
        {
            bool b = _iAuhority.AddRole(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "添加失败!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateRole(AuthorityRoleModel mod)
        {
            bool b = _iAuhority.UpdateRole(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "修改失败!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改角色的权限
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditRoleMenus(int rid,string ids)
        {
            bool b = _iAuhority.UpdateRoloMenus(rid, ids);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "设置失败!"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 部门设置
         
        /// <summary>
        /// 返回部门视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Department(int? id)
        {
            int parid = id ?? 0;
            ViewBag.ParId = parid;
            if (parid > 0)
            {
                ViewBag.PartMenu = _iAuhority.GetDepartmentById(parid);
            } 
            var list = _iAuhority.GetDepartmentList(parid);
            return View(list);
        }
        /// <summary>
        /// 增加部门
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddDepart(AuthorityDepartmentModel mod)
        { 
            bool b = _iAuhority.AddDepartment(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "添加失败!"), JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateDepart(AuthorityDepartmentModel mod)
        { 
            bool b = _iAuhority.UpdateDepartment(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "修改失败!"), JsonRequestBehavior.AllowGet);
        } 
        
        #endregion

        #region 个人账户操作
        
        /// <summary>
        /// 返回账号视图
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountManager()
        {
            var list = _iAuhority.GetListAccount(SuperMan.App_Start.UserContext.Current.GroupId);
            ViewBag.AllMenu = _iAuhority.GetAllMenuList();
            return View(list);
        }


        /// <summary>
        /// 返回账户权限菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMenusByAccount(int aid)
        {
            var list = _iAuhority.GetMenuIdsByAccountId(aid); 
            return Json(list!=null ? new ResultModel(true, string.Empty,list) : new ResultModel(false, "设置失败!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改账户的权限
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditAccountMenus(int aid, string ids)
        {
            bool b = _iAuhority.AddPermission(aid, ids);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "设置失败!"), JsonRequestBehavior.AllowGet);
        }

        #endregion

         
        /// <summary>
        /// 返回按钮视图
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuButton(int pid)
        {
            ViewBag.ParId = pid;
            ViewBag.PartMenu = _iAuhority.GetMenuById(pid);
            if (ViewBag.PartMenu == null)
            {
                Response.Redirect("/MenuManager/Menu/"+pid);
                return null;
            }
            var list = _iAuhority.GetMenuList(pid);
            return View(list);
        }

        

    }
}