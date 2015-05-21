using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using Ets.Model.DataModel.Clienter;
using ETS.Util;

namespace Ets.Dao.Clienter
{
    /// <summary>
    /// 骑士送餐轨迹表数据访问类ClienterLocationDao。
    /// Generate By: tools.etaoshi.com caoheyang
    /// Generate Time: 2015-05-19 10:00:53
    /// </summary>

    public class ClienterLocationDao : DaoBase
    {
        #region ClienterLocationDaoRepos  Members
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="clienterLocation"></param>
        public long Insert(ClienterLocation clienterLocation)
        {
            const string insertSql = @"
insert into ClienterLocation(Longitude,Latitude,ClienterId)
values(@Longitude,@Latitude,@ClienterId)
select @@IDENTITY
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Longitude", clienterLocation.Longitude);
            dbParameters.AddWithValue("Latitude", clienterLocation.Latitude);
            dbParameters.AddWithValue("ClienterId", clienterLocation.ClienterId);
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToLong(result);
        }

        #endregion

    }
}
