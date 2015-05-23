using System;
using System.Collections.Generic;
using System.Data;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.Generic;
using ETS.Extension;
using Ets.Model.DomainModel.Statistics;
using ETS.Util;

namespace Ets.Dao.Statistics
{
    public class OrderStatisticsDao : DaoBase
    {
        #region 订单时间间隔统计
        /// <summary>
        /// 获取订单时间间隔统计
        /// </summary>
        /// <param name="paramOrderCompleteTimeSpan"></param>
        /// <returns></returns>
        public IList<OrderCompleteTimeSpanInfo> QueryOrderCompleteTimeSpan(
            ParamOrderCompleteTimeSpan paramOrderCompleteTimeSpan)
        {
            #region sql text
            const string SQL_TEXT = @"
select convert(char(10),o.PubDate,120) 'Date',
	count(1) 'TaskCount',
	sum(case when datediff(second,o.PubDate,o.ActualDoneDate)<60*5 then 1 else 0 end) 'LessThanFiveTaskCount',
	sum(case when datediff(second,o.PubDate,o.ActualDoneDate)<60*5 then 1 else 0 end)/ (count(1)*1.0) 'LessThanFiveRate' ,
	sum(case when datediff(second,o.PubDate,o.ActualDoneDate) between 60*5 and 60*10 then 1 else 0 end) 'FiveToTenTaskCount',
	sum(case when datediff(second,o.PubDate,o.ActualDoneDate) between 60*5 and 60*10 then 1 else 0 end) / (count(1)*1.0) 'FiveToTenRate',
	sum(case when datediff(second,o.PubDate,o.ActualDoneDate) between 601 and 60*15 then 1 else 0 end) 'TenToFifteenTaskCount',
	sum(case when datediff(second,o.PubDate,o.ActualDoneDate) between 601 and 60*15 then 1 else 0 end) / (count(1)*1.0) 'TenToFifteenRate',
	sum(case when datediff(second,o.PubDate,o.ActualDoneDate)>60*15 then 1 else 0 end) 'GreaterThanFifteenCount',
	sum(case when datediff(second,o.PubDate,o.ActualDoneDate)>60*15 then 1 else 0 end)/ (count(1)*1.0)  'GreaterThanFifteenRate',
	sum(case when datediff(second,o.PubDate,o.ActualDoneDate)>60*120 then 1 else 0 end) 'GreaterThanTwoHoursCount',
	sum(case when datediff(second,o.PubDate,o.ActualDoneDate)>60*1440 then 1 else 0 end) 'GreaterThanOneDayCount'
from dbo.[order] o(nolock)
where o.Status=1
	and o.PubDate between @StartTime and @EndTime
group by convert(char(10),o.PubDate,120)
order by convert(char(10),o.PubDate,120) desc";
            #endregion

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("StartTime", DbType.DateTime).Value = paramOrderCompleteTimeSpan.StartDate.Value;
            dbParameters.Add("EndTime", DbType.DateTime).Value = paramOrderCompleteTimeSpan.EndDate.Value;

            return DbHelper.QueryWithRowMapper(SuperMan_Read, SQL_TEXT, dbParameters,
                new OrderCompleteTimeSpanInfoRowMapper());
        }

        /// <summary>
        /// 获取各城市时间间隔统计
        /// </summary>
        /// <param name="paramOrderCompleteTimeSpan"></param>
        /// <returns></returns>
        public IList<OrderCompleteTimeSpanInfo> QueryCityCompleteTimeSpan(
            ParamOrderCompleteTimeSpan paramOrderCompleteTimeSpan)
        {
            #region sql text
            const string SQL_TEXT = @"
with t as(
select convert(char(10),o.PubDate,120) 'date',count(1) 'taskCount'
from dbo.[order] o(nolock)
where o.Status=1
	and o.PubDate between @StartTime and @EndTime
group by convert(char(10),o.PubDate,120)
)

--每天各城市在不同处理时长的订单数量
select temp.date 'Date',t.taskCount 'TaskCount',temp.ReceviceCity 'City',temp.cityCount 'CityTaskCount',
    temp.cityCount*1.0 / t.taskCount 'CityTaskRate',
	temp.a5 'LessThanFiveTaskCount',temp.a5r 'LessThanFiveRate',temp.a10 'FiveToTenTaskCount',temp.a10r 'FiveToTenRate',
	temp.a15 'TenToFifteenTaskCount',temp.a15r 'TenToFifteenRate',temp.a155 'GreaterThanFifteenCount',temp.a155r 'GreaterThanFifteenRate',
	temp.a120 'GreaterThanTwoHoursCount',temp.a1440 'GreaterThanOneDayCount'
from 
(
	select convert(char(10),o.PubDate,120) 'date',o.ReceviceCity,count(1) 'cityCount',
		sum(case when datediff(second,o.PubDate,o.ActualDoneDate)<60*5 then 1 else 0 end) 'a5',
		sum(case when datediff(second,o.PubDate,o.ActualDoneDate)<60*5 then 1 else 0 end)/ (count(1)*1.0) 'a5r' ,
		sum(case when datediff(second,o.PubDate,o.ActualDoneDate) between 60*5 and 60*10 then 1 else 0 end) 'a10',
		sum(case when datediff(second,o.PubDate,o.ActualDoneDate) between 60*5 and 60*10 then 1 else 0 end) / (count(1)*1.0) 'a10r',
		sum(case when datediff(second,o.PubDate,o.ActualDoneDate) between 601 and 60*15 then 1 else 0 end) 'a15',
		sum(case when datediff(second,o.PubDate,o.ActualDoneDate) between 601 and 60*15 then 1 else 0 end) / (count(1)*1.0) 'a15r',
		sum(case when datediff(second,o.PubDate,o.ActualDoneDate)>60*15 then 1 else 0 end) 'a155',
		sum(case when datediff(second,o.PubDate,o.ActualDoneDate)>60*15 then 1 else 0 end)/ (count(1)*1.0)  'a155r',
		sum(case when datediff(second,o.PubDate,o.ActualDoneDate)>60*120 then 1 else 0 end)  'a120',
		sum(case when datediff(second,o.PubDate,o.ActualDoneDate)>60*1440 then 1 else 0 end)  'a1440'
	from dbo.[order] o(nolock)
	where o.Status=1
		and o.PubDate between @StartTime and @EndTime
	group by convert(char(10),o.PubDate,120), o.ReceviceCity
	--order by convert(char(10),o.PubDate,120),count(1) desc
) as temp
	join t on t.date = temp.date
order by temp.date,temp.cityCount desc";
            #endregion

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("StartTime", DbType.DateTime).Value = paramOrderCompleteTimeSpan.StartDate.Value;
            dbParameters.Add("EndTime", DbType.DateTime).Value = paramOrderCompleteTimeSpan.EndDate.Value;

            return DbHelper.QueryWithRowMapper(SuperMan_Read, SQL_TEXT, dbParameters,
                new OrderCompleteTimeSpanInfoRowMapper());
        }

        #region  Nested type: businessRowMapper

        /// <summary>
        /// 绑定对象
        /// </summary>
        private class OrderCompleteTimeSpanInfoRowMapper : IDataTableRowMapper<OrderCompleteTimeSpanInfo>
        {
            public OrderCompleteTimeSpanInfo MapRow(DataRow dataReader)
            {
                var result = new CityOrderCompleteTimeSpanInfo();

                result.Date = dataReader["Date"].ToString();
                result.TaskCount = ParseHelper.ToInt(dataReader["TaskCount"], 0);

                result.LessThanFiveTaskCount = ParseHelper.ToInt(dataReader["LessThanFiveTaskCount"], 0);
                result.LessThanFiveRate = ParseHelper.ToDecimal(dataReader["LessThanFiveRate"], 0);

                result.FiveToTenTaskCount = ParseHelper.ToInt(dataReader["FiveToTenTaskCount"], 0);
                result.FiveToTenRate = ParseHelper.ToDecimal(dataReader["FiveToTenRate"], 0);

                result.TenToFifteenTaskCount = ParseHelper.ToInt(dataReader["TenToFifteenTaskCount"], 0);
                result.TenToFifteenRate = ParseHelper.ToDecimal(dataReader["TenToFifteenRate"], 0);

                result.GreaterThanFifteenCount = ParseHelper.ToInt(dataReader["GreaterThanFifteenCount"], 0);
                result.GreaterThanFifteenRate = ParseHelper.ToDecimal(dataReader["GreaterThanFifteenRate"], 0);

                result.GreaterThanTwoHoursCount = ParseHelper.ToInt(dataReader["GreaterThanTwoHoursCount"], 0);
                result.GreaterThanOneDayCount = ParseHelper.ToInt(dataReader["GreaterThanOneDayCount"], 0);

                if (dataReader.HasColumn("City"))
                {
                    result.City = dataReader["City"].ToString();
                }
                if (dataReader.HasColumn("CityTaskCount"))
                {
                    result.CityTaskCount = ParseHelper.ToInt(dataReader["CityTaskCount"], 0);
                }
                if (dataReader.HasColumn("CityTaskRate"))
                {
                    result.CityTaskRate = ParseHelper.ToDecimal(dataReader["CityTaskRate"], 0);
                }

                return result;
            }
        }

        #endregion
        #endregion

        #region 各小时任务数量统计

        /// <summary>
        /// 各小时任务数量统计总计
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public IList<TaskStatisticsPerHourInfo> QueryTaskCountPerHour(ParamTaskPerHour queryInfo)
        {
            #region sql text
            const string SQL_TEXT = @"
select convert(char(10),o.PubDate,120) 'date',
	sum(case Datename(hour,o.PubDate) when 0 then 1 else 0 end) c0,
	sum(case Datename(hour,o.PubDate) when 1 then 1 else 0 end) c1,
	sum(case Datename(hour,o.PubDate) when 2 then 1 else 0 end) c2,
	sum(case Datename(hour,o.PubDate) when 3 then 1 else 0 end) c3,
	sum(case Datename(hour,o.PubDate) when 4 then 1 else 0 end) c4,
	sum(case Datename(hour,o.PubDate) when 5 then 1 else 0 end) c5,
	sum(case Datename(hour,o.PubDate) when 6 then 1 else 0 end) c6,
	sum(case Datename(hour,o.PubDate) when 7 then 1 else 0 end) c7,
	sum(case Datename(hour,o.PubDate) when 8 then 1 else 0 end) c8,
	sum(case Datename(hour,o.PubDate) when 9 then 1 else 0 end) c9,
	sum(case Datename(hour,o.PubDate) when 10 then 1 else 0 end) c10,
	sum(case Datename(hour,o.PubDate) when 11 then 1 else 0 end) c11,
	sum(case Datename(hour,o.PubDate) when 12 then 1 else 0 end) c12,
	sum(case Datename(hour,o.PubDate) when 13 then 1 else 0 end) c13,
	sum(case Datename(hour,o.PubDate) when 14 then 1 else 0 end) c14,
	sum(case Datename(hour,o.PubDate) when 15 then 1 else 0 end) c15,
	sum(case Datename(hour,o.PubDate) when 16 then 1 else 0 end) c16,
	sum(case Datename(hour,o.PubDate) when 17 then 1 else 0 end) c17,
	sum(case Datename(hour,o.PubDate) when 18 then 1 else 0 end) c18,
	sum(case Datename(hour,o.PubDate) when 19 then 1 else 0 end) c19,
	sum(case Datename(hour,o.PubDate) when 20 then 1 else 0 end) c20,
	sum(case Datename(hour,o.PubDate) when 21 then 1 else 0 end) c21,
	sum(case Datename(hour,o.PubDate) when 22 then 1 else 0 end) c22,
	sum(case Datename(hour,o.PubDate) when 23 then 1 else 0 end) c23
from dbo.[order] o(nolock)
where o.Status=1
	and o.PubDate between @startdate and @enddate
group by convert(char(10),o.PubDate,120)
order by convert(char(10),o.PubDate,120) desc,count(1) desc";
            #endregion

            var dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("startdate", DbType.Date).Value = queryInfo.StartDate.Value;
            dbParameters.Add("enddate", DbType.Date).Value = queryInfo.EndDate.Value;

            return DbHelper.QueryWithRowMapper(SuperMan_Read, SQL_TEXT, dbParameters,
                new TaskStatisticsPerHourInfoRowMapper());
        }

        public IList<TaskStatisticsPerHourInfo> QueryCityTaskCountPerHour(ParamTaskPerHour queryInfo)
        {
            #region sql text

            const string sqlText = @"
select convert(char(10),o.PubDate,120) 'date',o.ReceviceCity,
	sum(case Datename(hour,o.PubDate) when 0 then 1 else 0 end) c0,
	sum(case Datename(hour,o.PubDate) when 1 then 1 else 0 end) c1,
	sum(case Datename(hour,o.PubDate) when 2 then 1 else 0 end) c2,
	sum(case Datename(hour,o.PubDate) when 3 then 1 else 0 end) c3,
	sum(case Datename(hour,o.PubDate) when 4 then 1 else 0 end) c4,
	sum(case Datename(hour,o.PubDate) when 5 then 1 else 0 end) c5,
	sum(case Datename(hour,o.PubDate) when 6 then 1 else 0 end) c6,
	sum(case Datename(hour,o.PubDate) when 7 then 1 else 0 end) c7,
	sum(case Datename(hour,o.PubDate) when 8 then 1 else 0 end) c8,
	sum(case Datename(hour,o.PubDate) when 9 then 1 else 0 end) c9,
	sum(case Datename(hour,o.PubDate) when 10 then 1 else 0 end) c10,
	sum(case Datename(hour,o.PubDate) when 11 then 1 else 0 end) c11,
	sum(case Datename(hour,o.PubDate) when 12 then 1 else 0 end) c12,
	sum(case Datename(hour,o.PubDate) when 13 then 1 else 0 end) c13,
	sum(case Datename(hour,o.PubDate) when 14 then 1 else 0 end) c14,
	sum(case Datename(hour,o.PubDate) when 15 then 1 else 0 end) c15,
	sum(case Datename(hour,o.PubDate) when 16 then 1 else 0 end) c16,
	sum(case Datename(hour,o.PubDate) when 17 then 1 else 0 end) c17,
	sum(case Datename(hour,o.PubDate) when 18 then 1 else 0 end) c18,
	sum(case Datename(hour,o.PubDate) when 19 then 1 else 0 end) c19,
	sum(case Datename(hour,o.PubDate) when 20 then 1 else 0 end) c20,
	sum(case Datename(hour,o.PubDate) when 21 then 1 else 0 end) c21,
	sum(case Datename(hour,o.PubDate) when 22 then 1 else 0 end) c22,
	sum(case Datename(hour,o.PubDate) when 23 then 1 else 0 end) c23
from dbo.[order] o(nolock)
where o.Status=1
	and o.PubDate between @startdate and @enddate
group by convert(char(10),o.PubDate,120),o.ReceviceCity
order by convert(char(10),o.PubDate,120) desc,count(1) desc";
            #endregion

            var dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("startdate", DbType.Date).Value = queryInfo.StartDate.Value;
            dbParameters.Add("enddate", DbType.Date).Value = queryInfo.EndDate.Value;

            return DbHelper.QueryWithRowMapper(SuperMan_Read, sqlText, dbParameters,
                new TaskStatisticsPerHourInfoRowMapper());
        }

        private class TaskStatisticsPerHourInfoRowMapper : IDataTableRowMapper<TaskStatisticsPerHourInfo>
        {
            public TaskStatisticsPerHourInfo MapRow(DataRow dataReader)
            {
                var result = new TaskStatisticsPerHourInfo();

                result.Date = dataReader["date"].ToString();
                if (dataReader.HasColumn("ReceviceCity"))
                {
                    result.City = dataReader["ReceviceCity"].ToString();
                }
                result.TaskCounts = new List<int>();
                for (var index = 0; index < 24; index++)
                {
                    result.TaskCounts.Add(ParseHelper.ToInt(dataReader["c" + index], 0));
                }
                return result;
            }
        }
        #endregion

        #region 抢单、完成时长统计
        /// <summary>
        /// 抢单、完成时长统计
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public IList<JieDanTimeInfo> QueryJieDanTime(ParamJieDanTimeInfo queryInfo)
        {
            #region sql Text

            const string sqlText = @"
declare @clienterCount int
select @clienterCount =count(1) from dbo.clienter c(nolock) where c.Status=1
select convert(char(10),o.PubDate,120) 'Date',
	count(1) 'TaskCount',
	@clienterCount 'ClienterCount', 
	count(1)*1.0/@clienterCount 'AvgClienterCount',
	count(distinct o.clienterId) 'ActiveClienterCount',
	count(1)*1.0/count(distinct o.clienterId) 'AvgActiveClienterCount',
	sum(datediff(second,o.PubDate,osl.InsertTime)) 'PubReciveTotalSeconds',
	sum(datediff(second,o.PubDate,osl.InsertTime))*1.0/count(1)/60 'AvgPubReceiveMinutes',
	sum(datediff(second,o.PubDate,o.ActualDoneDate)) 'PubCompleteTotalSeconds',
	sum(datediff(second,o.PubDate,o.ActualDoneDate))*1.0/count(1)/60 'AvgPubCompleteMinutes'
from dbo.[order] o(nolock)
	join dbo.OrderSubsidiesLog osl(nolock) on o.Id=osl.OrderId and osl.OrderStatus=2
where o.Status=1
	and o.PubDate between @startdate and @enddate
	and o.Id not in (266957)
group by convert(char(10),o.PubDate,120)
order by convert(char(10),o.PubDate,120)";

            #endregion

            var dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("startdate", DbType.Date).Value = queryInfo.StartDate.Value;
            dbParameters.Add("enddate", DbType.Date).Value = queryInfo.EndDate.Value;

            return DbHelper.QueryWithRowMapper(SuperMan_Read, sqlText, dbParameters,
                new JieDanTimeInfoRowMapper());
        }

        private class JieDanTimeInfoRowMapper : IDataTableRowMapper<JieDanTimeInfo>
        {
            public JieDanTimeInfo MapRow(DataRow dataReader)
            {
                var result = new JieDanTimeInfo();

                result.Date = dataReader["Date"].ToString();
                result.TaskCount = ParseHelper.ToInt(dataReader["TaskCount"], 0);
                result.ActiveClienterCount = ParseHelper.ToInt(dataReader["ActiveClienterCount"], 0);
                result.AvgActiveClienterCount = Math.Round(ParseHelper.ToDecimal(dataReader["AvgActiveClienterCount"], 0),2);
                result.AvgClienterCount = Math.Round(ParseHelper.ToDecimal(dataReader["AvgClienterCount"], 0), 2);
                result.AvgPubCompleteMinutes = Math.Round(ParseHelper.ToDecimal(dataReader["AvgPubCompleteMinutes"], 0),2);
                result.AvgPubReceiveMinutes = Math.Round(ParseHelper.ToDecimal(dataReader["AvgPubReceiveMinutes"], 0), 2);
                result.ClienterCount = ParseHelper.ToInt(dataReader["ClienterCount"], 0);
                result.PubCompleteTotalSeconds = ParseHelper.ToLong(dataReader["PubCompleteTotalSeconds"], 0);
                result.PubReciveTotalSeconds = ParseHelper.ToLong(dataReader["PubReciveTotalSeconds"], 0);
                return result;
            }
        }
        #endregion
    }
}