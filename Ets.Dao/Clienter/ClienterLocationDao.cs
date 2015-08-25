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
using Ets.Model.DomainModel.Order;
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
        /// <param name="cityId">城市Id</param>
        /// <param name="deliveryCompanyInfo">物流公司</param>
        /// <returns></returns>
        public IList<AppActiveInfo> GetAppActiveInfos(string cityId, string deliveryCompanyInfo)
        {
            string strSql = string.Format(@"with t as(
select ClienterId,MAX(CreateTime) maxtime from ClienterLocation (Nolock) GROUP BY ClienterId HAVING MAX(CreateTime) >= '{0}'
)
select 2 as UserType, cl.Latitude Latitude,cl.Longitude Longitude,cl.Platform Platform,c.TrueName TrueName,c.PhoneNo Phone from ClienterLocation (Nolock) cl join  t 
on cl.CreateTime=t.maxtime
join clienter (Nolock) c on cl.ClienterId=c.Id ", DateTime.Now.AddMinutes(-ParseHelper.ToInt(GlobalConfigDao.GlobalConfigGet(0).SearchClienterLocationTimeInterval, 0)));
            string where = !string.IsNullOrEmpty(deliveryCompanyInfo) && deliveryCompanyInfo != "0"
                                    ? string.Format(" where c.DeliveryCompanyId='{0}' and c.cityId='{1}'", deliveryCompanyInfo, cityId.Trim())
                                    : string.Format("  where c.CityId='{0}'", cityId.Trim());
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, strSql+where);
            return MapRows<AppActiveInfo>(dt);
        }

        /// <summary>
        /// 获得实时坐标
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        public IList<Location> GetLocationsByTime(DateTime start, DateTime end,int clienterId)
        {
            string strSql = string.Format("select Longitude,Latitude from ClienterLocation (Nolock) where clienterId={0} and CreateTime>='{1}' and CreateTime<='{2}'", clienterId, start, end);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, strSql);
            return MapRows<Location>(dt);
        } 

        #endregion

    }
}
