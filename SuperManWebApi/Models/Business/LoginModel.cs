using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    /// <summary>
    /// 登录实体
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 登录手机号
        /// </summary>
        public string phoneNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string passWord { get; set; }
    }
}