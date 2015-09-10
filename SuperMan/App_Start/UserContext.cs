using System;
using System.Collections.Generic;
using System.Text;
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
                if (string.IsNullOrEmpty(cookie))
                {
                    return UserContext.Empty;
                }
                else
                {
                    //如果cookie的值是{"Id":1,"Password":"abc123","UserName":null,"LoginName":"admin","GroupId":0,"RoleId":0,"AccountType":1}格式
                    //则表示这个cookie的值是老的版本，不能兼容java，此时需要删除这个老的cookie，让用户重新登录
                    //java和.net通用的cookie的值需要是经过了UrlEncode的（由于cookie的值是json）
                    //if (cookie.Contains("{\""))
                    //{
                    //    return UserContext.Empty;
                    //}
                    cookie = HttpUtility.UrlDecode(cookie, Encoding.UTF8);
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

        //旧有判断cookie代码
        /*public static UserContext Current
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
        }*/

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
            //string cookieValue = CookieHelper.ReadCookie("menulist");
            //if (cookieValue == "")
            //{
            //    return new AuthorityMenuProvider().HasAuthority(UserContext.Current.Id, menuid);
            //}
            //else
            //{
            //    var list = JsonHelper.ToObject<List<int>>(cookieValue);
            //    return list.Contains(menuid);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int GroupId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int AppChannelId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int RoleId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int AccountType { get; set; }
 
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

    }
}