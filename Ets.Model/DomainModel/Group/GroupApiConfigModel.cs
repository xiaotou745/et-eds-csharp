using System;

namespace Ets.Model.DomainModel.Group
{
    /// <summary>
    /// 集团配置业务实体类-平扬  2015.3.16
    /// </summary>
    [Serializable]
    public class GroupApiConfigModel
    {
        /// <summary>
        /// id
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// app_key唯一值
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// app_secret加密值
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// app版本号
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// 集团id
        /// </summary>
        public Int64 GroupId { get; set; }
        /// <summary>
        /// 集团名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public byte IsValid { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
