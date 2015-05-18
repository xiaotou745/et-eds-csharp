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

    }
}