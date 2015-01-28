using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Clienter
{
    public class ModifyPwdInfoModel
    {
        public string phoneNo { get; set; }
        public string newPassword { get; set; }
    }
    public class ForgetPwdInfoModel
    {
        public string phoneNo { get; set; }
        public string password { get; set; }
        public string checkCode { get; set; }
    }
}