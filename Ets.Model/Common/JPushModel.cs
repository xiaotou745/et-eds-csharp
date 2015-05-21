using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    /// <summary>
    /// 极光推送实体 add by caoheyang 20150213
    /// </summary>
    public class JPushModel
    {

        /// <summary>
        /// 来源标识（B端/C端）
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// 提示title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string Alert { get; set; }

        /// <summary>
        /// 提示内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// >商户id  注册ID 数组。多个注册ID之间是 OR 关系，即取并集。 设备标识。一次推送最多 1000 个。
        /// </summary>
        public string RegistrationId { get; set; }

        /// <summary>
        /// 推送城市信息
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 集团id
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

    }
}
