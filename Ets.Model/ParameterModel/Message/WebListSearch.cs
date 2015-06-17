using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Message
{
    /// <summary>
    /// web后台列表页功能 add by caoheyang 20150616
    /// </summary>
    public class WebListSearch
    {
        private int _pageIndex;
        public int PageIndex
        {
            get
            {
                return _pageIndex == 0 ? 1 : _pageIndex;
            }
            set { _pageIndex = value; }
        }


        private int _pushWay = -1;

        /// <summary>
        /// 推送方式1短信2app通知3短信和app
        /// </summary>
        public int PushWay
        {
            get
            {
                return _pushWay;
            }
            set { _pushWay = value; }
        }

        private int _messageType = -1;
        /// <summary>
        /// 消息类型1通知2策略调整3活动
        /// </summary>
        public int MessageType
        {
            get
            {
                return _messageType;
            }
            set { _messageType = value; }
        }

        private int _sentStatus = -1;
        /// <summary>
        ///  发送状态  0待发布 1发布中 2已发布 3 已取消
        /// </summary>
        public int SentStatus
        {
            get
            {
                return _sentStatus;
            }
            set { _sentStatus = value; }
        }

        private int _sendType = -1;
        /// <summary>
        /// 推送类型1时时发布2定时发布
        /// </summary>
        public int SendType
        {
            get
            {
                return _sendType;
            }
            set { _sendType = value; }
        }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PubDateStart { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PubDateEnd { get; set; }
    }
}
