using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Bussiness
{
    /// <summary>
    /// 商户地址修改入口参数
    /// </summary>
    public class BusiAddAddressInfoModel
    {   /// <summary>
        /// 商户id
        /// </summary>
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
        /// 手机号  2
        /// </summary>
        public string phoneNo { get; set; }

        /// <summary>
        /// 座机
        /// </summary>
        public string landLine { get; set; }


        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string districtName { get; set; }



        /// <summary>
        /// 经度
        /// </summary>
        public double longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double latitude { get; set; }

        /// <summary>
        /// 是否更新了 手持照片  0更新   1 未更新
        /// </summary>
        public int IsUpdateCheckPicUrl { get; set; }

        /// <summary>
        ///  是否更新了卫生证照片   0更新   1 未更新
        /// </summary>
        public int IsUpdateBusinessLicensePic { get; set; }

        /// <summary>
        /// 手持照片 
        /// </summary>
        public string CheckPicUrl { get; set; }

        /// <summary>
        ///  卫生证照片   
        /// </summary>
        public string BusinessLicensePic { get; set; }
        /// <summary>
        /// 区Id
        /// </summary>
        public string districtId { get; set; }
      
    }
}
