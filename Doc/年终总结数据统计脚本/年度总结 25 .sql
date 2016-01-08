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




