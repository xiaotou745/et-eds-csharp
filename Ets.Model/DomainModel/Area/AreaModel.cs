using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Area
{
    public class AreaModel
    {
        /// <summary>
        /// 省ID
        /// </summary>
        public int ProvinceCode { get; set; }
        /// <summary>
        /// 省名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 市ID
        /// </summary>
        public int CityCode { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 区ID
        /// </summary>
        public int AreaCode { get; set; }
        /// <summary>
        /// 区名称
        /// </summary>
        public string AreaName { get; set; }

    }
}
