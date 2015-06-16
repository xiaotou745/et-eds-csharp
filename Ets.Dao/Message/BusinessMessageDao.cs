using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using Ets.Model.DataModel.Message;
using Ets.Model.ParameterModel.Common;
using Ets.Model.ParameterModel.Message;

namespace Ets.Dao.Message
{
    /// <summary>
    /// 商户 app通知表   add by caoheyang 20150615
    /// </summary>
    public class BusinessMessageDao : DaoBase
    {
        public BusinessMessageDao()
        {
        }
        /// <summary>
        /// 增加一条记录
        /// </summary>
        public void Insert(ClienterMessage clienterMessage)
        {
            const string insertSql = @"
insert into BusinessMessage(BusinessId,Content,IsRead)
values(@BusinessId,@Content,@IsRead)
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", clienterMessage.ClienterId);
            dbParameters.AddWithValue("Content", clienterMessage.Content);
            dbParameters.AddWithValue("IsRead", clienterMessage.IsRead);
            DbHelper.ExecuteNonQuery(SuperMan_Write, insertSql, dbParameters);
        }

        /// <summary>
        /// 更新消息为已读
        /// </summary>
        public void Update(long id)
        {
            const string updateSql = @"
update  BusinessMessage
set  IsRead=1
where  Id=@Id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }



        /// <summary>
        /// 查询对象
        /// </summary>
        public PageInfo<ClienterMessage> Query(ListBPM search)
        {
            string where = " BusinessId=" + search.BusinessId;
            return new PageHelper().GetPages<ClienterMessage>(SuperMan_Read, search.PageIndex, where,
                "Id desc", " Id,BusinessId,Content,IsRead", " BusinessMessage (nolock)", SystemConst.PageSize, true);
        }
    }
}
