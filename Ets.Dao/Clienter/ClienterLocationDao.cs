using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using Ets.Dao.GlobalConfig;
using ETS.Data.Core;
using ETS.Data.PageData;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel.Statistics;
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
insert into ClienterLocation(Longitude,Latitude,ClienterId,Platform)
values(@Longitude,@Latitude,@ClienterId,@Platform)
select @@IDENTITY
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Longitude", clienterLocation.Longitude);
            dbParameters.AddWithValue("Latitude", clienterLocation.Latitude);
            dbParameters.AddWithValue("ClienterId", clienterLocation.ClienterId);
            dbParameters.AddWithValue("Platform", clienterLocation.Platform);
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToLong(result);
        }

        /// <summary>
        /// 获得骑士app启动热力图
        /// </summary>
        /// <param name="cityName">城市名</param>
        /// <returns></returns>
        public IList<AppActiveInfo> GetAppActiveInfos(string cityName)
        {
            string strSql =string.Format(@"with t as(
select ClienterId,MAX(CreateTime) maxtime from ClienterLocation (Nolock) GROUP BY ClienterId HAVING MAX(CreateTime) > '{0}'
)
select 2 as UserType, cl.Latitude Latitude,cl.Longitude Longitude,cl.Platform Platform,c.TrueName TrueName,c.PhoneNo Phone from ClienterLocation (Nolock) cl join  t 
on cl.CreateTime=t.maxtime
join clienter (Nolock) c on cl.ClienterId=c.Id where c.City like '{1}%';", DateTime.Now.AddMinutes(-ParseHelper.ToInt(int.Parse(GlobalConfigDao.GlobalConfigGet(0).AllFinishedOrderUploadTimeInterval),0)), cityName);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, strSql);
            return MapRows<AppActiveInfo>(dt);
        }

        #endregion

    }
}
