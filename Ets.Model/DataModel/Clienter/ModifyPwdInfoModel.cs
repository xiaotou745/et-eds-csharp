using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Clienter
{
    public class ModifyPwdInfoModel
    {
        /// <summary>
        /// 联系方式
        /// </summary>
        public string phoneNo { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string newPassword { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string oldPassword { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string checkCode{ get; set; }


    }
}
