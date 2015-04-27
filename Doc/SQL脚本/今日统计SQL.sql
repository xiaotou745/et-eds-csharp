declare @startdate varchar(10)
set @startdate='2015-04-08'

drop table #b,#c,#order,#butie
    
--商家数量
select @startdate 'date', count(1) 'count' into #b from dbo.business b(nolock) where b.Status=1

--骑士数量
select @startdate 'date', isnull(sum(case when [Status] = 1 then 1
                        else 0
                   end), 0) as RzqsCount, --认证骑士数量
        isnull(sum(case when [Status] = 0 then 1
                        else 0
                   end), 0) as DdrzqsCount --等待认证骑士
                   into #c
from dbo.clienter c(nolock)

--订单的一些统计
select convert(char(10), PubDate, 120) 'date',
	count(distinct o.businessId) 'businessCount',--活跃商家数量
	count(distinct o.clienterId) 'clientorCount',--活跃骑士数量
	count(1) 'taskCount',--任务数量
	isnull(sum(case o.Status when 1 then 1 else 0 end),0) 'completeCount',--已完成任务数量
	isnull(sum(case o.Status when 2 then 1 else 0 end),0) 'DealingCount',--未完成任务数量
	isnull(sum(case o.Status when 0 then 1 else 0 end),0) 'notJiedanCount', --未接单任务数量
	sum(isnull(o.OrderCount,0)) 'orderCount', --订单数量
	sum(isnull(o.Amount,0)) 'totalPrice', --订单金额
	sum(isnull(o.SettleMoney,0)) 'SettleMoney', --应收
	sum(isnull(o.OrderCommission,0)) 'OrderCommission' --应付
into #order 
from dbo.[order] o(nolock) 
where o.PubDate>=@startdate and o.Status<>3 
group by convert(char(10), PubDate, 120)

---动态补贴的一些统计
select convert(char(10), PubDate, 120) 'date',
	isnull(sum(case o.DealCount when 0 then 1 else 0 end),0) 'zeroTime',
	isnull(sum(case o.DealCount when 1 then 1 else 0 end),0) 'oneTime',
	isnull(sum(case o.DealCount when 2 then 1 else 0 end),0) 'twoTime',
	isnull(sum(case o.DealCount when 3 then 1 else 0 end),0) 'threeTime'
into #butie
from dbo.[order] o(nolock)
where o.PubDate>=@startdate and o.Status<>3 
group by convert(char(10), PubDate, 120)


select b.date '日期',
	b.count '商家数量',
	c.RzqsCount '已认证骑士数量',
	c.DdrzqsCount '待认证骑士数量',
	o.businessCount '活跃商家数量',
	o.clientorCount '活跃骑士数量',
	o.taskCount '总任务数量',
	o.notJiedanCount '未被抢任务量',
	o.DealingCount '未完成任务量',
	o.orderCount '总订单数量',
	convert(decimal(18,2),o.orderCount*1.0 / o.taskCount) '任务平均订单量',
	o.totalPrice '订单总金额',
	convert(decimal(18,2),o.totalPrice*1.0 / o.orderCount) '客单价',
	convert(decimal(18,2),o.totalPrice*1.0 /o.taskCount) '任务单价',
	b2.zeroTime '0次补贴被抢任务',
	b2.oneTime '1次补贴被抢任务',
	b2.twoTime '2次补贴被抢任务',
	b2.threeTime '3次补贴被抢任务',
	convert(decimal(18,2),o.taskCount*1.0/o.businessCount) '商家平均发布任务',
	convert(decimal(18,2),o.orderCount*1.0/o.businessCount) '商家平均发布订单',
	convert(decimal(18,2),o.taskCount*1.0/o.clientorCount) '骑士平均完成任务',
	convert(decimal(18,2),o.orderCount*1.0/o.clientorCount) '骑士平均完成订单',
	o.SettleMoney '应收',
	o.OrderCommission '应付',
	o.SettleMoney-o.OrderCommission '盈亏'
from #b b
	join #c c on b.date = c.date
	join #order o on b.date = o.date
	join #butie b2(nolock) on b.date = b2.date