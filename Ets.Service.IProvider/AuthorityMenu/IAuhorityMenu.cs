using Ets.Model.Common;

namespace Ets.Service.IProvider.AuthorityMenu
{
    using System.Collections.Generic;
    using Model.DataModel.Authority;
    using Ets.Model.DomainModel.Authority;
    using Ets.Model.ParameterModel.Authority;
    using ETS.Data.PageData;
  
    /// <summary>
    /// 权限操作接口类 平扬 2015.3.18
    /// </summary>
    public interface IAuthorityMenuProvider
    {
        #region 菜单操作
         
        /// <summary>
        /// 增加菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddMenu(AuthorityMenuModel model);
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateMenu(AuthorityMenuModel model);

        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        List<AuthorityMenuModel> GetMenuList(int parId);

        /// <summary>
        /// 获取全部菜单项
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        List<AuthorityMenuModel> GetAllMenuList();


        /// <summary>
        /// 根据id得到一条菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AuthorityMenuModel GetMenuById(int id);

        #endregion

        #region 部门操作

        /// <summary>
        /// 增加部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddDepartment(AuthorityDepartmentModel model);
        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateDepartment(AuthorityDepartmentModel model);
        /// <summary>
        /// 根据id得到一条部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AuthorityDepartmentModel GetDepartmentById(int id);
        /// <summary>
        /// 获取全部部门
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        List<AuthorityDepartmentModel> GetDepartmentList(int parId);

        #endregion

        #region 角色管理操作

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddRole(AuthorityRoleModel model);
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateRole(AuthorityRoleModel model);

        /// <summary>
        /// 获取全部角色项
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        List<AuthorityRoleModel> GetListRoles();

        /// <summary>
        /// 根据id得到一条角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AuthorityRoleModel GetRoleById(int id);
        /// <summary>
        /// 根据roleid获取权限ID列表
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>True or False</returns>
        List<int> GetMenuIdsByRoloId(int roleId);

        /// <summary>
        /// 根据roleid修改权限菜单
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <param name="ids">ids</param>
        /// <returns>True or False</returns>
        bool UpdateRoloMenus(int roleId,string ids);

        #endregion

        #region 个人权限操作
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        List<Ets.Model.DataModel.Authority.AccountModel> GetListAccount();
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        Ets.Model.DataModel.Authority.AccountModel GetAccountByName(string loginName);
 
        /// <summary>
        /// 加入权限
        /// </summary>
        /// <param name="accoutId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool AddPermission(int accoutId, string ids);

        /// <summary>
        /// 根据用户id获取权限ID列表
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>True or False</returns> 
        List<int> GetMenuIdsByAccountId(int aId);

        /// <summary>
        /// 根据用户id获取权限ID列表
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>True or False</returns> 
        List<AuthorityMenuModel> GetMenusByAccountId(int aId);

        #endregion
        /// <summary>
        /// 后台用户列表查询
        /// danny-20150320
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<account> GetAuthorityManage(AuthoritySearchCriteria criteria);
        /// <summary>
        /// 检查当前用户是否存在
        /// danny-20150323
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool CheckHasAccountName(account account);

        /// <summary>
        /// 添加用户
        /// danny-20150323
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        DealResultInfo AddAccount(AccountCriteria criteria);
        /// <summary>
        /// 删除用户
        /// danny-20150323
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteAccountById(int id);
        /// <summary>
        /// 用户修改密码
        /// danny-20150323
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifypassword"></param>
        /// <returns></returns>
        bool ModifyPwdById(int id, string modifypassword);
        /// <summary>
        /// 验证用户权限
        /// danny-20150324
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="authorityName"></param>
        /// <returns></returns>
        bool HasAuthority(int accountId, string authorityName);

        /// <summary>
        /// 获取用户和城市对应关系列表
        /// danny-20150525
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IList<AccountCityRelationModel> GetAccountCityRel(int accountId);
    }
}
