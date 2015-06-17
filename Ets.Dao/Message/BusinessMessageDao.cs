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
using Ets.Model.ParameterModel.Common;
using Ets.Model.ParameterModel.Message;
using ETS.Util;

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
        public ReadBDM ReadB(long id)
        {
            const string updateSql = @"
update  BusinessMessage
set  IsRead=1
OutPut INSERTED.[Content],INSERTED.PubDate
where  Id=@Id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            return DbHelper.QueryForObjectDelegate<ReadBDM>(SuperMan_Write, updateSql, dbParameters,
              dataRow => new ReadBDM
              {
                  Content = dataRow["Content"] == null ? "" : dataRow["Content"].ToString(),
                  PubDate = Convert.ToDateTime(dataRow["PubDate"]).ToString("yyyy-MM-dd hh:mm"),
              });
        }



        /// <summary>
        /// 查询对象
        /// </summary>
        public PageInfo<ListBDM> Query(ListBPM search)
        {
            string where = " BusinessId=" + search.BusinessId;
            return new PageHelper().GetPages<ListBDM>(SuperMan_Read, search.PageIndex, where,
                "IsRead asc ,id desc ", " Id,Content,IsRead", " BusinessMessage (nolock)", SystemConst.PageSize, true);
        }

        /// <summary>
        /// 查询当前商户是否有未读消息  add by caoheyang 20150616
        /// </summary>
        public bool HasMessage(int businessId)
        {
            const string insertSql = @"
select  count(1)
from    dbo.BusinessMessage
where   IsRead = 0 and businessId=@BusinessId 
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", businessId);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters)) > 0;
        }
    }
}
