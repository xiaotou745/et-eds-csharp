using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    public class BusiForgetPwdInfoModel
    {
        public string phoneNumber { get; set; }
        public string password { get; set; }
        public string checkCode { get; set; }
        public string oldpassword { get; set; }
    }
}
