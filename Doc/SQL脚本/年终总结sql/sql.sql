--意见与反馈 总数量
SELECT COUNT(1) FROM dbo.Feedback (NOLOCK) WHERE CreateTime<'2016-01-01'

--意见与反馈 每月反馈数量
SELECT convert(varchar(7),CreateTime,120) as Date,    COUNT(1) as [Count]
FROM   Feedback (NOLOCK)
WHERE CreateTime<'2016-01-01'
GROUP BY convert(varchar(7),CreateTime,120)

--意见与反馈 不同用户类型的反馈数量
SELECT CASE  UserType WHEN 1 THEN '门店' ELSE  '骑士' END 用户类型 ,    COUNT(1) as 数量
FROM   Feedback (NOLOCK)
WHERE CreateTime<'2016-01-01'
GROUP BY UserType

--意见与反馈 不同反馈类型的反馈数量
SELECT CASE  FeedbackType WHEN 1 THEN '功能意见' WHEN 2 THEN '页面意见'
WHEN 3 THEN '您的新需求' WHEN 4 THEN '操作意见'   ELSE  '其它' END 反馈类型 ,    COUNT(1) as 数量
FROM   Feedback (NOLOCK)
WHERE CreateTime<'2016-01-01'
GROUP BY FeedbackType

--标签 无数据 标签相关
--集团商户  集团管理，集团绑定商户，充值，订单发单扣款，写余额流水

--物流公司订单数据
;with t as(
SELECT d.id, d.DeliveryCompanyName, SUM(CASE  WHEN c.ID IS NULL THEN 0 ELSE 1 END)  AS 数量  FROM dbo.DeliveryCompany d  (NOLOCK)
LEFT JOIN dbo.clienter c (NOLOCK) ON c.DeliveryCompanyId=d.Id  and c.DeliveryCompanyId>0
GROUP BY d.id, d.DeliveryCompanyName
)
SELECT t.id, t.DeliveryCompanyName,ISNULL(t.数量,0) AS  骑士数量,ISNULL(V1.应收商家,0) AS 应收商家 ,
ISNULL(V1.应付物流公司,0) AS 应付物流公司 ,ISNULL(V1.骑士结算总额,0) 骑士结算总额,ISNULL(V1.任务量,0 ) 任务量,ISNULL(V1.订单量,0) 订单量,ISNULL(V1.订单总金额,0) 订单总金额 FROM t
LEFT JOIN 
(
	SELECT d.id AS DeliveryCompanyID,SUM(SettleMoney) AS 应收商家 ,	
	SUM(case when d.SettleType=1 then o.Amount*d.DeliveryCompanyRatio/100 
	when d.SettleType=2 then d.DeliveryCompanySettleMoney*o.OrderCount END) AS 应付物流公司, 
	SUM(OrderCommission)AS  骑士结算总额,
	COUNT(1)AS 任务量,SUM(OrderCount) AS 订单量,
	SUM(Amount) AS 订单总金额 FROM [dbo].[order] o (NOLOCK)
    LEFT JOIN   dbo.DeliveryCompany d  (NOLOCK) ON o.DeliveryCompanyID=d.Id
	GROUP BY  d.id
)  V1 ON t.id=V1.DeliveryCompanyId



4	如风达	61	96951.00	0.00	        30666.00	    2613	23273	456555.70
4	如风达	61	76387.50	130521.600000	4.00	2084	18128	373144.70

SELECT  TOP 100
* FROM  [dbo].[order] o (NOLOCK)
 WHERE DeliveryCompanyId=4




SELECT ClienterFixMoney,* FROM DeliveryCompany WHERE id=4
case when dc.SettleType=1 then @orderamount*dc.DeliveryCompanyRatio/100 when dc.SettleType=2 then dc.DeliveryCompanySettleMoney*@ordercount