using System.Data;
using System.Linq;

namespace Ets.Dao.MenuSet
{
    using ETS.Util;
    using ETS.Dao;
    using ETS.Data.Core;
    using Model.DataModel.Authority;
    using System.Collections.Generic;
    using Ets.Model.ParameterModel.Authority;
    using ETS.Data.PageData;
    using System.Text;
    using System;

    /// <summary>
    /// 菜单权限数据操作类-平扬 2015.3.18
    /// </summary>
    public class AuthoritySetDao : DaoBase
    {
        #region 角色管理

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddRole(AuthorityRoleModel model)
        {
            string sql = @" INSERT INTO AuthorityRole (RoleName,BeLock) VALUES (@RoleName,@BeLock) ;select @@IDENTITY as InsertId;";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("RoleName", model.RoleName);
            dbParameters.AddWithValue("BeLock", model.BeLock);
            object i = DbHelper.ExecuteScalar(SuperMan_Write, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateRole(AuthorityRoleModel model)
        {
            string sql = @" UPDATE AuthorityRole set RoleName=@RoleName where id=@id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("RoleName", model.RoleName);
            dbParameters.AddWithValue("id", model.Id);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            return i > 0;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public List<AuthorityRoleModel> GetListRoles()
        {
            string sql = @" SELECT Id,RoleName,BeLock FROM AuthorityRole with(nolock) where BeLock=0 ";
            var dt = DbHelper.ExecuteDataset(SuperMan_Read, sql).Tables[0];
            return (List<AuthorityRoleModel>)ConvertDataTableList<AuthorityRoleModel>(dt);
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuthorityRoleModel GetRoleById(int id)
        {
            string sql = @" SELECT Id,RoleName,BeLock FROM AuthorityRole with(nolock) where Id=@id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            var dr = DbHelper.ExecuteReader(SuperMan_Read, sql, dbParameters);
            if (dr.Read())
            {
                return new AuthorityRoleModel
                {
                    Id = id,
                    RoleName = ParseHelper.ToString(dr["RoleName"]),
                    BeLock = ParseHelper.ToBool(dr["BeLock"])
                };
            }
            return null;
        }


        #endregion

        #region 菜单管理

        /// <summary>
        /// 增加菜单权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddMenu(AuthorityMenuModel model)
        {
            string sql = @" INSERT INTO AuthorityMenuClass (ParId,MenuName,BeLock,Url,IsButton) VALUES (@ParId,@MenuName,@BeLock,@Url,@IsButton);select @@IDENTITY as InsertId; ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ParId", model.ParId);
            dbParameters.AddWithValue("MenuName", model.MenuName);
            dbParameters.AddWithValue("BeLock", model.BeLock);
            dbParameters.AddWithValue("Url", model.Url);
            dbParameters.AddWithValue("IsButton", model.IsButton);
            object i = DbHelper.ExecuteScalar(SuperMan_Write, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMenu(AuthorityMenuModel model)
        {
            string sql = @" UPDATE AuthorityMenuClass set MenuName=@MenuName,Url=@Url,IsButton=@IsButton where id=@id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("id", model.Id);
            dbParameters.AddWithValue("MenuName", model.MenuName);
            dbParameters.AddWithValue("Url", model.Url);
            dbParameters.AddWithValue("IsButton", model.IsButton);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            return i > 0;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AuthorityMenuModel GetMenuById(int id)
        {
            string sql = @" SELECT Id,ParId,MenuName,BeLock,Url,IsButton FROM AuthorityMenuClass with(nolock) where Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            var dr = DbHelper.ExecuteReader(SuperMan_Read, sql, dbParameters);
            if (dr.Read())
            {
                return new AuthorityMenuModel
                {
                    Id = id,
                    ParId = ParseHelper.ToInt(dr["ParId"]),
                    MenuName = ParseHelper.ToString(dr["MenuName"]),
                    Url = ParseHelper.ToString(dr["Url"]),
                    IsButton = ParseHelper.ToBool(dr["IsButton"]),
                    BeLock = ParseHelper.ToBool(dr["BeLock"])
                };
            }
            return null;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetListMenu(int parId)
        {
            string sql = @" SELECT Id,ParId,MenuName,BeLock,Url,IsButton FROM AuthorityMenuClass with(nolock) where ParId=@ParId ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ParId", parId);
            var dt = DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters).Tables[0];
            return (List<AuthorityMenuModel>)ConvertDataTableList<AuthorityMenuModel>(dt);
        }

        /// <summary>
        /// 获取全部菜单列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetAllListMenu()
        {
            string sql = @" SELECT Id,ParId,MenuName,BeLock,Url,IsButton FROM AuthorityMenuClass with(nolock) where BeLock=CAST(0 as bit) ";
            var dt = DbHelper.ExecuteDataset(SuperMan_Read, sql).Tables[0];
            return (List<AuthorityMenuModel>)ConvertDataTableList<AuthorityMenuModel>(dt);
        }

        #endregion

        #region 部门管理

        /// <summary>
        /// 增加部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddDepart(AuthorityDepartmentModel model)
        {
            string sql = @" INSERT INTO AuthorityDepartment (ParId,DepartName,BeLock) VALUES (@ParId,@DepartName,@BeLock);select @@IDENTITY as InsertId; ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ParId", model.ParId);
            dbParameters.AddWithValue("DepartName", model.DepartName);
            dbParameters.AddWithValue("BeLock", model.BeLock);
            object i = DbHelper.ExecuteScalar(SuperMan_Write, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateDepart(AuthorityDepartmentModel model)
        {
            string sql = @" UPDATE AuthorityDepartment set DepartName=@DepartName where id=@id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("id", model.Id);
            dbParameters.AddWithValue("DepartName", model.DepartName);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            return i > 0;
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AuthorityDepartmentModel GetDepartById(int id)
        {
            string sql = @" SELECT Id,ParId,DepartName,BeLock FROM AuthorityDepartment  with(nolock) where Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            var dr = DbHelper.ExecuteReader(SuperMan_Read, sql, dbParameters);
            if (dr.Read())
            {
                return new AuthorityDepartmentModel
                {
                    Id = id,
                    ParId = ParseHelper.ToInt(dr["ParId"]),
                    DepartName = ParseHelper.ToString(dr["DepartName"]),
                    BeLock = ParseHelper.ToBool(dr["BeLock"])
                };
            }
            return null;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityDepartmentModel> GetListDepart(int parId)
        {
            string sql = @" SELECT Id,ParId,DepartName,BeLock FROM AuthorityDepartment with(nolock) where ParId=@ParId ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ParId", parId);
            var dt = DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters).Tables[0];
            return (List<AuthorityDepartmentModel>)ConvertDataTableList<AuthorityDepartmentModel>(dt);
        }

        #endregion

        #region 个人权限管理设置

        /// <summary>
        /// 获取个人账户列表
        /// </summary>
        /// <returns></returns>
        public List<AccountModel> GetListAccount()
        {
            string sql = @" SELECT [Id]
                                  ,[Password]
                                  ,[UserName]
                                  ,[LoginName]
                                  ,[Status]
                                  ,[AccountType]
                                  ,[FADateTime]
                                  ,[FAUser]
                                  ,[LCDateTime]
                                  ,[LCUser]
                                  ,[GroupId] FROM account with(nolock) ";
            var dt = DbHelper.ExecuteDataset(SuperMan_Read, sql).Tables[0];
            return (List<AccountModel>)ConvertDataTableList<AccountModel>(dt);
        }

        /// <summary>
        ///  获取个人权限列表
        /// </summary>
        /// <param name="accoutId">accoutId</param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetMenusByAccountId(int accoutId)
        {
            string sql =
                " select MenuId,Url,IsButton from AuthorityAccountMenuSet A with(nolock) inner join AuthorityMenuClass M on M.Id=A.MenuId where AccoutId=@AccoutId;";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("AccoutId", accoutId);
            DataTable dt = DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters).Tables[0];
            var list = new List<int>();
            return (List<AuthorityMenuModel>)ConvertDataTableList<AuthorityMenuModel>(dt);
        }

        /// <summary>
        ///  获取个人权限id列表
        /// </summary>
        /// <param name="accoutId">accoutId</param>
        /// <returns></returns>
        public List<int> GetMenuIdsByAccountId(int accoutId)
        {
            string sql = "select MenuId from AuthorityAccountMenuSet with(nolock) where AccoutId=@AccoutId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("AccoutId", accoutId);
            DataTable dt = DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters).Tables[0];
            var list = new List<int>();
            if (dt.Rows.Count > 0)
            {
                list.AddRange(from DataRow row in dt.Rows select ParseHelper.ToInt(row["MenuId"]));
            }
            return list;
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public AccountModel GetAccountByName(string loginName)
        {
            string sql = @" SELECT [Id]
                                  ,[Password]
                                  ,[UserName]
                                  ,[LoginName]
                                  ,[Status]
                                  ,[AccountType]
                                  ,[FADateTime]
                                  ,[FAUser]
                                  ,[LCDateTime]
                                  ,[LCUser]
                                  ,[GroupId],RoleId FROM account with(nolock) where LoginName=@loginName ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("LoginName", loginName);
            var dr = DbHelper.ExecuteReader(SuperMan_Read, sql, dbParameters);
            if (dr.Read())
            {
                return new AccountModel
                {
                    Id = ParseHelper.ToInt(dr["Id"]),
                    UserName = ParseHelper.ToString(dr["UserName"]),
                    LoginName = ParseHelper.ToString(dr["LoginName"]),
                    Status = ParseHelper.ToInt(dr["Status"]),
                    AccountType = ParseHelper.ToInt(dr["AccountType"]),
                    LCDateTime = ParseHelper.ToDatetime(dr["LCDateTime"]),
                    GroupId = ParseHelper.ToInt(dr["GroupId"]),
                    RoleId = ParseHelper.ToInt(dr["RoleId"])
                };
            }
            return null;
        }

        /// <summary>
        /// 检测是否存在指定权限
        /// </summary>
        /// <param name="accoutId">用户ID</param>
        /// <param name="menuId">权限ID</param>
        /// <returns></returns>
        public bool CheckPermission(int accoutId, int menuId)
        {
            string sql = "select Id from AuthorityAccountMenuSet with(nolock) where AccoutId=@AccoutId and MenuId = @MenuId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("AccoutId", accoutId);
            dbParameters.AddWithValue("MenuId", menuId);
            object i = DbHelper.ExecuteScalar(SuperMan_Read, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 加入权限
        /// </summary>
        /// <param name="accoutId">用户ID</param>
        /// <param name="menuId">权限ID</param>
        /// <returns></returns>
        public bool AddPermission(int accoutId, int menuId)
        {
            if (CheckPermission(accoutId, menuId)) return false;
            string sql = " INSERT INTO AuthorityAccountMenuSet(AccoutId,MenuId) VALUES  (@AccoutId,@MenuId);select @@IDENTITY as InsertId;";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("AccoutId", accoutId);
            dbParameters.AddWithValue("MenuId", menuId);
            object i = DbHelper.ExecuteScalar(SuperMan_Write, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }
        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="accoutId">accoutId</param>
        /// <returns>True or False</returns>
        public bool ClarePermission(int accoutId)
        {
            string sql = "delete from AuthorityAccountMenuSet where AccoutId=@AccoutId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("AccoutId", accoutId);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            return i > 0;
        }

        #endregion

        #region 角色权限管理

        /// <summary>
        /// 加入权限
        /// </summary>
        /// <param name="accoutId">用户ID</param>
        /// <param name="menuId">权限ID</param>
        /// <returns></returns>
        public bool AddGroupPermission(int roleId, int menuId)
        {
            if (CheckGroupPermission(roleId, menuId)) return false;
            string sql = " INSERT INTO AuthorityRoleMentMenuSet(RoleId,MenuId) VALUES  (@RoleId,@MenuId);select @@IDENTITY as InsertId;";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("RoleId", roleId);
            dbParameters.AddWithValue("MenuId", menuId);
            object i = DbHelper.ExecuteScalar(SuperMan_Write, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 检测是否存在指定权限组
        /// </summary>
        /// <param name="departId">权限组ID</param> 
        /// <param name="menuId">权限ID</param>
        /// <returns></returns>
        public bool CheckGroupPermission(int roleId, int menuId)
        {
            string sql = "select Id from AuthorityRoleMentMenuSet with(nolock) where RoleId= @RoleId and MenuId = @MenuId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("RoleId", roleId);
            dbParameters.AddWithValue("menuId", menuId);
            object i = DbHelper.ExecuteScalar(SuperMan_Read, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 根据条件删除权限组
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>True or False</returns>
        public bool ClareGroupPermission(int roleId)
        {
            string sql = "delete from AuthorityRoleMentMenuSet where roleId=@roleId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("roleId", roleId);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            return i > 0;
        }

        /// <summary>
        /// 根据roleid获取权限ID列表
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>True or False</returns>
        public List<int> GetMenuIdsByRoloId(int roleId)
        {
            string sql = "select MenuId from AuthorityRoleMentMenuSet with(nolock) where RoleId=@roleId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("roleId", roleId);
            DataTable dt = DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters).Tables[0];
            var list = new List<int>();
            if (dt.Rows.Count > 0)
            {
                list.AddRange(from DataRow row in dt.Rows select ParseHelper.ToInt(row["MenuId"]));
            }
            return list;
        }

        #endregion

        /// <summary>
        /// 后台用户列表查询
        /// danny-20150320
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetAuthorityManage<T>(AuthoritySearchCriteria criteria)
        {
            string columnList = @"   a.[Id]
                                    ,a.[Password]
                                    ,a.[UserName]
                                    ,a.[LoginName]
                                    ,a.[Status]
                                    ,a.[AccountType]
                                    ,a.[FADateTime]
                                    ,a.[FAUser]
                                    ,a.[LCDateTime]
                                    ,a.[LCUser]
                                    ,a.[GroupId]
                                    ,a.[RoleId]
                                    ,g.GroupName";
            var sbSqlWhere = new StringBuilder(" 1=1 AND a.Status=1 ");
            if (criteria.GroupId != null && criteria.GroupId != 0)
            {
                sbSqlWhere.AppendFormat(" AND a.GroupId={0} ", criteria.GroupId);
            }
            if (!string.IsNullOrEmpty(criteria.UserName))
            {
                sbSqlWhere.AppendFormat(" AND a.UserName='{0}' ", criteria.UserName);
            }
            string tableList = @" account a  WITH (NOLOCK) LEFT JOIN dbo.[group] g WITH(NOLOCK) ON g.Id = a.GroupId   ";
            string orderByColumn = " a.Id DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }

        /// <summary>
        /// 检查当前用户是否存在
        /// danny-20150323
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool CheckHasAccountName(account account)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM account WITH (NOLOCK) WHERE (UserName=@UserName OR LoginName=@LoginName) AND Status = 1 ";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@UserName", account.UserName);
                parm.AddWithValue("@LoginName", account.LoginName);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "检查当前用户是否存在");
                return false;
                throw;
            }
        }


        /// <summary>
        /// 添加用户
        /// danny-20150323
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool AddAccount(account account)
        {
            try
            {
                string sql = @"
                           INSERT INTO account
                               ([Password]
                               ,[UserName]
                               ,[LoginName]
                               ,[Status]
                               ,[AccountType]
                               ,[FADateTime]
                               ,[FAUser]
                               ,[LCDateTime]
                               ,[LCUser]
                               ,[GroupId]
                               ,[RoleId])
                         VALUES
                               (@Password
                               ,@UserName
                               ,@LoginName
                               ,@Status
                               ,@AccountType
                               ,@FADateTime
                               ,@FAUser
                               ,@LCDateTime
                               ,@LCUser
                               ,@GroupId
                               ,@RoleId)SELECT @@IDENTITY
                        ";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@Password", account.Password);
                parm.AddWithValue("@UserName", account.UserName);
                parm.AddWithValue("@LoginName", account.LoginName);
                parm.AddWithValue("@Status", account.Status);
                parm.AddWithValue("@AccountType", account.AccountType);
                parm.AddWithValue("@FADateTime", account.FADateTime);
                parm.AddWithValue("@FAUser", account.FAUser);
                parm.AddWithValue("@LCDateTime", account.LCDateTime);
                parm.AddWithValue("@LCUser", account.LCUser);
                parm.AddWithValue("@GroupId", account.GroupId);
                parm.AddWithValue("@RoleId", account.RoleId);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("添加用户", new { ex = ex, account = account });
                return false;
                throw;
            }
        }

        /// <summary>
        ///  删除用户
        /// danny-20150323
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAccountById(int id)
        {
            bool reslut = false;
            try
            {
                string sql = " update account set Status=@Status where Id=@id ";
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", id);
                dbParameters.AddWithValue("Status", Ets.Model.Common.ConstValues.AccountDisabled);
                int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
                if (i > 0) reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "删除用户");
                throw;
            }
            return reslut;
        }

        /// <summary>
        /// 用户修改密码
        /// danny-20150323
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifypassword"></param>
        /// <returns></returns>
        public bool ModifyPwdById(int id, string modifypassword)
        {
            bool reslut = false;
            try
            {
                string sql = " update account set Password=@Password where Id=@id ";
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", id);
                dbParameters.AddWithValue("Password", modifypassword);
                int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
                if (i > 0) reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "用户修改密码");
                throw;
            }
            return reslut;
        }
        /// <summary>
        /// 验证用户权限
        /// danny-20150324
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="authorityName"></param>
        /// <returns></returns>
        public bool HasAuthority(int accountId, string authorityName)
        {
            try
            {
                string sql = @" SELECT COUNT(*) 
                                FROM accountauthority aa WITH (NOLOCK)
                                JOIN authority a WITH (NOLOCK) ON aa.AuthorityId=a.Id
                                WHERE aa.AccountId=@AccountId AND a.Name =@Name ";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@AccountId", accountId);
                parm.AddWithValue("@Name", authorityName);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "验证用户权限");
                return false;
                throw;
            }
        }

        /// <summary>
        /// 添加用户城市对应关系
        /// danny-20150522
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddAccountCityRelation(AccountCityRelation model)
        {
            string sql = @"
INSERT INTO [AccountCityRelation]
    ([AccountId]
    ,[CityId]
    ,[CreateBy]
    ,[UpdateBy])
VALUES
    (@AccountId
    ,@CityId
    ,@CreateBy
    ,@UpdateBy)";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@AccountId", model.AccountId);
            parm.AddWithValue("@CityId", model.CityId);
            parm.AddWithValue("@CreateBy", model.CreateBy);
            parm.AddWithValue("@UpdateBy", model.UpdateBy);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm)) > 0;
        }
    }
}
