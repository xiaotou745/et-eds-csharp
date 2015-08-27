using ETS.Const;
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
using Ets.Model.DomainModel.Statistics;
using ETS.Data.Generic;
using ETS.Extension;
using ETS.Data.PageData;

namespace Ets.Dao.Statistics
{
    public class StatisticsDao : DaoBase
    {

        /// <summary>
        /// 获取某天的数据统计
        /// 窦海超
        /// 2015年5月26日 14:00:19
        /// </summary>
        /// <returns></returns>
        public IList<HomeCountTitleModel> GetDayStatistics(int Day)
        {
            string sql = string.Format(@"
declare @starttime varchar(10),@endtime varchar(10),@BusinessCount int,
@RzqsCount int , @DdrzqsCount int,@businessPrice decimal(18,2),
@incomeTotal decimal(18,2),@rechargeTotal decimal(18,2),
@withdrawBusinessPrice decimal(18,2)
set @starttime= convert(char(10),getdate()-{0},120)
set @endtime = convert(char(10),getdate(),120)
print @starttime +','+ @endtime

set @BusinessCount = (SELECT COUNT(Id) AS BusinessCount FROM dbo.business(NOLOCK) WHERE [status]=1)
--print @starttime +','+@endtime
set @RzqsCount = (SELECT count(1) RzqsCount from dbo.clienter (nolock) where Status =1 )--认证骑士数量
set @DdrzqsCount = (SELECT count(1) RzqsCount from dbo.clienter (nolock) where Status =0 )--等待认证骑士
set @businessPrice = (SELECT sum(BalancePrice) from dbo.business b where Status = 1 ) --商户余额总计（应付）
--set @incomeTotal = (select sum(TotalPrice) from OrderChild(nolock) where ThirdPayStatus=1)--在线支付(扫码/代付)总计
--set @rechargeTotal = (select sum(Amount) from BusinessBalanceRecord(nolock) where RecordType = 4 ) --商户充值总计
--set @withdrawBusinessPrice=(select sum(BalancePrice) from BusinessWithdrawForm(nolock) where  Status =3 )--商户已提款金额（实付）

;with t1 as (
	SELECT CONVERT(CHAR(10),o.PubDate,120) AS PubDate, --发布时间
		  isnull(sum(case when DealCount=0 then 1 else 0 end ),0) 'ZeroSubsidyOrderCount',
		  isnull(sum(case when DealCount=1 then 1 else 0 end ),0) 'OneSubsidyOrderCount',
		  isnull(sum(case when DealCount=2 then 1 else 0 end ),0) 'TwoSubsidyOrderCount',
		  isnull(sum(case when DealCount=3 then 1 else 0 end ),0) 'ThreeSubsidyOrderCount'
	FROM [order](NOLOCK) AS o
	WHERE   o.[Status]<>3 and o.PubDate between @starttime and @endtime
	group by CONVERT(CHAR(10),o.PubDate,120)
),
t2 as (
	select 
	convert(char(10),o.PubDate,120) PubDate,
	count(distinct clienterId) 'ActiveClienter',--活跃骑士
	count(distinct businessId) 'ActiveBusiness'--活跃商家
	from dbo.[order](nolock) as o 
	where status<>3 and o.PubDate between @starttime and @endtime
	group by convert(char(10),o.PubDate,120)
),
t3 as (
	 select convert(char(10),CreateTime ,120) PubDate,
	 sum(TotalPrice)  incomeTotal
	 from OrderChild(nolock) where ThirdPayStatus=1 and PayTime between @starttime and @endtime
	 group by convert(char(10),CreateTime ,120)--在线支付(扫码/代付)总计
),
t4 as (
	 select
                convert(char(10),PayTime,120) PubDate,
                sum(case when PayType=1 then payAmount else 0 end) ZhiFuBaoRecharge,
                sum(case when PayType=2 then payAmount else 0 end) WeiXinRecharge,
                sum(case when PayType=3 then payAmount else 0 end) SystemRecharge,
                sum(case when PayType=4 then payAmount else 0 end) SystemPresented,
                (sum(case when PayType=1 then payAmount else 0 end) +
                sum(case when PayType=2 then payAmount else 0 end) +
                sum(case when PayType=3 then payAmount else 0 end) +
                sum(case when PayType=4 then payAmount else 0 end) ) rechargeTotal
                from BusinessRecharge(nolock) where PayTime between @startime  and @endtime 
	 --商户充值总计
),
t5 as (
	 (select  convert(char(10),PayTime,120) PubDate,
	  sum(BalancePrice) withdrawBusinessPrice from BusinessWithdrawForm(nolock) where  Status =3 and PayTime  between @starttime and @endtime
	  group by convert(char(10),PayTime,120) 
	  )
)
select 
	CONVERT(CHAR(10),o.PubDate,120) AS PubDate, --发布时间
	@RzqsCount RzqsCount,--认证骑士数量
	@DdrzqsCount DdrzqsCount,--等待认证骑士
	@BusinessCount BusinessCount,--商家总数
	SUM(ISNULL(o.Amount,0)) AS OrderPrice, --订单金额
	ISNULL(COUNT(o.Id),0) AS MisstionCount,--任务量
	SUM(ISNULL(OrderCount,0)) AS OrderCount,--订单量
	SUM(isnull(o.SettleMoney,0)) AS YsPrice,  -- 应收金额
	SUM(ISNULL(OrderCommission,0)) AS YfPrice,  --应付金额
	min(t1.ZeroSubsidyOrderCount) ZeroSubsidyOrderCount, --零次抢
	min(t1.OneSubsidyOrderCount) OneSubsidyOrderCount, --一次抢
	min(t1.TwoSubsidyOrderCount) TwoSubsidyOrderCount,--二次抢
	min(t1.ThreeSubsidyOrderCount) ThreeSubsidyOrderCount,--三次抢
	min(t2.ActiveClienter) ActiveClienter, --活跃骑士
	min(t2.ActiveBusiness) ActiveBusiness, --活跃商家
	isnull(min(t3.incomeTotal),0) incomeTotal,--在线支付(扫码/代付)总计
 	isnull( min(t4.rechargeTotal),0) rechargeTotal, --商户充值总计
    t4.ZhiFuBaoRecharge,
    t4.WeiXinRecharge,
    t4.SystemRecharge,
    t4.SystemPresented,
	@businessPrice businessBalance, --商户余额总计（应付）
 	isnull( min(t5.withdrawBusinessPrice),0) withdrawBusinessPrice --商户已提款金额（实付）
 from dbo.[order] o (nolock)
 left join t1 on convert(char(10),o.PubDate,120) = convert(char(10),t1.PubDate,120) 
 left join t2 on convert(char(10),o.PubDate,120) = convert(char(10),t2.PubDate,120) 
 left join t3 on convert(char(10),o.PubDate,120) = convert(char(10),t3.PubDate,120) 
 left join t4 on convert(char(10),o.PubDate,120)  = convert(char(10),t4.PubDate,120) 
 left join t5 on convert(char(10),o.PubDate,120)  = convert(char(10),t5.PubDate,120) 
  where o.Status <> 3 and o.PubDate between @starttime and @endtime
group by CONVERT(CHAR(10),o.PubDate,120)

", Day);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<HomeCountTitleModel>(dt);
        }

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

        public DateTime MaxDate()
        {
            string sql = @"SELECT max(InsertTime) FROM dbo.Statistic s ";
            return Convert.ToDateTime(DbHelper.ExecuteScalar(SuperMan_Read, sql));
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
            string sql = @"
INSERT INTO [dbo].[Statistic]
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
,[ActiveClienter]
,[incomeTotal]
,[rechargeTotal]
,[businessBalance]
,[withdrawBusinessPrice]
,SystemRecharge
,SystemPresented
,ZhiFuBaoRecharge
,WeiXinRecharge
)
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
,@ActiveClienter
,@incomeTotal
,@rechargeTotal
,@businessBalance
,@withdrawBusinessPrice
,@SystemRecharge
,@SystemPresented
,@ZhiFuBaoRecharge
,@WeiXinRecharge
)
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
            //parm.AddWithValue("@incomeTotal", model.incomeTotal);
            //parm.AddWithValue("@rechargeTotal", model.rechargeTotal);
            //parm.AddWithValue("@businessBalance", model.businessBalance);
            //parm.AddWithValue("@withdrawBusinessPrice", model.withdrawBusinessPrice);

            parm.Add("incomeTotal", DbType.Decimal, 18).Value = model.incomeTotal;
            parm.Add("rechargeTotal", DbType.Decimal, 18).Value = model.rechargeTotal;
            parm.Add("businessBalance", DbType.Decimal, 18).Value = model.businessBalance;
            parm.Add("withdrawBusinessPrice", DbType.Decimal, 18).Value = model.withdrawBusinessPrice;

            parm.Add("SystemRecharge", DbType.Decimal, 18).Value = model.SystemRecharge;
            parm.Add("SystemPresented", DbType.Decimal, 18).Value = model.SystemRecharge;
            parm.Add("ZhiFuBaoRecharge", DbType.Decimal, 18).Value = model.ZhiFuBaoRecharge;
            parm.Add("WeiXinRecharge", DbType.Decimal, 18).Value = model.WeiXinRecharge;

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
                        and Status<>3
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
declare 
 @startime varchar(10),
 @endtime varchar(10)
 set @startime = convert(char(10),getdate() ,120) 
 set @EndTime =  convert(char(10),getdate()+1 ,120) 
 
 ;with  t as ( select   convert(char(10), getdate(), 120) PubDate,
                        isnull(sum(case when [Status] = 1 then 1
                                        else 0
                                   end), 0) as RzqsCount, --认证骑士数量
                        isnull(sum(case when [Status] = 0 then 1
                                        else 0
                                   end), 0) as DdrzqsCount --等待认证骑士
               from     dbo.clienter(nolock)
             ),
        t2
          as ( select   convert(char(10), getdate(), 120) PubDate,
                        count(Id) as BusinessCount
               from     dbo.business(nolock)
               where    [status] = 1
             ),
        t3
          as ( select   convert(char(10), getdate(), 120) PubDate,
                        count(distinct clienterId) as ActiveClienter,
                        count(distinct businessId) as ActiveBusiness
               from     dbo.[order] (nolock) as o
               --where    convert(char(10), o.PubDate, 120) = convert(char(10), getdate(), 120)
               where o.PubDate between @startime  and @endtime
                        and status <> 3
             ),
        t4
          as ( select   convert(char(10), getdate(), 120) PubDate,
                        sum(case when Status = 2 then 1
                                 else 0
                            end) UnfinishedMissionCount,--未完成任务量
                        sum(case when Status = 0 then 1
                                 else 0
                            end) UnGrabMissionCount--未被抢任务量
               from     dbo.[order] (nolock) as o
               --where    convert(char(10), PubDate, 120) = convert(char(10), getdate(), 120)
                 where o.PubDate between @startime  and @endtime
                        and Status <> 3
               group by convert(char(10), PubDate, 120)
             ),
        t5
          as ( select   PubDate,
                        isnull(sum(case when a.DealCount = 0 then OrderCount
                                   end), 0) ZeroSubsidyOrderCount,
                        isnull(sum(case when a.DealCount = 1 then OrderCount
                                   end), 0) OneSubsidyOrderCount,
                        isnull(sum(case when a.DealCount = 2 then OrderCount
                                   end), 0) TwoSubsidyOrderCount,
                        isnull(sum(case when a.DealCount = 3 then OrderCount
                                   end), 0) ThreeSubsidyOrderCount
               from     ( select    convert(char(10), PubDate, 120) as PubDate, --发布时间
                                    count(1) as OrderCount,--订单量
                                    DealCount
                          from      dbo.[order] (nolock) as o
                          where     o.[Status] <> 3
                                    --and convert(char(10), PubDate, 120) = convert(char(10), getdate(), 120)
                                     and o.PubDate between @startime  and @endtime
                          group by  convert(char(10), PubDate, 120), DealCount
                        ) a
               group by PubDate
             ),
        t6
          as ( select   convert(char(10), PubDate, 120) as PubDate,
                        sum(isnull(Amount, 0)) as OrderPrice, --订单金额
                        isnull(count(o.Id), 0) as MisstionCount,--总任务量
                        sum(isnull(OrderCount, 0)) as OrderCount,--总订单量
                        sum(isnull(o.SettleMoney, 0)) as YsPrice,  -- 应收金额
                        sum(isnull(OrderCommission, 0)) as YfPrice  --应付金额  --应付金额
               from     dbo.[order] (nolock) as o
                        left join dbo.business (nolock) as b on o.businessId = b.Id
               where    o.[Status] <> 3
                        --and convert(char(10), PubDate, 120) = convert(char(10), getdate(), 120)
                          and PubDate between @startime  and @endtime
               group by convert(char(10), PubDate, 120)
             ),
       t7 
		as (
			 select   sum(TotalPrice) userTotal,convert(char(10),PayTime,120) PubDate
               from     dbo.OrderChild oc ( nolock )
               where  ThirdPayStatus=1 and  PayStyle = 1
                         --and convert(char(10), PayTime, 120) = convert(char(10), getdate(), 120)
                         and PayTime between @startime  and @endtime
               group by convert(char(10), PayTime, 120)
			),
		t8
		as (
			 select   sum(TotalPrice) clienterTotal,convert(char(10),PayTime,120) PubDate
               from     dbo.OrderChild oc ( nolock )  
               where  ThirdPayStatus=1 and  PayStyle = 2
                         --and convert(char(10), PayTime, 120) = convert(char(10), getdate(), 120)
                         and PayTime between @startime  and @endtime
               group by convert(char(10), PayTime, 120)
			),
		t9
		 as ( 
                select
                convert(char(10),PayTime,120) PubDate,
                sum(case when PayType=1 then payAmount else 0 end) ZhiFuBaoRecharge,
                sum(case when PayType=2 then payAmount else 0 end) WeiXinRecharge,
                sum(case when PayType=3 then payAmount else 0 end) SystemRecharge,
                sum(case when PayType=4 then payAmount else 0 end) SystemPresented,
                (sum(case when PayType=1 then payAmount else 0 end) +
                sum(case when PayType=2 then payAmount else 0 end) +
                sum(case when PayType=3 then payAmount else 0 end) +
                sum(case when PayType=4 then payAmount else 0 end) ) rechargeTotal
                from BusinessRecharge(nolock) where PayTime between @startime  and @endtime  
                group by convert(char(10),PayTime,120)
			 --商户充值总计
		 )
    select  t.PubDate, isnull(t.RzqsCount, 0) RzqsCount,
            isnull(t.DdrzqsCount, 0) DdrzqsCount,
            isnull(t2.BusinessCount, 0) BusinessCount,
            isnull(t3.ActiveClienter, 0) ActiveClienter,
            isnull(t3.ActiveBusiness, 0) ActiveBusiness,
            isnull(t4.UnfinishedMissionCount, 0) UnfinishedMissionCount,
            isnull(t4.UnGrabMissionCount, 0) UnGrabMissionCount,
            isnull(t5.ZeroSubsidyOrderCount, 0) ZeroSubsidyOrderCount,
            isnull(t5.OneSubsidyOrderCount, 0) OneSubsidyOrderCount,
            isnull(t5.TwoSubsidyOrderCount, 0) TwoSubsidyOrderCount,
            isnull(t5.ThreeSubsidyOrderCount, 0) ThreeSubsidyOrderCount,
            isnull(t6.OrderPrice, 0) OrderPrice,
            isnull(t6.MisstionCount, 0) MisstionCount,
            isnull(t6.OrderCount, 0) OrderCount, isnull(t6.YsPrice, 0) YsPrice,
            isnull(t6.YfPrice, 0) YfPrice,
            isnull(t7.userTotal,0) userTotal,
            isnull(t8.clienterTotal,0) clienterTotal,
            isnull(t9.rechargeTotal,0) rechargeTotal,
            t9.ZhiFuBaoRecharge,
            t9.WeiXinRecharge,
            t9.SystemRecharge,
            t9.SystemPresented,
            (isnull( userTotal,0)+isnull(clienterTotal,0))+isnull(t9.rechargeTotal,0) incomeTotal
    from    t
            left join t2 on t.PubDate = t2.PubDate
            left join t3 on t.PubDate = t3.PubDate
            left join t4 on t.PubDate = t4.PubDate
            left join t5 on t.PubDate = t5.PubDate
            left join t6 on t.PubDate = t6.PubDate
            left join t7 on t.PubDate = t7.PubDate
            left join t8 on t.Pubdate = t8.PubDate
            left join t9 on t.Pubdate = t9.PubDate
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

        #region 商家充值统计

        /// <summary>
        /// 查询分页后的商家成功充值的记录信息
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public PageInfo<BusinessBalanceInfo> QueryBusinessBalance(BussinessBalanceQuery queryInfo)
        {
//            string columnList = @"
//                                    bbr.Id
//                                    ,bbr.RelationNo
//                                    ,bbr.BusinessId
//                                    ,b.Name
//                                    ,b.PhoneNo
//                                    ,b.Address
//                                    ,bbr.OperateTime
//                                    ,bbr.Amount
//                                    ,bbr.Balance";
//            string tables = " dbo.BusinessBalanceRecord bbr(nolock) join dbo.business b(nolock) on bbr.BusinessId = b.Id";

            string columnList = @"  bbr.Id
                                    ,bbr.OrderNo RelationNo
                                    ,bbr.BusinessId
                                    ,b.Name
                                    ,b.PhoneNo
                                    ,b.[Address]
                                    ,bbr.PayTime OperateTime
                                    ,bbr.payAmount Amount ";
            string tables = " dbo.BusinessRecharge bbr(nolock) join dbo.business b(nolock) on bbr.BusinessId = b.Id ";

            var sbSqlWhere = GetQueryRechargeWhere(queryInfo);

            //0为充值时间倒序，1为充值金额降序，2为充值金额升序
            string orderByColumn = " bbr.PayTime desc  ";
            switch (queryInfo.OrderType)
            {
                case 1: orderByColumn = " bbr.payAmount desc  ";
                    break;
                case 2: orderByColumn = " bbr.payAmount asc  ";
                    break;
                default:
                    break;
            }
            return new PageHelper().GetPages<BusinessBalanceInfo>(SuperMan_Read, queryInfo.PageIndex, sbSqlWhere,
                orderByColumn, columnList, tables, ETS.Const.SystemConst.PageSize, true);
        }

        /// <summary>
        /// 根据查询条件组装过滤的sql语句
        /// </summary>
        /// <UpdateBy>wc</UpdateBy> 
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        private static string GetQueryRechargeWhere(BussinessBalanceQuery queryInfo)
        {
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(queryInfo.BusinessId))
            {
                sbSqlWhere.AppendFormat(" AND bbr.BusinessId='{0}' ", queryInfo.BusinessId);
            }
            if (!string.IsNullOrWhiteSpace(queryInfo.StartDate))
            {
                sbSqlWhere.AppendFormat(" AND bbr.PayTime>='{0}' ", queryInfo.StartDate);
            }
            if (!string.IsNullOrWhiteSpace(queryInfo.EndDate))
            {
                DateTime finalDt = ParseHelper.ToDatetime(queryInfo.EndDate);
                if (finalDt != DateTime.MaxValue)
                {
                    finalDt = finalDt.AddDays(1);
                }
                sbSqlWhere.AppendFormat(" AND bbr.PayTime<='{0}' ", finalDt.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrWhiteSpace(queryInfo.Name))
            {
                sbSqlWhere.AppendFormat(" AND b.Name='{0}' ", queryInfo.Name);
            }
            if (!string.IsNullOrWhiteSpace(queryInfo.PhoneNo))
            {
                sbSqlWhere.AppendFormat(" AND b.PhoneNo='{0}' ", queryInfo.PhoneNo);
            }
            if (!string.IsNullOrWhiteSpace(queryInfo.CityId))
            {
                sbSqlWhere.AppendFormat(" AND b.CityId={0} ", queryInfo.CityId);
            }
            if (queryInfo.RechargePrice > 0)
            {
                sbSqlWhere.AppendFormat(" AND bbr.payAmount>={0} ", queryInfo.RechargePrice);
            }
            if (queryInfo.RechargeType > 0)
            {
                if (queryInfo.RechargeType == 3)//充值赠送
                {
                    sbSqlWhere.Append(" and bbr.PayType=4 ");
                } 
                else if (queryInfo.RechargeType == 1)//系统充值
                {
                    sbSqlWhere.AppendFormat("and  bbr.PayType=3 ");
                }

                else if (queryInfo.RechargeType == 2)//客户端充值
                {
                    sbSqlWhere.AppendFormat(" and bbr.PayType in(1,2)");
                }
            } 
            //else
            //{
            //    sbSqlWhere.Append("  and bbr.PayType IN(1,2,3,4) ");
            //}
            return sbSqlWhere.ToString();
        }


        /// <summary>
        /// 根据查询条件组装过滤的sql语句
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        private static string GetQueryWhere(BussinessBalanceQuery queryInfo)
        {
            var sbSqlWhere = new StringBuilder(" bbr.status=1  ");
            if (!string.IsNullOrWhiteSpace(queryInfo.BusinessId))
            {
                sbSqlWhere.AppendFormat(" AND bbr.BusinessId='{0}' ", queryInfo.BusinessId);
            }
            if (!string.IsNullOrWhiteSpace(queryInfo.StartDate))
            {
                sbSqlWhere.AppendFormat(" AND bbr.operatetime>='{0}' ", queryInfo.StartDate);
            }
            if (!string.IsNullOrWhiteSpace(queryInfo.EndDate))
            {
                DateTime finalDt = ParseHelper.ToDatetime(queryInfo.EndDate);
                if (finalDt != DateTime.MaxValue)
                {
                    finalDt = finalDt.AddDays(1);
                }
                sbSqlWhere.AppendFormat(" AND bbr.operatetime<='{0}' ", finalDt.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrWhiteSpace(queryInfo.Name))
            {
                sbSqlWhere.AppendFormat(" AND Name='{0}' ", queryInfo.Name);
            }
            if (!string.IsNullOrWhiteSpace(queryInfo.PhoneNo))
            {
                sbSqlWhere.AppendFormat(" AND PhoneNo='{0}' ", queryInfo.PhoneNo);
            }
            if (!string.IsNullOrWhiteSpace(queryInfo.CityId))
            {
                sbSqlWhere.AppendFormat(" AND CityId={0} ", queryInfo.CityId);
            }
            if (queryInfo.RechargePrice > 0)
            {
                sbSqlWhere.AppendFormat(" AND Amount>={0} ", queryInfo.RechargePrice);
            }
            if (queryInfo.RechargeType > 0)
            {
                if (queryInfo.RechargeType == 3)//充值赠送
                {
                    sbSqlWhere.Append(" and RecordType=12  ");
                }

                else if (queryInfo.RechargeType == 1)//系统充值
                {
                    sbSqlWhere.AppendFormat("and  bbr.RecordType=9 AND Remark!='商家客户端充值'");
                }

                else if (queryInfo.RechargeType == 2)//客户端充值
                {
                    sbSqlWhere.AppendFormat(" and bbr.RecordType=9 AND Remark='商家客户端充值'");
                }
            }
            else {
                sbSqlWhere.Append("  and (bbr.RecordType=9 or RecordType=12) ");
            }
            return sbSqlWhere.ToString();
        }

        /// <summary>
        /// 查询给定条件下商家成功充值的总金额
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public decimal QueryBusinessTotalAmount(BussinessBalanceQuery queryInfo)
        {
            string sql = @"
                            SELECT  SUM(bbr.amount) AS totalAmount
                            FROM    BusinessBalanceRecord bbr ( NOLOCK )
                                    INNER JOIN dbo.business b ( NOLOCK ) ON bbr.BusinessId = b.Id where ";
            var sbSqlWhere = GetQueryWhere(queryInfo);
            object obj = DbHelper.ExecuteScalar(SuperMan_Read, sql + sbSqlWhere);
            return ParseHelper.ToDecimal(obj, 0);
        }

        /// <summary>
        /// 查询给定条件下充值成功的商户的个数
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <returns></returns>
        public long QueryBusinessNum(BussinessBalanceQuery queryInfo)
        {
            string sql = @"
                            SELECT count(distinct bbr.BusinessId) as num from 
                            BusinessBalanceRecord bbr(nolock) join business b(nolock) on bbr.BusinessId = b.Id where ";
            var sbSqlWhere = GetQueryWhere(queryInfo);

            object obj = DbHelper.ExecuteScalar(SuperMan_Read, sql + sbSqlWhere);
            return ParseHelper.ToLong(obj, 0);
        }
        #endregion


        #region 获取推荐人统计
        /// <summary>
        /// 分页获取推荐人统计列表
        /// </summary>
        /// <param name="recommendQuery"></param>
        /// <returns></returns>
        public PageInfo<RecommendDataModel> GetRecommendList(RecommendQuery recommendQuery)
        {
            string getcount = @"SELECT COUNT(ID) FROM
                            (SELECT b.id AS ID 
                            FROM dbo.business b(nolock)
                            join dbo.[order] o (nolock) on b.Id=o.businessId
                            where o.Status<>3
                            {0}
                            {1}
                            {2} 
                            group by b.Id ) AS t
";//查询数据总数
            string starPar = "";
            string endPar = "";
            string remPar = "";
            if (!string.IsNullOrWhiteSpace(recommendQuery.StartDate))
            {
                starPar = string.Format("AND o.PubDate>='{0}'", recommendQuery.StartDate);
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.EndDate))
            {
                endPar = string.Format(" AND o.PubDate<='{0}' ", Convert.ToDateTime(recommendQuery.EndDate).AddDays(1).ToString());
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.RecommendPhone))
            {
                remPar = string.Format(" AND B.RecommendPhone='{0}' ", recommendQuery.RecommendPhone.Trim());
            }
            var count = (int)DbHelper.ExecuteScalar(SuperMan_Read, CommandType.Text, string.Format(getcount, starPar, endPar, remPar));//总条数
            int pagecount = Convert.ToInt32(Math.Ceiling(count*1.0/SystemConst.PageSize));//总页数
            string columnList = @"    MIN(b.id) AS ID,
                                      MIN(b.Name) AS Name,
                                      MIN(b.PhoneNo) AS PhoneNo,
                                      MIN (b.address) AS [Address],
                                      MIN(b.InsertTime) AS InsertTime,
                                      MIN(b.RecommendPhone) AS RecommendPhone,
                                      sum(o.OrderCount) AS  OrderCount,
                                      MIN(b.Status) AS Status ";
            string tables = @"  dbo.business b(nolock) join dbo.[order] o (nolock) on b.Id=o.businessId ";
            StringBuilder sbSqlWhere = new StringBuilder(" o.Status<>3 ");
            if (!string.IsNullOrWhiteSpace(recommendQuery.StartDate))
            {
                sbSqlWhere.AppendFormat(" AND o.PubDate>='{0}' ", recommendQuery.StartDate);
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.EndDate))
            {
                sbSqlWhere.AppendFormat(" AND o.PubDate<='{0}' ", Convert.ToDateTime(recommendQuery.EndDate).AddDays(1).ToString());
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.RecommendPhone))
            {
                sbSqlWhere.AppendFormat(" AND B.RecommendPhone='{0}' ", recommendQuery.RecommendPhone.Trim());
            }
            sbSqlWhere.Append(" group by b.Id ");
            var temp= new PageHelper().GetPages<RecommendDataModel>(SuperMan_Read, recommendQuery.PageIndex, sbSqlWhere.ToString(),
                " b.Id ", columnList, tables, ETS.Const.SystemConst.PageSize, false);
            PageInfo<RecommendDataModel> result = new PageInfo<RecommendDataModel>(count, recommendQuery.PageIndex, temp.Records, pagecount, SystemConst.PageSize);
            
            
            return result;


            #region

//            string querysql=@"SELECT 
//                        MIN(b.id) AS ID,
//                        MIN(b.Name) AS Name,
//                        MIN(b.PhoneNo) AS PhoneNo,
//                        MIN (b.address) AS [Address],
//                        MIN(b.InsertTime) AS InsertTime,
//                        MIN(b.RecommendPhone) AS RecommendPhone,
//                        sum(o.OrderCount) AS  OrderCount,
//                        MIN(b.Status) AS Stauts,
//                        ROW_NUMBER() OVER (order by  b.Id   ) AS RowNum
//                        INTO #Temp
//                        FROM dbo.business b(nolock)
//                        join dbo.[order] o (nolock) on b.Id=o.businessId
//                        where 1=1 
//                        {0}
//                        {1}
//                        {2}
//                        group by b.Id  
//
//                        SELECT COUNT(ID) AS TotalCount FROM #Temp
//                        SELECT * FROM #Temp WHERE RowNum BETWEEN @Top AND @Last
//                        DROP TABLE #Temp";
//            string starPar = "";
//            string endPar = "";
//            string remPar = "";
//            if (!string.IsNullOrWhiteSpace(recommendQuery.StartDate))
//            {
//                starPar = string.Format("AND o.PubDate>='{0}'", recommendQuery.StartDate);
//            }
//            if (!string.IsNullOrWhiteSpace(recommendQuery.EndDate))
//            {
//                endPar=string.Format(" AND o.PubDate<='{0}' ", Convert.ToDateTime(recommendQuery.EndDate).AddDays(1).ToString());
//            }
//            if (!string.IsNullOrWhiteSpace(recommendQuery.RecommendPhone))
//            {
//                remPar=string.Format(" AND B.RecommendPhone='{0}' ", recommendQuery.RecommendPhone.Trim());
//            }
//            var dbParameters = DbHelper.CreateDbParameters();
//            var top = SystemConst.PageSize * (recommendQuery.PageIndex-1)+1;
//            var last = top + SystemConst.PageSize - 1;
//            dbParameters.Add("@Top", DbType.Int32).Value = top;
//            dbParameters.Add("@Last", DbType.Int32).Value = last;
//            DataSet ds = DbHelper.ExecuteDataset(SuperMan_Read, string.Format(querysql, starPar, endPar, remPar), dbParameters);
//            if (ds.HasData())
//            {   //无数据
//                return new PageInfo<RecommendDataModel>(0, 1, new List<RecommendDataModel>(), 0, 15);
//            }
//            var count = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]);
//            int pagecount = Convert.ToInt32(Math.Ceiling(count*1.0/SystemConst.PageSize));
//            var list = MapRows<RecommendDataModel>(ds.Tables[1]);
//            PageInfo<RecommendDataModel> result = new PageInfo<RecommendDataModel>(count, recommendQuery.PageIndex, list, pagecount, SystemConst.PageSize);
//            return result;

            #endregion
        }
        #endregion


        #region 20150810新版推荐人统计 茹化肖 2015年8月10日13:48:043
        /// <summary>
        /// 推荐统计 商家 
        /// 茹化肖
        /// 2015年8月10日13:50:30
        /// </summary>
        /// <param name="recommendQuery"></param>
        /// <returns></returns>
        public PageInfo<RecommendDataModel> GetRecommendListB(RecommendQuery recommendQuery)
        {
            string starPar = "";
            string endPar = "";
            string remPar = "";
            #region===拼参数
            if (!string.IsNullOrWhiteSpace(recommendQuery.StartDate))
            {
                starPar = string.Format("AND o.PubDate>='{0}'", recommendQuery.StartDate);
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.EndDate))
            {
                endPar = string.Format(" AND o.PubDate<='{0}' ", Convert.ToDateTime(recommendQuery.EndDate).AddDays(1).ToString());
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.RecommendPhone))
            {
                remPar = string.Format(" AND b.RecommendPhone='{0}' ", recommendQuery.RecommendPhone.Trim());
            }
            #endregion

            #region===查询数据总数
            string getcount = @"SELECT  COUNT(1)
FROM    ( SELECT    T.RecommendPhone ,
                    COUNT(t.id) AS BusCount ,
                    SUM(T.OrderCount) AS OrderCount ,
                    SUM(T.TaskCount) AS TaskCount
          FROM      ( SELECT    RecommendPhone ,
                                b.id ,
                                SUM(ISNULL(o.OrderCount, 0)) AS OrderCount ,
                                COUNT(o.Id) AS TaskCount
                      FROM      dbo.business AS b ( NOLOCK )
                                LEFT JOIN dbo.[order] AS o ( NOLOCK ) ON b.id = o.businessId
                      WHERE     ISNULL(b.RecommendPhone, '') != ''
                                {0}
                                {1}
                                {2}
GROUP BY                        b.RecommendPhone ,
                                b.Id
                    ) AS T
          GROUP BY  T.RecommendPhone
        ) AS T2
";
            var count = (int)DbHelper.ExecuteScalar(SuperMan_Read, CommandType.Text, string.Format(getcount, starPar, endPar, remPar));//总条数
            int pagecount = Convert.ToInt32(Math.Ceiling(count * 1.0 / SystemConst.PageSize));//总页数
            #endregion

            #region===分页查询数据
            string columnList = @"  T1.RecommendPhone,
                                    COUNT(T1.id) AS BusCount,
                                    SUM(T1.OrderCount) AS OrderCount ,
                                    SUM(T1.TaskCount) AS TaskCount  ";
            string tables = @"  (
SELECT b.RecommendPhone,b.id,SUM(ISNULL(o.OrderCount,0)) AS OrderCount,COUNT(o.Id) AS TaskCount
FROM dbo.business AS b (NOLOCK)
LEFT JOIN dbo.[order] AS o(NOLOCK) ON b.id=o.businessId
WHERE ISNULL(b.RecommendPhone,'')!=''
{0}
{1}
{2}
GROUP BY b.RecommendPhone,b.Id
) AS T1 ";
            string whereStr = " 1=1 GROUP BY  T1.RecommendPhone";
            tables = string.Format(tables, starPar, endPar, remPar);
            var temp = new PageHelper().GetPages<RecommendDataModel>(SuperMan_Read, recommendQuery.PageIndex, whereStr,
                " T1.RecommendPhone ", columnList, tables, ETS.Const.SystemConst.PageSize, false);
            #endregion
            var result = new PageInfo<RecommendDataModel>(count, recommendQuery.PageIndex, temp.Records, pagecount, SystemConst.PageSize);
            return result;
        }
        /// <summary>
        /// 推荐统计 骑士
        /// 茹化肖
        /// 2015年8月10日13:50:30
        /// </summary>
        /// <param name="recommendQuery"></param>
        /// <returns></returns>
        public PageInfo<RecommendDataModel> GetRecommendListC(RecommendQuery recommendQuery)
        {
            string starPar = "";
            string endPar = "";
            string remPar = "";
            #region===拼参数
            if (!string.IsNullOrWhiteSpace(recommendQuery.StartDate))
            {
                starPar = string.Format("AND o.PubDate>='{0}'", recommendQuery.StartDate);
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.EndDate))
            {
                endPar = string.Format(" AND o.PubDate<='{0}' ", Convert.ToDateTime(recommendQuery.EndDate).AddDays(1).ToString());
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.RecommendPhone))
            {
                remPar = string.Format(" AND c.RecommendPhone='{0}' ", recommendQuery.RecommendPhone.Trim());
            }
            #endregion

            #region===查询数据总数
            string getcount = @"SELECT  COUNT(1)
FROM    ( SELECT    T.PhoneNo ,
                    T.TrueName ,
                    COUNT(T.Id) AS ClienterCount ,
                    SUM(T.TaskCount) AS TaskCount ,
                    SUM(T.OrderCount) AS OrderCount
          FROM      ( SELECT    c2.PhoneNo ,
                                c2.TrueName ,
                                c.Id ,
                                COUNT(o.OrderNo) AS TaskCount ,
                                SUM(ISNULL(o.OrderCount, 0)) AS OrderCount
                      FROM      dbo.clienter AS c ( NOLOCK )
                                JOIN dbo.clienter AS c2 ( NOLOCK ) ON c.recommendPhone = c2.PhoneNo
                                LEFT JOIN dbo.[order] AS o ( NOLOCK ) ON o.clienterId = c.id
                      WHERE     ISNULL(c.recommendPhone, '') <> ''
								{0}
                                {1}
                                {2}
GROUP BY                        c2.PhoneNo ,
                                c2.TrueName ,
                                c.Id
                    ) AS T
          GROUP BY  T.PhoneNo ,
                    T.TrueName
        ) AS T1
";
            var count = (int)DbHelper.ExecuteScalar(SuperMan_Read, CommandType.Text, string.Format(getcount, starPar, endPar, remPar));//总条数
            int pagecount = Convert.ToInt32(Math.Ceiling(count * 1.0 / SystemConst.PageSize));//总页数
            #endregion

            #region===分页查询数据
            string columnList = @"
        T1.PhoneNo ,
        T1.TrueName ,
        COUNT(T1.Id) AS ClienterCount ,
        SUM(T1.TaskCount) AS TaskCount ,
        SUM(T1.OrderCount) AS OrderCount";
           
            string tables = @" ( SELECT    c2.PhoneNo ,
                    c2.TrueName ,
                    c.Id ,
                    COUNT(o.OrderNo) AS TaskCount ,
                    SUM(ISNULL(o.OrderCount, 0)) AS OrderCount
          FROM      dbo.clienter AS c ( NOLOCK )
                    JOIN dbo.clienter AS c2 ( NOLOCK ) ON c.recommendPhone = c2.PhoneNo
                    LEFT JOIN dbo.[order] AS o ( NOLOCK ) ON o.clienterId = c.id
          WHERE     ISNULL(c.recommendPhone, '') <> ''
                    {0}
                    {1}
                    {2}
GROUP BY            c2.PhoneNo ,
                    c2.TrueName ,
                    c.Id
        ) AS T1 ";
            tables = string.Format(tables, starPar, endPar, remPar);
            string whereStr = " 1=1 GROUP BY T1.PhoneNo ,T1.TrueName ";

            var temp = new PageHelper().GetPages<RecommendDataModel>(SuperMan_Read, recommendQuery.PageIndex, whereStr,
                "  T1.PhoneNo ", columnList, tables, ETS.Const.SystemConst.PageSize, false);
            #endregion
            var result = new PageInfo<RecommendDataModel>(count, recommendQuery.PageIndex, temp.Records, pagecount, SystemConst.PageSize);
            return result;
        }
        #endregion
        /// <summary>
        /// 推荐统计--商户详情 分页
        /// 2015年8月10日17:19:31
        /// 茹化肖
        /// </summary>
        /// <param name="recommendQuery"></param>
        /// <returns></returns>
        public PageInfo<RecommendDetailDataModel> GetRecommendDetailListB(RecommendQuery recommendQuery)
        {
            string starPar = "";
            string endPar = "";
            string remPar = "";
            string quyPar = "";
            #region===拼参数
            if (!string.IsNullOrWhiteSpace(recommendQuery.StartDate))
            {
                starPar = string.Format("AND o.PubDate>='{0}'", recommendQuery.StartDate);
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.EndDate))
            {
                endPar = string.Format(" AND o.PubDate<='{0}' ", Convert.ToDateTime(recommendQuery.EndDate).AddDays(1).ToString());
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.RecommendPhone))
            {
                remPar = string.Format(" AND b.RecommendPhone='{0}' ", recommendQuery.RecommendPhone.Trim());
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.QueryPhone))
            {
                quyPar = string.Format(" AND b.PhoneNo='{0}' ", recommendQuery.QueryPhone.Trim());
            }
            #endregion

            #region===查询数据总数
            string getcount = @"SELECT  COUNT(1)
FROM    ( SELECT    b.Name ,
                    b.PhoneNo ,
                    b.Address ,
                    b.InsertTime ,
                    COUNT(o.Id) AS TaskCount ,
                    SUM(o.OrderCount) AS OrderCount
          FROM      dbo.business AS b ( NOLOCK )
                    LEFT JOIN dbo.[order] AS o ( NOLOCK ) ON o.businessId = b.Id
          WHERE     1 = 1
                    {0}
                    {1}
                    {2}
                    {3}
GROUP BY            b.Name ,
                    b.PhoneNo ,
                    b.Address ,
                    b.InsertTime
        ) AS T
";
            var count = (int)DbHelper.ExecuteScalar(SuperMan_Read, CommandType.Text, string.Format(getcount, starPar, endPar, remPar,quyPar));//总条数
            int pagecount = Convert.ToInt32(Math.Ceiling(count * 1.0 / SystemConst.PageSize));//总页数
            #endregion

            #region===分页查询数据
            string columnList = @"
                    b.Name AS BusName ,
                    b.PhoneNo AS PhoneNo,
                    b.Address AS BusAddress,
                    b.InsertTime AS RegDateTime ,
                    COUNT(o.Id) AS TaskCount ,
                    SUM(o.OrderCount) AS OrderCount ";

            string tables = @" dbo.business AS b ( NOLOCK )
                    LEFT JOIN dbo.[order] AS o ( NOLOCK ) ON o.businessId = b.Id ";


            StringBuilder whereStr = new StringBuilder(" 1=1 ");
            whereStr.Append(starPar);
            whereStr.Append(endPar);
            whereStr.Append(remPar);
            whereStr.Append(quyPar);
            whereStr.Append(@" GROUP BY            b.Name ,
                    b.PhoneNo ,
                    b.Address ,
                    b.InsertTime ");
            var temp = new PageHelper().GetPages<RecommendDetailDataModel>(SuperMan_Read, recommendQuery.PageIndex, whereStr.ToString(),
                "  b.InsertTime ", columnList, tables, ETS.Const.SystemConst.PageSize, false);
            #endregion
            var result = new PageInfo<RecommendDetailDataModel>(count, recommendQuery.PageIndex, temp.Records, pagecount, SystemConst.PageSize);
            return result;
        }
        /// <summary>
        /// 推荐统计--骑士详情 分页
        /// 2015年8月10日17:19:46
        /// 茹化肖
        /// </summary>
        /// <param name="recommendQuery"></param>
        /// <returns></returns>
        public PageInfo<RecommendDetailDataModel> GetRecommendDetailListC(RecommendQuery recommendQuery)
        {
            string starPar = "";
            string endPar = "";
            string remPar = "";
            string quyPar = "";
            #region===拼参数
            if (!string.IsNullOrWhiteSpace(recommendQuery.StartDate))
            {
                starPar = string.Format("AND o.PubDate>='{0}'", recommendQuery.StartDate);
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.EndDate))
            {
                endPar = string.Format(" AND o.PubDate<='{0}' ", Convert.ToDateTime(recommendQuery.EndDate).AddDays(1).ToString());
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.RecommendPhone))
            {
                remPar = string.Format(" AND c.RecommendPhone='{0}' ", recommendQuery.RecommendPhone.Trim());
            }
            if (!string.IsNullOrWhiteSpace(recommendQuery.QueryPhone))
            {
                quyPar = string.Format(" AND c.PhoneNo='{0}' ", recommendQuery.QueryPhone.Trim());
            }
            #endregion

            #region===查询数据总数
            string getcount = @"   SELECT COUNT(1) FROM 
    (SELECT  ISNULL(MAX(c.TrueName), '') AS CliName ,
        MAX(c.PhoneNo) AS PhoneNo ,
        MAX(c.InsertTime) AS RegDateTime ,
        COUNT(o.Id) AS TaskCount ,
        SUM(ISNULL(o.OrderCount, 0)) AS OrderCount
FROM    dbo.clienter AS c ( NOLOCK )
        LEFT JOIN dbo.[order] AS o ( NOLOCK ) ON o.clienterId = c.Id
WHERE   1 = 1
{0}
{1}
{2}
{3}
GROUP BY  c.PhoneNo ) AS T
";
            var count = (int)DbHelper.ExecuteScalar(SuperMan_Read, CommandType.Text, string.Format(getcount, starPar, endPar, remPar, quyPar));//总条数
            int pagecount = Convert.ToInt32(Math.Ceiling(count * 1.0 / SystemConst.PageSize));//总页数
            #endregion

            #region===分页查询数据
            string columnList = @"
                   ISNULL(MAX(c.TrueName), '') AS CliName ,
                    MAX(c.PhoneNo) AS PhoneNo ,
                    MAX(c.InsertTime) AS RegDateTime ,
                    COUNT(o.Id) AS TaskCount ,
                    SUM(ISNULL(o.OrderCount, 0)) AS OrderCount ";

            string tables = @" dbo.clienter AS c ( NOLOCK )
        LEFT JOIN dbo.[order] AS o ( NOLOCK ) ON o.clienterId = c.Id ";


            StringBuilder whereStr = new StringBuilder(" 1=1 ");
            whereStr.Append(starPar);
            whereStr.Append(endPar);
            whereStr.Append(remPar);
            whereStr.Append(quyPar);
            whereStr.Append(@" GROUP BY c.PhoneNo ");
            var temp = new PageHelper().GetPages<RecommendDetailDataModel>(SuperMan_Read, recommendQuery.PageIndex, whereStr.ToString(),
                "  c.PhoneNo ", columnList, tables, ETS.Const.SystemConst.PageSize, false);
            #endregion
            var result = new PageInfo<RecommendDetailDataModel>(count, recommendQuery.PageIndex, temp.Records, pagecount, SystemConst.PageSize);
            return result;
        }
    }
}
