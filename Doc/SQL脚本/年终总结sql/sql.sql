--����뷴�� ������
SELECT COUNT(1) FROM dbo.Feedback (NOLOCK) WHERE CreateTime<'2016-01-01'

--����뷴�� ÿ�·�������
SELECT convert(varchar(7),CreateTime,120) as Date,    COUNT(1) as [Count]
FROM   Feedback (NOLOCK)
WHERE CreateTime<'2016-01-01'
GROUP BY convert(varchar(7),CreateTime,120)

--����뷴�� ��ͬ�û����͵ķ�������
SELECT CASE  UserType WHEN 1 THEN '�ŵ�' ELSE  '��ʿ' END �û����� ,    COUNT(1) as ����
FROM   Feedback (NOLOCK)
WHERE CreateTime<'2016-01-01'
GROUP BY UserType

--����뷴�� ��ͬ�������͵ķ�������
SELECT CASE  FeedbackType WHEN 1 THEN '�������' WHEN 2 THEN 'ҳ�����'
WHEN 3 THEN '����������' WHEN 4 THEN '�������'   ELSE  '����' END �������� ,    COUNT(1) as ����
FROM   Feedback (NOLOCK)
WHERE CreateTime<'2016-01-01'
GROUP BY FeedbackType

--��ǩ ������ ��ǩ���
--�����̻�  ���Ź������Ű��̻�����ֵ�����������ۿд�����ˮ

--������˾��������
;with t as(
SELECT d.id, d.DeliveryCompanyName, SUM(CASE  WHEN c.ID IS NULL THEN 0 ELSE 1 END)  AS ����  FROM dbo.DeliveryCompany d  (NOLOCK)
LEFT JOIN dbo.clienter c (NOLOCK) ON c.DeliveryCompanyId=d.Id  and c.DeliveryCompanyId>0
GROUP BY d.id, d.DeliveryCompanyName
)
SELECT t.id, t.DeliveryCompanyName,ISNULL(t.����,0) AS  ��ʿ����,ISNULL(V1.Ӧ���̼�,0) AS Ӧ���̼� ,
ISNULL(V1.Ӧ��������˾,0) AS Ӧ��������˾ ,ISNULL(V1.��ʿ�����ܶ�,0) ��ʿ�����ܶ�,ISNULL(V1.������,0 ) ������,ISNULL(V1.������,0) ������,ISNULL(V1.�����ܽ��,0) �����ܽ�� FROM t
LEFT JOIN 
(
	SELECT d.id AS DeliveryCompanyID,SUM(SettleMoney) AS Ӧ���̼� ,	
	SUM(case when d.SettleType=1 then o.Amount*d.DeliveryCompanyRatio/100 
	when d.SettleType=2 then d.DeliveryCompanySettleMoney*o.OrderCount END) AS Ӧ��������˾, 
	SUM(OrderCommission)AS  ��ʿ�����ܶ�,
	COUNT(1)AS ������,SUM(OrderCount) AS ������,
	SUM(Amount) AS �����ܽ�� FROM [dbo].[order] o (NOLOCK)
    LEFT JOIN   dbo.DeliveryCompany d  (NOLOCK) ON o.DeliveryCompanyID=d.Id
	GROUP BY  d.id
)  V1 ON t.id=V1.DeliveryCompanyId



4	����	61	96951.00	0.00	        30666.00	    2613	23273	456555.70
4	����	61	76387.50	130521.600000	4.00	2084	18128	373144.70

SELECT  TOP 100
* FROM  [dbo].[order] o (NOLOCK)
 WHERE DeliveryCompanyId=4




SELECT ClienterFixMoney,* FROM DeliveryCompany WHERE id=4
case when dc.SettleType=1 then @orderamount*dc.DeliveryCompanyRatio/100 when dc.SettleType=2 then dc.DeliveryCompanySettleMoney*@ordercount