﻿
using Ets.Model.ParameterModel.Clienter;
using ETS.Transaction;
using ETS.Transaction.Common;

namespace Ets.Service.Provider.Authority
{
    using System;
    using System.Collections.Generic;
    using ETS.Util;
    using Dao.MenuSet;
    using IProvider.AuthorityMenu;
    using Model.DataModel.Authority;
    using Ets.Model.DomainModel.Authority;
    using Ets.Model.ParameterModel.Authority;
    using Ets.Model.Common;
    using System.Linq;
    using ETS.Data.PageData;
    using ETS.Const;
    /// <summary>
    /// 权限业务操作类-平扬 2015.3.18
    /// </summary>
    public class AuthorityMenuProvider : IAuthorityMenuProvider
    {
        readonly AuthoritySetDao authoritySetDao = new AuthoritySetDao();

        #region 菜单操作

        /// <summary>
        /// 增加菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddMenu(AuthorityMenuModel model)
        {
            bool reslut;
            try
            {
                reslut = authoritySetDao.AddMenu(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMenu(AuthorityMenuModel model)
        {
            bool reslut;
            try
            {
                reslut = authoritySetDao.UpdateMenu(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetMenuList(int parId)
        {
            List<AuthorityMenuModel> list;
            try
            {
                list = authoritySetDao.GetListMenu(parId);
            }
            catch (Exception ex)
            {
                list = null;
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetAllMenuList()
        {
            List<AuthorityMenuModel> list;
            try
            {
                list = authoritySetDao.GetAllListMenu();
            }
            catch (Exception ex)
            {
                list = null;
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }

        /// <summary>
        /// 根据id得到一条菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuthorityMenuModel GetMenuById(int id)
        {
            try
            {
                return authoritySetDao.GetMenuById(id);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }

        #endregion

        #region 角色管理操作

        /// <summary>
        /// 根据roleid获取权限ID列表
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>True or False</returns> 
        public List<int> GetMenuIdsByRoloId(int roleId)
        {
            List<int> list;
            try
            {
                list = authoritySetDao.GetMenuIdsByRoloId(roleId);
            }
            catch (Exception ex)
            {
                list = null;
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }

        /// <summary>
        /// 根据roleid修改权限菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool UpdateRoloMenus(int roleId, string ids)
        {
            bool reslut = false;
            try
            {
                string[] mids = ids.TrimEnd(',').Split(',');
                if (mids.Length > 0)
                {
                    //清楚旧权限
                    authoritySetDao.ClareGroupPermission(roleId);
                    foreach (var i in mids)
                    {
                        int mid = ParseHelper.ToInt(i);
                        if (!authoritySetDao.CheckGroupPermission(roleId, mid))
                        {
                            authoritySetDao.AddGroupPermission(roleId, mid);
                        }
                    }
                    reslut = true;
                }

            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddRole(AuthorityRoleModel model)
        {
            bool reslut;
            try
            {
                reslut = authoritySetDao.AddRole(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateRole(AuthorityRoleModel model)
        {
            bool reslut;
            try
            {
                reslut = authoritySetDao.updateRole(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public List<AuthorityRoleModel> GetListRoles()
        {
            try
            {
                return authoritySetDao.GetListRoles();
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }
        /// <summary>
        /// 得到一条角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuthorityRoleModel GetRoleById(int id)
        {
            try
            {
                return authoritySetDao.GetRoleById(id);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }

        #endregion

        #region 部门

        /// <summary>
        /// 增加部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddDepartment(AuthorityDepartmentModel model)
        {
            bool reslut;
            try
            {
                reslut = authoritySetDao.AddDepart(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateDepartment(AuthorityDepartmentModel model)
        {
            bool reslut;
            try
            {
                reslut = authoritySetDao.UpdateDepart(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AuthorityDepartmentModel GetDepartmentById(int id)
        {
            try
            {
                return authoritySetDao.GetDepartById(id);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityDepartmentModel> GetDepartmentList(int parId)
        {
            List<AuthorityDepartmentModel> list;
            try
            {
                list = authoritySetDao.GetListDepart(parId);
            }
            catch (Exception ex)
            {
                list = null;
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }

        #endregion


        #region 个人权限操作
        /// <summary>
        /// 获取个人权限
        /// </summary>
        /// <param name="aId"></param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetMenusByAccountId(int aId)
        {
            try
            {
                return authoritySetDao.GetMenusByAccountId(aId);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;

        }


        /// <summary>
        /// 获取个人账户列表
        /// </summary>
        /// <returns></returns>
        public List<Ets.Model.DataModel.Authority.AccountModel> GetListAccount()
        {
            try
            {
                return authoritySetDao.GetListAccount();
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }
        /// <summary>
        /// 获取个人账号信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Ets.Model.DataModel.Authority.AccountModel GetAccountByName(string name)
        {
            try
            {
                return authoritySetDao.GetAccountByName(name);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }


        /// <summary>
        /// 根据用户id获取权限ID列表
        /// </summary>
        /// <param name="aId">roleId</param>
        /// <returns>True or False</returns> 
        public List<int> GetMenuIdsByAccountId(int aId)
        {
            List<int> list;
            try
            {
                list = authoritySetDao.GetMenuIdsByAccountId(aId);
            }
            catch (Exception ex)
            {
                list = new List<int>();
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }


        /// <summary>
        /// 批量添加权限
        /// </summary>
        /// <param name="accoutId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool AddPermission(int accoutId, string ids)
        {
            bool reslut = false;
            try
            {
                string[] mids = ids.TrimEnd(',').Split(',');
                if (mids.Length > 0)
                {
                    //清除旧权限
                    authoritySetDao.ClarePermission(accoutId);
                    foreach (var i in mids)
                    {
                        int mid = ParseHelper.ToInt(i);
                        if (!authoritySetDao.CheckPermission(accoutId, mid))
                        {
                            authoritySetDao.AddPermission(accoutId, mid);
                        }
                    }
                    reslut = true;

                    //java中对用户的权限信息进行了redis缓存，因此当.net中对用户的权限进行更新时，需要清除java中的这个用户的权限缓存信息
                    //zhaohailong，20150916
                    var redis = new ETS.NoSql.RedisCache.RedisCache();
                    string cacheKey = RedissCacheKey.Menu_Auth+accoutId;
                    redis.Delete(cacheKey);
                }

            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }


        #endregion

        /// <summary>
        /// 后台用户列表查询
        /// danny-20150320
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<account> GetAuthorityManage(AuthoritySearchCriteria criteria)
        {
            PageInfo<account> pageinfo = authoritySetDao.GetAuthorityManage<account>(criteria);
            return pageinfo;
        }
        /// <summary>
        /// 检查当前用户是否存在
        /// danny-20150323
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool CheckHasAccountName(account account)
        {
            return authoritySetDao.CheckHasAccountName(account);
        }
       
        /// <summary>
        ///  删除用户
        /// danny-20150323
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status">状态 0不可用1可用</param>
        /// <returns></returns>
        public bool ChangStatus(int id, int status)
        {
            return authoritySetDao.ChangStatus(id,status);
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
            return authoritySetDao.ModifyPwdById(id, modifypassword);
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
            return authoritySetDao.HasAuthority(accountId, authorityName);
        }

        /// <summary>
        /// 验证用户权限
        /// danny-20150324
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public bool HasAuthority(int accountId, int menuid)
        {
            return authoritySetDao.CheckPermission(accountId, menuid);
        }
        /// <summary>
        /// 添加用户
        /// danny-20150323
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public DealResultInfo AddAccount(AccountCriteria criteria)
        {
            var dealResultInfo = new DealResultInfo
            {
                DealFlag = false
            };
            var accountModel = new account
            {
                Id = criteria.Id,
                UserName = criteria.UserName,
                LoginName = criteria.LoginName,
                Password =string.IsNullOrWhiteSpace(criteria.Password)?"":MD5Helper.MD5(criteria.Password),
                GroupId = criteria.GroupId,
                Status = criteria.Status,
                AccountType=criteria.AccountType
            };
            var isHave = authoritySetDao.CheckHasAccountName(accountModel);
            if (criteria.OptionType == "0")//添加用户
            {
                if (isHave)
                {
                    dealResultInfo.DealMsg = "用户名已存在!";
                    return dealResultInfo; 
                }
            }
            else//修改用户信息
            {
                if (!isHave)
                {
                    dealResultInfo.DealMsg = "此用户不存在!";
                    return dealResultInfo;
                }
            }
            using (var tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                var accountCityRelation = new AccountCityRelation
                {
                    AccountId = criteria.Id,
                    CreateBy = criteria.OptUserName,
                    UpdateBy = criteria.OptUserName,
                };
                if (criteria.OptionType == "0")//添加用户
                {
                    accountCityRelation.AccountId = authoritySetDao.AddAccount(accountModel);
                    if (accountCityRelation.AccountId == 0)
                    {
                        dealResultInfo.DealMsg = "插入用户信息失败!";
                        return dealResultInfo;
                    }
                }
                else//修改用户信息
                {
                    if (!authoritySetDao.ModifyAccount(accountModel))
                    {
                        dealResultInfo.DealMsg = "修改用户信息失败!";
                        return dealResultInfo;
                    }
                }
                if (!string.IsNullOrWhiteSpace(criteria.CityCodeList))
                {
                    var cityCodeList = criteria.CityCodeList.Split('|');
                    
                    //authoritySetDao.DeleteAccountCityRelation(accountCityRelation);
                    foreach (var cityCode in cityCodeList)
                    {
                        var tempstr = cityCode.Split(',');
                        accountCityRelation.CityId = ParseHelper.ToInt(tempstr[0]);
                        accountCityRelation.IsEnable = ParseHelper.ToInt(tempstr[1]);
                        if (!authoritySetDao.AddAccountCityRelation(accountCityRelation))
                        {
                            dealResultInfo.DealMsg = "插入用户和城市关联信息失败!";
                            return dealResultInfo;
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(criteria.DcIdList))
                {
                    var dclist = criteria.DcIdList.Split('|');
                    foreach (string dcid in dclist)
                    {
                        var temp = dcid.Split(',');
                        var mode = new AccountDcRelation()
                        {
                            AccountId = accountCityRelation.AccountId,
                            CreateBy = criteria.OptUserName,
                            DeliveryCompanyID = ParseHelper.ToInt(temp[0]),
                            IsEnable = ParseHelper.ToInt(temp[1])
                        };
                        if (!authoritySetDao.AddAccountDCRelation(mode))
                        {
                            dealResultInfo.DealMsg = "插入用户和配送公司关联信息失败!";
                            return dealResultInfo;
                        }
                    }
                }
                else
                {
                    authoritySetDao.DeleteAccountDCRelation(accountCityRelation.AccountId);
                }
                tran.Complete();
            }
            dealResultInfo.DealMsg = "用户信息提交成功！";
            dealResultInfo.DealFlag = true;
            return dealResultInfo;
        }

        /// <summary>
        /// 获取用户和城市对应关系列表
        /// danny-20150525
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IList<AccountCityRelationModel> GetAccountCityRel(int accountId)
        {
            return authoritySetDao.GetAccountCityRel(accountId);
        }

        /// <summary>
        /// 查询后台账号信息列表（分页）
        /// danny-20150609
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<AccountModel> GetAccountListOfPaging(AuthoritySearchCriteria criteria)
        {
            return authoritySetDao.GetAccountListOfPaging<AccountModel>(criteria);
        }


        /// <summary>
        /// 获取用户和物流公司对应关系
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IList<AccountDCRelationModel> GetAccountDCRel(int accountId)
        {
            return authoritySetDao.GetAccountDCRel(accountId);
        }
    }
}
