using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Clienter
{
    public class ForgetPwdInfoModel
    {
        /// <summary>
        /// 联系方式
        /// </summary>
        public string phoneNo { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string checkCode { get; set; }
    }
}
