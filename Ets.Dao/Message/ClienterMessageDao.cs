using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Extension;
using Ets.Model.DataModel.Message;
using Ets.Model.DomainModel.Message;
using Ets.Model.ParameterModel.Message;
using ETS.Util;

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
        public long Insert(ClienterMessage clienterMessage)
        {
            const string insertSql = @"
insert into ClienterMessage(ClienterId,Content,IsRead)
values(@ClienterId,@Content,@IsRead)
select @@IDENTITY
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterMessage.ClienterId);
            dbParameters.AddWithValue("Content", clienterMessage.Content);
            dbParameters.AddWithValue("IsRead", clienterMessage.IsRead);
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters); //提现单号
            return ParseHelper.ToLong(result);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        public ReadCDM ReadC(long id)
        {
            const string updateSql = @"
update  ClienterMessage
set  IsRead=1
OutPut INSERTED.[Content],INSERTED.[PubDate]
where  Id=@Id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            return DbHelper.QueryForObjectDelegate<ReadCDM>(SuperMan_Write, updateSql, dbParameters,
                dataRow => new ReadCDM
                {
                    Content = dataRow["Content"]==null?"":dataRow["Content"].ToString(),
                    PubDate = Convert.ToDateTime(dataRow["PubDate"]).ToString("yyyy-MM-dd hh:mm"),
                });
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        public PageInfo<ListCDM> Query(ListCPM search)
        {
            string where = " ClienterId=" + search.ClienterId;
            return new PageHelper().GetPages<ListCDM>(SuperMan_Read, search.PageIndex, where,
                "IsRead asc ,id desc ", "Id,Content,IsRead", " ClienterMessage (nolock)", SystemConst.PageSize, true);
        }

        /// <summary>
        /// 查询当前骑士是否有未读消息  add by caoheyang 20150616
        /// </summary>
        public bool HasMessage(int clienterId)
        {
            const string insertSql = @"
select  count(1)
from    dbo.ClienterMessage(nolock)
where   IsRead = 0 and clienterId=@ClienterId 
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterId);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters)) > 0;
        }
    }
}
