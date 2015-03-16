using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Bussiness
{
    /// <summary>
    /// B端注册时提供的信息
    /// </summary>
    public class RegisterInfoModel
    {
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        public string CityId { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string phoneNo { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string passWord { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string verifyCode { get; set; }
        /// <summary>
        /// 集团Id
        /// </summary>
        public int GroupId { get; set; }
    }
}
