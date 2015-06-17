using Ets.Model.DataModel.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Message
{
    public class MessageModelDM : MessageModel
    {
        /// <summary>
        /// 操作人id
        /// </summary>
        public int OptUserId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OptUserName { get; set; }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string OptLog { get; set; }
        /// <summary>
        /// 操作类型（1：新增 2：修改）
        /// </summary>
        public int DealType { get; set; }
    }
}
