using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using ETS.Dao;
using ETS.Data.PageData;
using Ets.Model.DataModel.Common;
using Ets.Model.DataModel.Message;
using Ets.Model.ParameterModel.Message;

namespace Ets.Dao.Message
{
    /// <summary>
    /// 消息dao   add caoheyang 20150516
    /// </summary>
    public  class MessageDao : DaoBase
    {
        /// <summary>
        /// web后台列表页功能 add by caoheyang 20150616
        /// </summary>
        public PageInfo<MessageModel> WebList(WebListSearch model)
        {
            string where = " 1=1 ";

            return new PageHelper().GetPages<MessageModel>(SuperMan_Read, model.PageIndex, where, "Id desc", "Id,PushWay,MessageType,SendType,Content,CreateBy,SendTime,SentStatus", " message (nolock)", SystemConst.PageSize, true);
        }

    }
}
