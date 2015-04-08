using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Area
{
    /// <summary>
    /// 根据省市区名称获取对应的省市区编码实体 add by caoheyang 20150407
    /// </summary>
    public class DMAreaCodeInfo
    {
        /// <summary>
        /// 省编码
        /// </summary>
        public int ProvinceCode { get; set; }
        /// <summary>
        /// 省是否开放 城市是否开放0不开放,1开放
        /// </summary>
        public int ProvinceIsOpen { get; set; }
        /// <summary>
        /// 市编码
        /// </summary>
        public int CityCode { get; set; }
        /// <summary>
        /// 市是否开放 城市是否开放0不开放,1开放
        /// </summary>
        public int CityIsOpen { get; set; }
        /// <summary>
        /// 区编码
        /// </summary>
        public int AreaCode { get; set; }
        /// <summary>
        /// 区是否开放 城市是否开放0不开放,1开放
        /// </summary>
        public int AreaIsOpen { get; set; }
    }
}
