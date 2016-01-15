using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Common
{
    public class ParamModel
    {
        public string data { get; set; }
        /// <summary>
        /// 登录手机号
        /// </summary>
        public string phoneNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string passWord { get; set; }

        /// <summary>
        /// 手机唯一标识ssid
        /// </summary>
        public string Ssid { get; set; }

    }
}
