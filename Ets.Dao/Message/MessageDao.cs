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
using ETS.Data.Core;
using System.Data;

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
        /// 获取消息列表
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150617</UpdateTime>
        /// <param name="sentStatus"></param>
        /// <returns></returns>
        public IList<MessageModel> GetMessageList(int sentStatus)
        {
            string querysql = @"  
select id, [Content],PushTarget,PushCity,PushPhone 
from dbo.[Message]
where SentStatus=@SentStatus
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
            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int64, 8, id);
            dbParameters.AddWithValue("@Id", id);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters); 
        }

    }
}
