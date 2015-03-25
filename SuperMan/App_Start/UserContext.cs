using System.Web;
using Ets.Service.IProvider.AuthorityMenu;
using Ets.Service.Provider.Authority;
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
                    IAuthorityMenuProvider bllAccount = new AuthorityMenuProvider(); 
                    var account = bllAccount.GetAccountByName(user.Name);
                    if (account == null)
                    {
                        return UserContext.Empty;
                    } 
                    var userContext = new UserContext
                    {
                        Id = account.Id,
                        Name = account.LoginName,
                        RoleId = account.RoleId,
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
            bool has = new AuthorityMenuProvider().HasAuthority(UserContext.Current.Id, authorityName);
            return has;
        }

        /// <summary>
        /// 判断用户是否有某权限()
        /// </summary>
        /// <param name="menuid">权限id</param>
        /// <returns></returns>
        public bool HasAuthority(int menuid)
        {
            if (this == UserContext.Empty)
            {
                return false;
            }
            return new AuthorityMenuProvider().HasAuthority(UserContext.Current.Id, menuid);
        }

        public int Id { get; set; }

        public int AppChannelId { get; set; }
        public int RoleId { get; set; }
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