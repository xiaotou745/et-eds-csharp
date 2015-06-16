using System.Collections.Generic;
using System.Web;
using ETS.Const;
using Ets.Model.ParameterModel.Authority;
using Ets.Service.Provider.Authority;
using ETS.Util;

namespace SuperMan.App_Start
{
    public class UserContext
    {
        private static readonly UserContext Empty = new UserContext(); 
        public static UserContext Current
        {
            get
            {
                var cookie = ETS.Util.CookieHelper.ReadCookie(SystemConst.cookieName);
                if (cookie == "")
                {
                    return UserContext.Empty;
                }
                var userInfo = JsonHelper.ToObject<SimpleUserInfoModel>(cookie);
                return new UserContext
                {
                    Id = userInfo.Id,
                    Name = userInfo.LoginName,
                    RoleId = userInfo.RoleId,
                    GroupId = ETS.Util.ParseHelper.ToInt(userInfo.GroupId, 0),
                    AccountType = userInfo.AccountType
                };
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
            string cookieValue = CookieHelper.ReadCookie("menulist");
            if (cookieValue == "")
            {
                return new AuthorityMenuProvider().HasAuthority(UserContext.Current.Id, menuid);
            }
            else
            {
                var list =JsonHelper.ToObject<List<int>>(cookieValue);
                return list.Contains(menuid);
            } 
        }

        public int Id { get; set; }
        public int GroupId { get; set; }
        public int AppChannelId { get; set; }
        public int RoleId { get; set; }
        public int AccountType { get; set; }
        public string Name { get; set; }

    }
}