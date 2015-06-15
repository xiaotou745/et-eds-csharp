using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using Ets.Model.DataModel.Message;

namespace Ets.Dao.Message
{
    /// <summary>
    /// 骑士 app通知表   add by caoheyang 20150615
    /// </summary>
    public class ClienterMessageDao : DaoBase
    {
        public ClienterMessageDao()
        {

        }

        /// <summary>
        /// 增加一条记录
        /// </summary>
        public void Insert(ClienterMessage clienterMessage)
        {
            const string insertSql = @"
insert into ClienterMessage(ClienterId,Content,IsRead)
values(@ClienterId,@Content,@IsRead)
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterMessage.ClienterId);
            dbParameters.AddWithValue("Content", clienterMessage.Content);
            dbParameters.AddWithValue("IsRead", clienterMessage.IsRead);
            DbHelper.ExecuteNonQuery(SuperMan_Write, insertSql, dbParameters);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        public void Update(ClienterMessage clienterMessage)
        {
            const string updateSql = @"
update  ClienterMessage
set  IsRead=@IsRead
where  Id=@Id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", clienterMessage.Id);
            dbParameters.AddWithValue("IsRead", clienterMessage.IsRead);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

    }
}
