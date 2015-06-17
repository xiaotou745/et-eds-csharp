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
using Ets.Model.DomainModel.Message;

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
        /// <summary>
        /// 添加消息任务
        /// danny-20150617
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddMessageTask(MessageModelDM model)
        {
            string sql = string.Format(@" 
INSERT INTO [Message]
           ([PushWay]
           ,[MessageType]
           ,[Content]
           ,[SentStatus]
           ,[PushType]
           ,[PushTarget]
           ,[PushCity]
           ,[PushPhone]
           ,[SendType]
           ,[SendTime]
           ,[OverTime]
           ,[CreateBy]
           ,[CreateTime]
           ,[UpdateBy]
           ,[UpdateTime])
     VALUES
           (@PushWay
           ,@MessageType
           ,@CONTENT
           ,@SentStatus
           ,@PushType
           ,@PushTarget
           ,@PushCity
           ,@PushPhone
           ,@SendType
           ,@SendTime
           ,@OverTime
           ,@CreateBy
           ,getdate()
           ,@UpdateBy
           ,getdate());");
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@PushWay", model.PushWay);
            parm.AddWithValue("@MessageType", model.MessageType);
            parm.AddWithValue("@Content", model.Content);
            parm.AddWithValue("@SentStatus", model.SentStatus);
            parm.AddWithValue("@PushType", model.PushType);
            parm.AddWithValue("@PushTarget", model.PushTarget);
            parm.AddWithValue("@PushCity", model.PushCity);
            parm.AddWithValue("@PushPhone", model.PushPhone);
            parm.AddWithValue("@SendType", model.SendType);
            parm.AddWithValue("@SendTime", model.SendTime);
            parm.AddWithValue("@OverTime", model.OverTime);
            parm.AddWithValue("@CreateBy", model.OptUserName);
            parm.AddWithValue("@UpdateBy", model.OptUserName);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

    }
}
