using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// 骑士上传坐标接口  参数实体类
    /// <UpdateBy>caoheyang</UpdateBy>
    /// <UpdateTime>20150519</UpdateTime>
    /// </summary>
    public class ClienterPushLocaltionPM
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
        /// 骑士Id(Clienter表）
        /// </summary>
        public int ClienterId { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 客户端平台   如:ios    android
        /// </summary>
        public string Platform { get; set; }
    }
}
