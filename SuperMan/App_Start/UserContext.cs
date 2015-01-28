using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperMan.Models;
using SuperMan.Models;
using SuperManCommonModel;
using SuperManBusinessLogic.Authority_Logic;

namespace SuperMan.App_Start
{
    public class UserContext
    {
        private static readonly UserContext Empty = new UserContext();

        private const string CurrentUserContextCacheKey = "UserContext";

        public static UserContext Current
        {
            get
            {
                if (HttpContext.Current.Items[CurrentUserContextCacheKey] == null)
                {
                    var user = HttpContext.Current.User.Identity;

                    var bllAccount = new AccountBussinessLogic();
                    var account = bllAccount.Get(user.Name);
                    if (account == null)
                    {
                        return UserContext.Empty;
                    }

                    var userContext = new UserContext
                    {
                        Id = account.Id,
                        Name = account.LoginName,
                        AccountType = AccountType.AdminUser // (AccountType)account.AccountType,
                    };
                    //AppChannel accountChannel = account.AppChannels.FirstOrDefault();
                    //if (accountChannel != null)
                    //{
                    //    userContext.AppChannelId = accountChannel.Id;
                    //}

                    //if (userContext.AccountType == AccountType.General)
                    //{
                    //    var gallery = DependencyResolver.Current.GetService<IGalleryBussinessLogic>().GetByAccountId(userContext.Id);
                    //    userContext.Gallery = gallery;
                    //}

                    HttpContext.Current.Items[CurrentUserContextCacheKey] = userContext;
                }
                return HttpContext.Current.Items[CurrentUserContextCacheKey] as UserContext;
            }
        }

        public bool HasAuthority(string authorityName)
        {
            if (this == UserContext.Empty)
            {
                return false;
            }
            var bllAccount = new AccountBussinessLogic();
            bool has =  bllAccount.HasAuthority(UserContext.Current.Id, authorityName);
            return has;
        }

        public int Id { get; set; }

        public int AppChannelId { get; set; }

        public AccountType AccountType { get; set; }

        public string Name { get; set; }

        //public UserContextModel UserContextModel
        //{
        //    get
        //    {
        //        return new UserContextModel(this.AccountType, this.Id, this.Gallery == null ? 0 : this.Gallery.Id);
        //    }
        //}
    }
}