using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperManCommonModel;
using SuperManCommonModel.Entities;
using SuperManCommonModel.Models;
using SuperManCore;
using SuperManDataAccess;

namespace SuperManBusinessLogic.Authority_Logic
{
    public class AccountBussinessLogic
    {

        public AccountBussinessLogic()
        {
        }  

        //public List<ChannelAccountItem> SearchChannelAccount(ChannelAccountSearchCriteria criteria)
        //{
        //    var query = (from a in _accountRepository.Table
        //                 join m in _appChannelRepository.Table on a.Id equals m.AccountId
        //                 where m.IsDelete == false
        //                 select new ChannelAccountItem
        //                 {
        //                     AccountId = a.Id,
        //                     AccountType = a.AccountType,
        //                     LoginName = a.LoginName,
        //                     Password = a.Password,
        //                     ChannelId = m.Id,
        //                     ChannelName = m.ChannelName,
        //                     CreateTime = m.CreateTime,
        //                     IsDelete = m.IsDelete
        //                 });
        //    if (string.IsNullOrEmpty(criteria.LoginName) == false)
        //    {
        //        query = query.Where(m => m.LoginName == criteria.LoginName);
        //    }
        //    if (string.IsNullOrEmpty(criteria.ChannelName) == false)
        //    {
        //        query = query.Where(m => m.ChannelName == criteria.ChannelName);
        //    }

        //    return query.ToList();
        //}


        public bool HasAuthority(int accountId, string authorityName)
        {
            using (var db = new supermanEntities())
            {
              return  db.accountauthority.Any(i => i.AccountId == accountId && i.authority.Name == authorityName);
            }
        }

        public account Get(int accountId)
        {
            using (var db = new supermanEntities())
            {
                return db.account.FirstOrDefault(i => i.Id == accountId);
            }
        }

        public account Get(string name)
        {
            using (var db = new supermanEntities())
            {
                return db.account.FirstOrDefault(i => i.LoginName == name);

            }
        } 

        public UserLoginResults ValidateUser(string userName, string password)
        {
            using (var db = new supermanEntities())
            {

                var user = this.Get(userName);
                if (user == null)
                {
                    return UserLoginResults.UserNotExist;
                }
                if (user.Password != password)
                {
                    return UserLoginResults.WrongPassword;
                }
                if (user.AccountType == (int) AccountType.AdminUser)
                {
                    var  admin = db.account.Single(i => i.Id== user.Id);
                    if ( admin.Status == 0)
                    {
                        return UserLoginResults.AccountClosed;
                    }
                }
            }
            return UserLoginResults.Successful;
        }

        public IList<authority> GetAllAuthorities()
        {
            using (var db = new supermanEntities())
            {
                return  db.authority.ToList();
            }
        }

        //public IList<account> Search(AccountSearchCriteria criteria)
        //{
        //    var query = _accountRepository.Table;
        //    if (!string.IsNullOrEmpty(criteria.LoginName))
        //    {
        //        query = query.Where(i => i.LoginName == criteria.LoginName);
        //    }
        //    return query.ToList();
        //}


    }
}
