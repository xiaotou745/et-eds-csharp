using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    public class OpenCityModel
    {
        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 省份编码
        /// </summary>
        public int ProvinceCode { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public int CityCode { get; set; }
        /// <summary>
        /// 区县名称
        /// </summary>
        public string DistrictName { get; set; }
        /// <summary>
        /// 区县编码
        /// </summary>
        public int DistrictCode { get; set; }
        /// <summary>
        /// 是否开放
        /// </summary>
        public int IsPublic { get; set; }
    }
}
