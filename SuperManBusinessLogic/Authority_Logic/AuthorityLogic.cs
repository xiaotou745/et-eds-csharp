using SuperManCommonModel;
using SuperManCommonModel.Entities;
using SuperManCommonModel.Models;
using SuperManCore;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManBusinessLogic.Authority_Logic
{
    public class AuthorityLogic
    {
        private volatile static AuthorityLogic _instance = null;
        private static readonly object lockHelper = new object();
        private AuthorityLogic() { }
        public static AuthorityLogic authorityLogic()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new AuthorityLogic();
                }
            }
            return _instance;
        }


        /// <summary>
        ///  后台用户列表查询 add by caohheyang 20150212
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public AuthorityManage GetAuthorityManage(AuthoritySearchCriteria criteria)
        {
            using (var db = new supermanEntities())
            {
                var items = db.account.Where(p => p.Status == ConstValues.AccountAvailable);
                if (!string.IsNullOrEmpty(criteria.UserName))
                    items = items.Where(p => p.UserName == criteria.UserName);
                if (criteria.GroupId != null)  //集团查询
                    items = items.Where(p => p.GroupId == criteria.GroupId);
                var pagedQuery = new AuthorityManage();
                var resultModel = new PagedList<account>(items.ToList(), criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize);
                var businesslists = new AuthorityManageList(resultModel.ToList(), resultModel.PagingResult);
                pagedQuery.authorityManageList = businesslists;
                return pagedQuery;
            }
        }

        /// <summary>
        /// 添加帐户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool AddAccount(account account)
        {
            bool bResult = false;
            using (var db = new supermanEntities())
            {
                db.account.Add(account);
                int i = db.SaveChanges();
                if (i == 1)
                {
                    bResult = true;
                }
            }
            return bResult;
        }

        /// <summary>
        /// 是否存在该用户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool HasAccountName(account account)
        {
            bool bResult = false;
            using (var db = new supermanEntities())
            {
                var oldauid = db.account.Where(a => a.UserName == account.UserName || a.LoginName == account.LoginName).ToList();
                if ( oldauid.Count > 0)
                {
                    bResult=true;
                } 
            }
            return bResult;
        }

        public bool GetAccountById(int id)
        {
            bool result = false;
            using (var db = new supermanEntities())
            {
                var account = db.account.Find(id);
                if (account != null)
                {
                    return true;
                }
            }
            return result;
        }
        public enum MyEnum
        {

        }
        /// <summary>
        /// 更新后台用户权限信息
        /// </summary>
        /// <param name="model"></param>
        public void UpdateAuthority(AuthorityListModel model)
        {
            using (var db = new supermanEntities())
            {
                var allAuthorities = db.authority.ToList(); //获取所有菜单功能
                var account = db.account.Find(model.id);    //找到当前用户
                if (account != null)
                {
                    var oldauid = db.accountauthority.Where(p => p.AccountId.Value == account.Id).ToList();
                    List<int> existAuid = new List<int>(); 
                    List<accountauthority> toRemove = new List<accountauthority>();
                    foreach (var oldid in oldauid)
                    {
                        if (model.auths == null)
                        {
                            // oldauid.Remove(oldid);
                            toRemove.Add(oldid);
                            continue;
                        }
                        // if exist 
                        if (model.auths.Contains(oldid.AuthorityId.Value))
                        {
                            // will for skip 
                            existAuid.Add(oldid.AuthorityId.Value);
                        }
                        else
                        {
                            // remove 
                            toRemove.Add(oldid);
                        }
                    }
                    // oldauid.Remove()
                    foreach (var rm in toRemove)
                    {
                        var todelete = db.accountauthority.FirstOrDefault(m => m.Id == rm.Id);
                        db.accountauthority.Remove(todelete);
                    }
                    // news to add 
                    var exceptid = (from m in model.auths 
                                select m).Except(existAuid).ToList();
                    foreach (int newid in exceptid)
                    {
                        var aa = new accountauthority();
                        aa.AccountId = account.Id;
                        aa.AuthorityId = newid;
                        db.accountauthority.Add(aa);
                    }
 
                    db.SaveChanges();
                }
            }
        }

        private static void NewMethod(bool bAuthority, supermanEntities db, SuperManDataAccess.account account, int authorityId)
        {
            if (bAuthority == true)
            {
                var aa = new accountauthority();
                aa.AccountId = account.Id;
                aa.AuthorityId = authorityId;
                db.accountauthority.Add(aa);
            }
        }
        public List<int> GetAuthorities(int accountId)
        {
            using (var db = new supermanEntities())
            {
                var query = (from m in db.accountauthority
                             where m.account.Id == accountId
                             select m.AuthorityId.Value).ToList();
                return query;
            }
        }

        public bool DeleteById(int id)
        {
            var result = false;
            using (var db = new supermanEntities())
            {
                var query = db.account.Find(id);
                if (query != null)
                {
                    query.Status = ConstValues.AccountDisabled;
                    int i = db.SaveChanges();
                    if (i == 1)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ModifyPwdById(int id, string modifypassword)
        {
            var result = false;
            using (var db = new supermanEntities())
            {
                var query = db.account.Find(id);
                if (query != null)
                {
                    query.Password = modifypassword;
                    int i = db.SaveChanges();
                    if (i == 1)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}
