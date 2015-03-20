using System.Data;
using System.Linq;

namespace Ets.Dao.MenuSet
{
    using ETS.Util;
    using ETS.Dao;
    using ETS.Data.Core;
    using Model.DataModel.Authority;
    using System.Collections.Generic;

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
            var dt  = DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters).Tables[0];
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
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql,dbParameters);
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
            var list=new List<int>();
            if (dt.Rows.Count > 0)
            {
                list.AddRange(from DataRow row in dt.Rows select ParseHelper.ToInt(row["MenuId"]));
            }
            return list; 
        } 
        
        #endregion
    }
}
