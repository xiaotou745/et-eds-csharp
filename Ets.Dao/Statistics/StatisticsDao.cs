using Ets.Model.Common;
using ETS;
using ETS.Dao;
using ETS.Data.Core;
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
            if (Config.ConfigKey("IsFirst") == null)
            {
                where = " and CONVERT(CHAR(10),PubDate,120)=DATEADD(DAY,-1,CONVERT(CHAR(10),GETDATE(),120)) ";//统计昨天数据
            }
            string sql = @"SELECT 
                            (SELECT COUNT(1) FROM clienter(NOLOCK) WHERE [Status]=1) AS RzqsCount, --认证骑士数量
                            (SELECT COUNT(1) FROM clienter(NOLOCK) WHERE [Status]=0) AS DdrzqsCount, --等待认证骑士
                            (SELECT COUNT(Id) AS BusinessCount FROM dbo.business(NOLOCK) WHERE [status]=1) AS BusinessCount,  --商家总数
                            CONVERT(CHAR(10),PubDate,120) AS PubDate, --发布时间
                            SUM(ISNULL(Amount,0)) AS OrderPrice, --订单金额
                            ISNULL(COUNT(o.Id),0) AS MisstionCount,--任务量
                            SUM(ISNULL(OrderCount,0)) AS OrderCount,--订单量
                            ISNULL(SUM(o.Amount*ISNULL(b.BusinessCommission,0)/100+ ISNULL( b.DistribSubsidy ,0)* o.OrderCount),0) AS YsPrice,  -- 应收金额
                            ISNULL( SUM( OrderCommission),0) AS YfPrice  --应付金额
                            FROM dbo.[order](NOLOCK) AS o
                            LEFT JOIN dbo.business(NOLOCK) AS b ON o.businessId=b.Id
                            WHERE  
                            o.[Status]=1 " + where;
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
            if (Config.ConfigKey("IsFirst") == null)
            {
                where = " and CONVERT(CHAR(10),PubDate,120)=DATEADD(DAY,-1,CONVERT(CHAR(10),GETDATE(),120)) ";//统计昨天数据
            }
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
            parm.AddWithValue("@ZeroSubsidyOrderCount", model.OneSubsidyOrderCount);
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
            string sql = @"select 
                        count(distinct clienterId) as ActiveClienter,
                        count(distinct businessId) as ActiveBusiness
                        from dbo.[order] as o 
                        where convert(char(10),PubDate,120)=convert(char(10),getdate(),120) and status<>3";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<HomeCountTitleModel>(dt)[0];
        }
    }
}
