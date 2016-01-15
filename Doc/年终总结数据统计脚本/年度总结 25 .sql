use  superman
go


--25 
--版本控制记录总数
select SUM(1) from dbo.AppVersion 
--每月发版数量 
SELECT  SUM(1) as 数量,convert(varchar(7),CreateDate,120) 月份  FROM dbo.AppVersion(nolock)  group by   convert(varchar(7),CreateDate,120)






--26-29  充值统计

--总览
select  CONVERT(VARCHAR(7), PayTime, 120) 月份 ,
        SUM(1) as 次数 ,
        SUM(payAmount) 充值总金额 ,
        CASE PayType
          when ( 1 ) then '支付宝'
          when ( 2 ) then '微信'
          when ( 3 ) then '后台'
          when ( 4 ) then '赠送'
        end as 充值方式
from    dbo.BusinessRecharge(nolock)
where PayTime<'2016-01-01 00:00:00'
group by CONVERT(VARCHAR(7), PayTime, 120) ,
        PayType
order by PayType ,
        CONVERT(VARCHAR(7), PayTime, 120)

--城市查询
select  CONVERT(VARCHAR(7), PayTime, 120) 月份 ,
        SUM(1) as 次数 ,
        SUM(payAmount) 充值总金额 ,
        CASE PayType
          when ( 1 ) then '支付宝'
          when ( 2 ) then '微信'
          when ( 3 ) then '后台'
          when ( 4 ) then '赠送'
        end as 充值方式 ,
        business.City as 城市
from    dbo.BusinessRecharge(nolock)
        join dbo.business on dbo.BusinessRecharge.BusinessId = business.id
where   PayTime < '2016-01-01 00:00:00'
group by CONVERT(VARCHAR(7), PayTime, 120) ,
        PayType ,
        dbo.business.City
order by PayType ,
        CONVERT(VARCHAR(7), PayTime, 120) ,
        dbo.business.City


-- 33 订单审核统计
--订单总数  
select  COUNT(1)
from    dbo.[order]  (nolock)
where   PubDate < '2016-01-01 00:00:00'

--审核总数
select  COUNT(1)
from    OrderOther(nolock)
        join [order](nolock) on dbo.OrderOther.OrderId = dbo.[order].Id
where   AuditStatus != 0
        and AuditOptName is not null
        and PubDate < '2016-01-01 00:00:00'
        
        
--月订单总数，审核总数
select  CONVERT(VARCHAR(7), PubDate, 120) 月份 ,
        COUNT(dbo.[order].Id) as 本月订单总数 ,
        SUM(CASE when AuditStatus != 0
                      and AuditOptName is not null then 1
                 else 0
            end) 本月人工审核订单数
from    [order] (nolock)
        left  join OrderOther(nolock) on dbo.OrderOther.OrderId = dbo.[order].Id
where   PubDate < '2016-01-01 00:00:00'
group by CONVERT(VARCHAR(7), PubDate, 120)
order by CONVERT(VARCHAR(7), PubDate, 120)