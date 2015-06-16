using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Message
{
    /// <summary>
    ///  消息类  add by caoheyang  20150616
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 推送方式1短信2app通知3短信和app
        /// </summary>
        public int PushWay { get; set; }
        /// <summary>
        /// 消息类型1通知2策略调整3活动
        /// </summary>
        public int MessageType { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        ///  发送状态  0待发布 1发布中 2已发布 3 已取消
        /// </summary>
        public int SentStatus { get; set; }
        /// <summary>
        /// 推送类型1系统群发2指定对象
        /// </summary>
        public int PushType { get; set; }
        /// <summary>
        /// 推送对象1商家2骑士3商家和骑士4批量导入
        /// </summary>
        public int PushTarget { get; set; }
        /// <summary>
        /// 推送城市
        /// </summary>
        public string PushCity { get; set; }
        /// <summary>
        /// 推送手机号
        /// </summary>
        public string PushPhone { get; set; }
        /// <summary>
        /// 推送类型1时时发布2定时发布
        /// </summary>
        public int SendType { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? OverTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后更改人
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 最后更改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
