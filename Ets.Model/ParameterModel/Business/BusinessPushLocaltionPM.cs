using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// 商户上传坐标接口  参数实体类
    /// <UpdateBy>彭宜</UpdateBy>
    /// <UpdateTime>20150727</UpdateTime>
    /// </summary>
    public class BusinessPushLocaltionPM
    {
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// 商户Id
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}
