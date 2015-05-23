using ETS.Data.Generic;
using ETS.Extension;
using Ets.Model.Common;
using ETS;
using ETS.Dao;
using ETS.Data.Core;
using Ets.Model.DomainModel.Statistics;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Statistics
{
    public class StatisticsDao : DaoBase
    {
        /// <summary>
        /// 获取统计数据
        /// 窦海超
        /// 2015年3月26日 14:48:29
        /// </summary>
        /// <returns></returns>
        public IList<HomeCountTitleModel> GetStatistics()
        {
            string where = string.Empty;
            //if (Config.ConfigKey("IsFirst") == null)
            //{
            where = " and CONVERT(CHAR(10),PubDate,120)=DATEADD(DAY,-1,CONVERT(CHAR(10),GETDATE(),120)) ";//统计昨天数据
            //}
            string sql = @"SELECT 
                            (SELECT COUNT(1) FROM clienter(NOLOCK) WHERE [Status]=1) AS RzqsCount, --认证骑士数量
                            (SELECT COUNT(1) FROM clienter(NOLOCK) WHERE [Status]=0) AS DdrzqsCount, --等待认证骑士
                            (SELECT COUNT(Id) AS BusinessCount FROM dbo.business(NOLOCK) WHERE [status]=1) AS BusinessCount,  --商家总数
                            CONVERT(CHAR(10),PubDate,120) AS PubDate, --发布时间
                            SUM(ISNULL(Amount,0)) AS OrderPrice, --订单金额
                            ISNULL(COUNT(o.Id),0) AS MisstionCount,--任务量
                            SUM(ISNULL(OrderCount,0)) AS OrderCount,--订单量
                            SUM(isnull(o.SettleMoney,0)) AS YsPrice,  -- 应收金额
                            SUM(ISNULL(OrderCommission,0)) AS YfPrice  --应付金额
                            FROM dbo.[order](NOLOCK) AS o
                            LEFT JOIN dbo.business(NOLOCK) AS b ON o.businessId=b.Id
                            WHERE  
                            o.[Status]<>3 " + where;
            sql += " GROUP BY CONVERT(CHAR(10),PubDate,120) ORDER BY PubDate ASC";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<HomeCountTitleModel>(dt);
        }

        /// <summary>
        /// 获取统计数据
        /// 窦海超
        /// 2015年3月26日 14:48:29
        /// </summary>
        /// <returns></returns>
        public IList<HomeCountTitleModel> GetSubsidyOrderCountStatistics()
        {
            string where = string.Empty;
            //if (Config.ConfigKey("IsFirst") == null)
            //{
            where = " and CONVERT(CHAR(10),PubDate,120)=DATEADD(DAY,-1,CONVERT(CHAR(10),GETDATE(),120)) ";//统计昨天数据
            //}
            string sql = @"SELECT CONVERT(CHAR(10),PubDate,120) AS PubDate, --发布时间
                                  sum(case when DealCount=0 then 1 else 0 end ) as ZeroSubsidyOrderCount,
                                  sum(case when DealCount=1 then 1 else 0 end ) as OneSubsidyOrderCount,
                                  sum(case when DealCount=2 then 1 else 0 end ) as TwoSubsidyOrderCount,
                                  sum(case when DealCount=3 then 1 else 0 end ) as ThreeSubsidyOrderCount
                           FROM [order](NOLOCK) AS o
                           WHERE   o.[Status]<>3 " + where;
            sql += " GROUP BY CONVERT(CHAR(10),PubDate,120) ORDER BY PubDate ASC";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<HomeCountTitleModel>(dt);
        }

        /// <summary>
        /// 获取当前数据是否存在 
        /// 窦海超
        /// 2015年3月26日 14:48:15
        /// </summary>
        /// <param name="Time">当前时间</param>
        /// <returns></returns>
        public bool CheckDateStatistics(string Time)
        {
            string sql = @"SELECT COUNT(1) FROM STATISTIC(nolock) WHERE CONVERT(CHAR(10),InsertTime,120)='" + Time + "'";
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql), 0) > 0 ? true : false;
        }

        /// <summary>
        /// 新增统计数据
        /// 窦海超
        /// 2015年3月26日 15:03:15
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertDataStatistics(HomeCountTitleModel model)
        {
            string sql = @"INSERT INTO [dbo].[Statistic]
                            ([InsertTime]
                            ,[BusinessCount]
                            ,[RzqsCount]
                            ,[DdrzqsCount]
                            ,[OrderPrice]
                            ,[MisstionCount]
                            ,[OrderCount]
                            ,[BusinessAverageOrderCount]
                            ,[MissionAverageOrderCount]
                            ,[ClienterAverageOrderCount]
                            ,[YsPrice]
                            ,[YfPrice]
                            ,[YkPrice]
                            ,[ZeroSubsidyOrderCount]
                            ,[OneSubsidyOrderCount]
                            ,[TwoSubsidyOrderCount]
                            ,[ThreeSubsidyOrderCount]
                            ,[ActiveBusiness]
                            ,[ActiveClienter])
                            VALUES
                            (@InsertTime
                            ,@BusinessCount
                            ,@RzqsCount
                            ,@DdrzqsCount
                            ,@OrderPrice
                            ,@MisstionCount
                            ,@OrderCount
                            ,@BusinessAverageOrderCount
                            ,@MissionAverageOrderCount
                            ,@ClienterAverageOrderCount
                            ,@YsPrice
                            ,@YfPrice
                            ,@YkPrice
                            ,@ZeroSubsidyOrderCount
                            ,@OneSubsidyOrderCount
                            ,@TwoSubsidyOrderCount
                            ,@ThreeSubsidyOrderCount
                            ,@ActiveBusiness
                            ,@ActiveClienter)
                            ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@InsertTime", model.PubDate);
            parm.AddWithValue("@BusinessCount", model.BusinessCount);
            parm.AddWithValue("@RzqsCount", model.RzqsCount);
            parm.AddWithValue("@DdrzqsCount", model.DdrzqsCount);
            parm.AddWithValue("@OrderPrice", model.OrderPrice);
            parm.AddWithValue("@MisstionCount", model.MisstionCount);
            parm.AddWithValue("@OrderCount", model.OrderCount);
            parm.AddWithValue("@BusinessAverageOrderCount", model.BusinessAverageOrderCount);
            parm.AddWithValue("@MissionAverageOrderCount", model.MissionAverageOrderCount);
            parm.AddWithValue("@ClienterAverageOrderCount", model.ClienterAverageOrderCount);
            parm.AddWithValue("@YsPrice", model.YsPrice);
            parm.AddWithValue("@YfPrice", model.YfPrice);
            parm.AddWithValue("@YkPrice", model.YkPrice);
            parm.AddWithValue("@ZeroSubsidyOrderCount", model.ZeroSubsidyOrderCount);
            parm.AddWithValue("@OneSubsidyOrderCount", model.OneSubsidyOrderCount);
            parm.AddWithValue("@TwoSubsidyOrderCount", model.TwoSubsidyOrderCount);
            parm.AddWithValue("@ThreeSubsidyOrderCount", model.ThreeSubsidyOrderCount);
            parm.AddWithValue("@ActiveBusiness", model.ActiveBusiness);
            parm.AddWithValue("@ActiveClienter", model.ActiveClienter);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;

        }

        /// <summary>
        /// 获取当天
        /// 未完成任务量
        /// 未被抢任务量
        /// 窦海超
        /// 2015年4月8日 14:00:14
        /// </summary>
        /// <returns></returns>
        public HomeCountTitleModel GetCurrentUnFinishOrderinfo()
        {
            string sql = @"
                        select 
                        sum(case when Status=2 then 1 else 0 end) UnfinishedMissionCount,--未完成任务量
                        sum(case when Status=0 then 1 else 0 end) UnGrabMissionCount--未被抢任务量
                        from dbo.[order](nolock) as o
                        where convert(char(10),PubDate,120)=convert(char(10),getdate(),120) 
                        and Status<>4
                        group by convert(char(10),PubDate,120)
                            ";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            return MapRows<HomeCountTitleModel>(dt)[0];
        }

        /// <summary>
        /// 获取当天活跃商家和骑士
        /// 窦海超
        /// 2015年4月8日 14:57:27
        /// </summary>
        /// <returns></returns>
        public HomeCountTitleModel GetCurrentActiveBussinessAndClienter()
        {
            string sql = @"
select 
count(distinct clienterId) as ActiveClienter,
count(distinct businessId) as ActiveBusiness
from dbo.[order](nolock) as o 
where convert(char(10),o.PubDate,120) = convert(char(10), dateadd(day,-1,getdate()) , 120) and status<>3
";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<HomeCountTitleModel>(dt)[0];
        }


        /// <summary>
        /// 获取当日数据统计
        /// danny-20150422
        /// </summary>
        /// <returns></returns>
        public HomeCountTitleModel GetCurrentDateModel()
        {
            string sql = @"
                            with t as(
                                        SELECT 
                                          CONVERT(CHAR(10),GETDATE(),120) PubDate,
                                          ISNULL(SUM(CASE WHEN [Status]=1 THEN 1 ELSE 0 END),0) AS RzqsCount, --认证骑士数量
                                          ISNULL(SUM(CASE WHEN [Status]=0 THEN 1 ELSE 0 END),0) AS DdrzqsCount --等待认证骑士
                                        FROM dbo.clienter(NOLOCK)
                            )
                            ,t2 as (
                                        SELECT 
                                          CONVERT(CHAR(10),GETDATE(),120) PubDate,
                                          COUNT(Id) AS BusinessCount 
                                        FROM dbo.business(NOLOCK) 
                                        WHERE [status]=1
                            )
                            ,t3 AS(
                                        select 
                                          CONVERT(CHAR(10),GETDATE(),120) PubDate,
                                          count(distinct clienterId) as ActiveClienter,
                                          count(distinct businessId) as ActiveBusiness
                                        from dbo.[order](nolock) as o 
                                        where convert(char(10),o.PubDate ,120) = convert(char(10), getdate(), 120) and status<>3
                              )
                            ,t4 AS(
                                        select 
                                          CONVERT(CHAR(10),GETDATE(),120) PubDate,
                                          sum(case when Status=2 then 1 else 0 end) UnfinishedMissionCount,--未完成任务量
                                          sum(case when Status=0 then 1 else 0 end) UnGrabMissionCount--未被抢任务量
                                        from dbo.[order](nolock) as o
                                        where convert(char(10),PubDate,120)=convert(char(10),getdate(),120) 
                                        and Status<>3
                                        group by convert(char(10),PubDate,120)
                              )
                            ,t5 AS(
                                        SELECT  PubDate ,
                                                ISNULL(SUM(CASE when a.DealCount=0 THEN OrderCount END),0) ZeroSubsidyOrderCount,
                                                ISNULL(SUM(CASE when a.DealCount=1 THEN OrderCount END),0) OneSubsidyOrderCount,
                                                ISNULL(SUM(CASE when a.DealCount=2 THEN OrderCount END),0) TwoSubsidyOrderCount,
                                                ISNULL(SUM(CASE when a.DealCount=3 THEN OrderCount END),0) ThreeSubsidyOrderCount
                                        FROM (
                                              SELECT 
                                                CONVERT(CHAR(10),PubDate,120) AS PubDate, --发布时间
                                                COUNT(1) AS OrderCount,--订单量
                                                DealCount
                                              FROM dbo.[order](NOLOCK) AS o
                                              WHERE  o.[Status]<>3 
                                                AND convert(char(10),PubDate,120)=convert(char(10),getdate(),120) 
                                              GROUP BY CONVERT(CHAR(10),PubDate,120),DealCount 
                                              ) a
                                        group by PubDate
                             )
                            ,t6 AS(
                                        SELECT 
                                          CONVERT(CHAR(10),PubDate,120) AS PubDate, 
                                          SUM(ISNULL(Amount,0)) AS OrderPrice, --订单金额
                                          ISNULL(COUNT(o.Id),0) AS MisstionCount,--总任务量
                                          SUM(ISNULL(OrderCount,0)) AS OrderCount,--总订单量
                                          SUM(ISNULL(o.SettleMoney,0)) AS YsPrice,  -- 应收金额
                                          SUM( ISNULL(OrderCommission,0)) AS YfPrice  --应付金额  --应付金额
                                        FROM dbo.[order](NOLOCK) AS o
                                          LEFT JOIN dbo.business(NOLOCK) AS b ON o.businessId=b.Id
                                         WHERE  o.[Status]<>3 
                                          AND convert(char(10),PubDate,120)=convert(char(10),getdate(),120) 
                                        GROUP BY CONVERT(CHAR(10),PubDate,120)
                              )
                            select t.PubDate,
                              isnull(t.RzqsCount,0)RzqsCount,
                              isnull(t.DdrzqsCount,0) DdrzqsCount,
                              isnull(t2.BusinessCount,0) BusinessCount,
                              isnull(t3.ActiveClienter,0) ActiveClienter,
                              isnull(t3.ActiveBusiness,0) ActiveBusiness,
                              isnull(t4.UnfinishedMissionCount,0) UnfinishedMissionCount,
                              isnull(t4.UnGrabMissionCount,0) UnGrabMissionCount,
                              isnull(t5.ZeroSubsidyOrderCount,0) ZeroSubsidyOrderCount,
                              isnull(t5.OneSubsidyOrderCount,0) OneSubsidyOrderCount,
                              isnull(t5.TwoSubsidyOrderCount,0) TwoSubsidyOrderCount,
                              isnull(t5.ThreeSubsidyOrderCount,0) ThreeSubsidyOrderCount,
                              isnull(t6.OrderPrice,0) OrderPrice,
                              isnull(t6.MisstionCount,0) MisstionCount,
                              isnull(t6.OrderCount,0) OrderCount,
                              isnull(t6.YsPrice,0) YsPrice,
                              isnull(t6.YfPrice,0) YfPrice
                            from t 
	                            left join t2 on t.PubDate = t2.PubDate 
                                left join t3 on t.PubDate = t3.PubDate
                                left join t4 on t.PubDate = t4.PubDate
                                left join t5 on t.PubDate = t5.PubDate
                                left join t6 on t.PubDate = t6.PubDate
                                ";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            return MapRows<HomeCountTitleModel>(dt)[0];
        }

        #region 活跃商家、骑士数量统计



        /// <summary>
        /// 活跃数量统计
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public IList<ActiveBusinessClienterInfo> QueryActiveBusinessClienter(ParamActiveInfo queryInfo)
        {
            #region sql text.
            const string SQL_TEXT = @"
declare @BusinessCount int 
declare @ClienterCount int

--昨天通过审核商家数量
select @BusinessCount=count(1)
from dbo.business b(nolock)
where b.InsertTime<@enddate
	and b.Status=1

--昨天通过审核骑士数量
select @ClienterCount=count(1)
from dbo.clienter c(nolock)
where c.InsertTime<@enddate
	and c.Status=1
	and c.City is not null

--订单相关统计
select convert(char(10),o.PubDate,120) 'Date',
	count(distinct o.businessId) 'ActiveBusinessCount',
	count(distinct o.clienterId) 'ActiveClienterCount' into #tempOrder
from dbo.[order] o(nolock)
where o.Status=1
	and o.PubDate between @startdate and @enddate
group by convert(char(10),o.PubDate,120)

--	商户结算金额	订单骑士佣金	
select o.Date,
	@BusinessCount 'BusinessCount',
	o.ActiveBusinessCount,o.ActiveBusinessCount*1.0/@BusinessCount 'ActiveBusinessRate',
	@ClienterCount 'ClienterCount',
	o.ActiveClienterCount,
	o.ActiveClienterCount*1.0/@ClienterCount 'ActiveClienterRate'
from #tempOrder o
order by o.Date desc, o.ActiveClienterCount desc";
            #endregion

            var dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("startdate", DbType.DateTime).Value = queryInfo.StartDate.Value;
            dbParameters.Add("enddate", DbType.DateTime).Value = queryInfo.EndDate.Value;

            return DbHelper.QueryWithRowMapper(SuperMan_Read, SQL_TEXT, dbParameters,
                new ActiveBusinessClienterInfoRowMapper());
        }

        public IList<ActiveBusinessClienterInfo> QueryCityActiveBusinessClienter(ParamActiveInfo queryInfo)
        {
            #region SQL text

            const string SQL_TEXT = @"
--昨天通过审核商家数量
select b.City, count(1) 'BusinessCount' into #tempBusiness
from dbo.business b(nolock)
where b.InsertTime<@enddate
	and b.Status=1
group by b.City

--昨天通过审核骑士数量
select c.City,count(1) 'ClienterCount' into #tempClienter
from dbo.clienter c(nolock)
where c.InsertTime<@enddate
	and c.Status=1
	and c.City is not null
group by c.City

--订单相关统计
select convert(char(10),o.PubDate,120) 'Date',
	o.ReceviceCity,
	count(distinct o.businessId) 'ActiveBusinessCount',
	count(distinct o.clienterId) 'ActiveClienterCount' into #tempOrder
from dbo.[order] o(nolock)
where o.Status=1
	and o.PubDate between @startdate and @enddate
group by convert(char(10),o.PubDate,120),o.ReceviceCity

--	商户结算金额	订单骑士佣金	
select o.Date,
	o.ReceviceCity 'City',
	tb.BusinessCount,o.ActiveBusinessCount,
    o.ActiveBusinessCount*1.0/tb.BusinessCount 'ActiveBusinessRate',
	tc.ClienterCount,o.ActiveClienterCount,
    o.ActiveClienterCount*1.0/tc.ClienterCount 'ActiveClienterRate'
from #tempOrder o
	left join #tempBusiness tb on o.ReceviceCity=tb.City
	left join #tempClienter tc on o.ReceviceCity=tc.City
order by o.Date desc, o.ActiveClienterCount desc";
            #endregion

            var dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("startdate", DbType.DateTime).Value = queryInfo.StartDate.Value;
            dbParameters.Add("enddate", DbType.DateTime).Value = queryInfo.EndDate.Value;

            return DbHelper.QueryWithRowMapper(SuperMan_Read, SQL_TEXT, dbParameters,
                new ActiveBusinessClienterInfoRowMapper());
        }

        /// <summary>
        /// 绑定对象
        /// </summary>
        private class ActiveBusinessClienterInfoRowMapper : IDataTableRowMapper<ActiveBusinessClienterInfo>
        {
            public ActiveBusinessClienterInfo MapRow(DataRow dataReader)
            {
                var result = new CityActiveBusinessClienterInfo();

                result.Date = dataReader["Date"].ToString();
                result.BusinessCount = ParseHelper.ToInt(dataReader["BusinessCount"], 0);
                result.ActiveBusinessCount = ParseHelper.ToInt(dataReader["ActiveBusinessCount"], 0);
                result.ActiveBusinessRate = ParseHelper.ToDecimal(dataReader["ActiveBusinessRate"], 0);

                result.ClienterCount = ParseHelper.ToInt(dataReader["ClienterCount"], 0);
                result.ActiveClienterCount = ParseHelper.ToInt(dataReader["ActiveClienterCount"], 0);
                result.ActiveClienterRate = ParseHelper.ToDecimal(dataReader["ActiveClienterRate"], 0);

                if (dataReader.HasColumn("City"))
                {
                    result.City = dataReader["City"].ToString();
                }

                return result;
            }
        }
        
        #endregion
    }
}
