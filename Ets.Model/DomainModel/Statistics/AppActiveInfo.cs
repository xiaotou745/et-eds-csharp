using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Statistics
{
    public class AppActiveInfo
    {
        /// <summary>
        /// 用户类型    1:商家    2:骑士
        /// </summary>
        public byte UserType { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 平台类型
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
    }
}
