using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Message
{
    /// <summary>
    ///  消息详情 add by caoheyang 20150617
    /// </summary>
   public  class ReadBDM
    {

        /// <summary>
        /// 消息体
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        public string PubDate { get; set; }


    }
}
