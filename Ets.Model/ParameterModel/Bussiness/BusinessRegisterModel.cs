using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Bussiness
{
    public class BusinessRegisterModel
    {

        /// <summary>
        /// 商户名称
        /// </summary>
        //[Required]
        public string B_Name { get; set; }
        /// <summary>
        /// 原平台商户Id
        /// </summary>
        //[Required]
        public int B_OriginalBusiId { get; set; }
        public string B_Password { get; set; } 
        /// <summary>
        /// 身份证号
        /// </summary>
        public string B_IdCard { get; set; }
        /// <summary>
        /// 电话号码手机号
        /// </summary>
        //[Required]
        public string PhoneNo { get; set; }
        /// <summary>
        /// 电话号码2
        /// </summary>
        public string PhoneNo2 { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        //[Required] 
        public string B_Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        //[Required]
        //[Required]
        public string B_City { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        //[Required]
        //[Required]
        public string B_Area { get; set; }
        /// <summary>
        /// 街道地址
        /// </summary>
        //[Required]
        //[Required]
        public string Address { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        //[Required]
        //[Required]
        public string B_ProvinceCode { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        //[Required]
        //[Required]
        public string B_CityCode { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        //[Required]
        //[Required]
        public string B_AreaCode { get; set; }
        /// <summary>
        /// 商户所在区域经度
        /// </summary>
        public double B_Longitude { get; set; }
        /// <summary>
        /// 商户所在区域纬度
        /// </summary>
        public double B_Latitude { get; set; }
        /// <summary>
        /// 佣金类型Id
        /// </summary>
        public int CommissionTypeId { get; set; }
        /// <summary>
        /// 外送费
        /// </summary>
        public decimal DistribSubsidy { get; set; }
    }
}
