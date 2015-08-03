using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Clienter
{
    /// <summary>
    /// 登录实体
    /// </summary>
    public class LoginCPM
    {
        /// <summary>
        /// 登录手机号
        /// </summary>
        public string phoneNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string passWord { get; set; }

        //ssid
        public string Ssid { get; set; }
    }
}
