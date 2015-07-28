using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Clienter
{
    /// <summary>
    /// 骑士送餐轨迹表 实体类ClienterLocation 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com  caohgeyang 
    /// Generate Time: 2015-05-19 10:00:53
    /// </summary>

    public class ClienterLocation
    {
        /// <summary>
        /// 主键 自增ID
        /// </summary>
        public long ID { get; set; }
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
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 客户端平台   如:ios    android
        /// </summary>
        public string Platform { get; set; }
    }
}
