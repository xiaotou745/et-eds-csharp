use superman
go

declare @startdate varchar(10)
set @startdate='2015-04-10'

;with t as(
select temp.date,temp.businessCount,count(temp.clienterId) cCount
from 
(
	select convert(char(10), PubDate, 120) 'date', o.clienterId,count(distinct o.businessId) 'businessCount'
	from dbo.[order] o(nolock)
	where o.PubDate>=@startdate and o.Status =1
	group by convert(char(10), PubDate, 120), o.clienterId
) as temp
group by temp.date,temp.businessCount
)
,t2 as (
select convert(char(10), csl.InsertTime-1, 120) date,csl.BusinessCount 'businessCount',
	count(distinct csl.ClienterId) clientorCount,sum(csl.Amount) totalAmount
from dbo.CrossShopLog csl(nolock)
where csl.InsertTime-1 > @startdate
group by convert(char(10), csl.InsertTime-1, 120), csl.BusinessCount
)

select temp2.date,sum(temp2.amount) totalAmount,max(case temp2.businessCount when 1 then temp2.cCount else 0 end) c1,
	max(case temp2.businessCount when 1 then temp2.amount else 0 end) a1,
	max(case temp2.businessCount when 2 then temp2.cCount else 0 end) c2,
	max(case temp2.businessCount when 2 then temp2.amount else 0 end) a2,
	max(case temp2.businessCount when 3 then temp2.cCount else 0 end) c3,
	max(case temp2.businessCount when 3 then temp2.amount else 0 end) a3,
	max(case temp2.businessCount when 4 then temp2.cCount else 0 end) c4,
	max(case temp2.businessCount when 4 then temp2.amount else 0 end) a4,
	max(case temp2.businessCount when 5 then temp2.cCount else 0 end) c5,
	max(case temp2.businessCount when 5 then temp2.amount else 0 end) a5,
	max(case temp2.businessCount when 6 then temp2.cCount else 0 end) c6,
	max(case temp2.businessCount when 6 then temp2.amount else 0 end) a6,
	max(case temp2.businessCount when 7 then temp2.cCount else 0 end) c7,
	max(case temp2.businessCount when 7 then temp2.amount else 0 end) a7,
	max(case temp2.businessCount when 8 then temp2.cCount else 0 end) c8,
	max(case temp2.businessCount when 8 then temp2.amount else 0 end) a8,
	max(case temp2.businessCount when 9 then temp2.cCount else 0 end) c9,
	max(case temp2.businessCount when 9 then temp2.amount else 0 end) a9,
	sum(case when temp2.businessCount>9 then temp2.cCount else 0 end) c10,
	sum(case when temp2.businessCount>9 then temp2.amount else 0 end) a10
from 
(
select t.date,t.businessCount,t.cCount,isnull(t2.totalAmount,0) amount
from t t
	left join t2 t2 on t.date = t2.date and t.businessCount=t2.businessCount
) as temp2
group by temp2.date
order by temp2.date desc

