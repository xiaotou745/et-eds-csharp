using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Bussiness
{
    /// <summary>
    /// 登录成功返回的数据
    /// </summary>
    public class BusiLoginResultModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
        public byte? status { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        public string districtId { get; set; }
        public string district { get; set; }


        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        public string Landline { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public string cityId { get; set; }


        /// <summary>
        /// 手机号
        /// </summary>
        public string phoneNo { get; set; }

        /// <summary>
        ///外送费
        /// </summary>
        public decimal? DistribSubsidy { get; set; }

        /// <summary>
        ///第三方店铺id（暂时只有美团）
        /// </summary>
        public string OriginalBusiId { get; set; }
        
    }
}
