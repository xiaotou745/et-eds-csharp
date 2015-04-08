using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Area
{
    /// <summary>
    ///  根据省市区名称获取对应的省市区编码参数实体 add by caoheyang 20150407
    /// </summary>
    public class ParaAreaNameInfo
    {
        /// <summary>
        /// 省名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 区名称
        /// </summary>
        public string AreaName { get; set; }
    }
}
