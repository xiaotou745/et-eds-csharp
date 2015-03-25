using System.Web;
using ETS.Const;
using Ets.Model.ParameterModel.Authority;
using Ets.Service.Provider.Authority;
using SuperManCommonModel;

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
                    var cookie = ETS.Util.CookieHelper.ReadCookie(SystemConst.cookieName);
                    if (cookie == "")
                    {
                        return UserContext.Empty; 
                    }
                    var userInfo = Letao.Util.JsonHelper.ToObject<SimpleUserInfoModel>(cookie);
                    var userContext = new UserContext
                    {
                        Id = userInfo.Id,
                        Name = userInfo.LoginName,
                        RoleId = userInfo.RoleId,
                        GroupId = userInfo.GroupId,
                        AccountType = AccountType.AdminUser
                    };
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
        public int GroupId { get; set; }
        public int AppChannelId { get; set; }
        public int RoleId { get; set; }
        public AccountType AccountType { get; set; } 
        public string Name { get; set; } 
        
    }
}