using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Authority
{
    /// <summary>
    ///骑士登陆实体
    /// </summary>
    public class ClienterLoginLogDM
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 骑士Id
        /// </summary>
        public int? ClienterId { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// SSID
        /// </summary>
        public string Ssid { get; set; }
        /// <summary>
        /// 手机操作系统android,ios
        /// </summary>
        public string OperSystem { get; set; }
        /// <summary>
        /// 手机具体型号5.0
        /// </summary>
        public string OperSystemModel { get; set; }
        /// <summary>
        /// 手机类型,三星、苹果
        /// </summary>
        public string PhoneType { get; set; }
        /// <summary>
        /// App版本
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否成功   1 成功   0失败
        /// </summary>
        public int IsSuccess { get; set; }

    }
}
