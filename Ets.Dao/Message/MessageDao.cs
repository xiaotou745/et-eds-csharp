﻿using System;
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
using ETS.Data.Core;
using System.Data;
using ETS.Extension;

namespace Ets.Dao.Message
{
    /// <summary>
    /// 消息dao   add caoheyang 20150516
    /// </summary>
    public class MessageDao : DaoBase
    {
        /// <summary>
        /// web后台列表页功能 add by caoheyang 20150616
        /// </summary>
        public async Task<PageInfo<MessageModel>> WebList(WebListSearch model)
        {
            string where = " 1=1 ";
            where = where + (model.SendType == -1 ? "" : string.Format(" and SendType={0}", model.SendType));
            where = where + (model.MessageType == -1 ? "" : string.Format(" and MessageType={0}", model.MessageType));
            where = where + (model.PushWay == -1 ? "" : string.Format(" and PushWay={0}", model.PushWay));
            where = where + (model.SentStatus == -1 ? "" : string.Format(" and SentStatus={0}", model.SentStatus));
            where = where + (model.PubDateStart == null ? "" : string.Format(" and SendTime>='{0} 00:00:00'", model.PubDateStart.Value.ToString("yyyy-MM-dd")));
            where = where + (model.PubDateEnd == null ? "" : string.Format(" and SendTime<='{0} 23:59:59'", model.PubDateEnd.Value.ToString("yyyy-MM-dd")));
            return new PageHelper().GetPages<MessageModel>(SuperMan_Read, model.PageIndex, where, "Id desc", "Id,PushWay,MessageType,SendType,substring(Content,1,15) as Content,UpdateBy,SendTime,SentStatus,CreateTime", " message (nolock)", SystemConst.PageSize, true);
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
            parm.AddWithValue("@PushCity", model.PushCity??"");
            parm.AddWithValue("@PushPhone", model.PushPhone);
            parm.AddWithValue("@SendType", model.SendType);
            parm.AddWithValue("@SendTime", model.SendTime.ToString("yyyy-MM-dd HH:mm:ss").StartsWith("0001") ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : model.SendTime.ToString("yyyy-MM-dd HH:mm:ss"));
            parm.AddWithValue("@OverTime", model.OverTime);
            parm.AddWithValue("@CreateBy", model.OptUserName);
            parm.AddWithValue("@UpdateBy", model.OptUserName);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 修改消息任务
        /// danny-20150617
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyMessageTask(MessageModelDM model)
        {
            string sql = @"UPDATE Message 
                            SET PushWay=@PushWay,
                                MessageType=@MessageType,
                                Content=@Content,
                                PushType=@PushType,
                                
                                           ";
            if (model.PushType ==1)
            {
                sql += " PushTarget=@PushTarget,PushCity=@PushCity, ";
            }
            else
            {
                sql += " PushPhone=@PushPhone, ";
            }
            if(model.SendType==2)
            {
                sql += " SendTime=@SendTime, ";
            }
            sql += @" SendType=@SendType
                        WHERE  Id = @Id;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@PushWay", model.PushWay);
            parm.AddWithValue("@MessageType", model.MessageType);
            parm.AddWithValue("@Content", model.Content);
            parm.AddWithValue("@PushType", model.PushType);
            parm.AddWithValue("@PushTarget", model.PushTarget);
            parm.AddWithValue("@PushCity", model.PushCity);
            parm.AddWithValue("@PushPhone", model.PushPhone);
            parm.AddWithValue("@SendTime", model.SendTime.ToString("yyyy-MM-dd HH:mm:ss").StartsWith("0001") ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : model.SendTime.ToString("yyyy-MM-dd HH:mm:ss"));
            parm.AddWithValue("@SendType", model.SendType);
            parm.AddWithValue("@Id", model.Id);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150617</UpdateTime>
        /// <param name="sentStatus"></param>
        /// <returns></returns>
        public IList<MessageModel> GetMessageList(int sentStatus)
        {
            string querysql = @"  
select id, PushWay,[Content],PushTarget,PushCity,PushPhone 
from dbo.[Message](nolock)
where SentStatus=0 or SentStatus=1 and GETDATE()>=sendTime
order by SendType";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@SentStatus", sentStatus);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, querysql, dbParameters);
            return MapRows<MessageModel>(dt);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150617</UpdateTime>
        /// <param name="sentStatus"></param>
        /// <returns></returns>
        public void Update(long id)
        {
            const string updateSql = @"
update  message
set  SentStatus=2,OverTime=getdate()
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("Id", DbType.Int64, 8).Value = id;
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }
        /// <summary>
        /// 根据消息Id获取消息信息
        /// danny-20150617
        /// </summary>
        /// <param name="messageId">消息Id</param>
        /// <returns></returns>
        public MessageModel GetMessageById(int messageId)
        {
            string selSql = @" 
SELECT [Id]
      ,[PushWay]
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
      ,[UpdateTime]
  FROM [Message] msg WITH(NOLOCK)
  WHERE Id=@MessageId;  ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@MessageId", messageId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql, parm));
            if (dt != null && dt.Rows.Count > 0)
                return DataTableHelper.ConvertDataTableList<MessageModel>(dt)[0];
            return null;
        }

        /// <summary>
        /// 取消发布 add by caoheyang  20150617
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateby"></param>
        /// <returns></returns>
        public async Task<int> CanelMessage(long id, string updateby)
        {
            const string updateSql = @"
update  message
set  SentStatus=3,OverTime=getdate(),UpdateTime=getdate(),UpdateBy=@UpdateBy
where  Id=@Id and SentStatus=0";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("UpdateBy", updateby);
            dbParameters.AddWithValue("@Id", id);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }
    }
}
