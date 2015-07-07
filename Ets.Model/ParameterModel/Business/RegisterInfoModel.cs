using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// B端注册时提供的信息
    /// </summary>
    public class RegisterInfoPM
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

        public string RecommendPhone { get; set; }
    }

    /// <summary>
    /// 后台添加商户实体
    /// </summary>
    public class AddBusinessModel
    {
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }

        public string CityId { get; set; }
        /// <summary>
        /// 店铺名
        /// </summary>
        public string businessName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string phoneNo { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string passWord { get; set; }
        /// <summary>
        /// 结算比例
        /// </summary>
        public string businessCommission { get; set; }
        /// <summary>
        /// 商户地址
        /// </summary>
        public string businessaddr { get; set; }
        /// <summary>
        /// 商户外送费
        /// </summary>
        public string businessWaisong { get; set; }
        /// <summary>
        /// 集团Id
        /// </summary>
        public int GroupId { get; set; }
    }

}
