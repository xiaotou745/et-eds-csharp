using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// 登录实体
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// MD5加密后的手机号
        /// </summary>
        //public string aesPhoneNo { get; set; }
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
        /// <summary>
        /// 手机操作系统android,ios
        /// </summary>
        public string OperSystem { get; set; }
        /// <summary>
        /// 手机具体型号5.0
        /// </summary>
        public string OperSystemModel { get; set; }
        /// <summary>
        /// 手机类型,三星、苹果
        /// </summary>
        public string PhoneType { get; set; }
        /// <summary>
        /// App版本
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
