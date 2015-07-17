using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    public class AppVerionModel
    {
        /// <summary>
        /// 当前版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 是否强制升级 1 是 0否 默认0
        /// </summary>
        public int IsMust { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string UpdateUrl { get; set; }

        /// <summary>
        /// 升级信息 可以不填
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 自增主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 客户端类型 1:Android 2 :IOS 默认Android
        /// </summary>
        public int PlatForm { get; set; }
        /// <summary>
        /// 用户版本 1 骑士 2 商家 默认1骑士
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 是否定时 0否 1 是
        /// </summary>
        public int IsTiming { get; set; }
        /// <summary>
        /// 定时发布时间
        /// </summary>
        public DateTime? TimingDate { get; set; }
        /// <summary>
        /// 发布状态 0待发布 1 已发布 2 取消发布
        /// </summary>
        public int PubStatus { get; set; }


    }
}
