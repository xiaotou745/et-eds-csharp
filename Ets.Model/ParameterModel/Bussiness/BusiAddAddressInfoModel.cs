using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Bussiness
{
    public class BusiAddAddressInfoModel
    {
        public int userId { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string businessName { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phoneNo { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string landLine { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string districtName { get; set; }
        /// <summary>
        /// 区Id
        /// </summary>
        public string districtId { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double latitude { get; set; }
        public string cityId { get; set; }
    }
}
