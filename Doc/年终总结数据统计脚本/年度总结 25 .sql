use  superman
go


--25 
--�汾���Ƽ�¼����
select SUM(1) from dbo.AppVersion 
--ÿ�·������� 
SELECT  SUM(1) as ����,convert(varchar(7),CreateDate,120) �·�  FROM dbo.AppVersion(nolock)  group by   convert(varchar(7),CreateDate,120)






--26-29  ��ֵͳ��

--����
select  CONVERT(VARCHAR(7), PayTime, 120) �·� ,
        SUM(1) as ���� ,
        SUM(payAmount) ��ֵ�ܽ�� ,
        CASE PayType
          when ( 1 ) then '֧����'
          when ( 2 ) then '΢��'
          when ( 3 ) then '��̨'
          when ( 4 ) then '����'
        end as ��ֵ��ʽ
from    dbo.BusinessRecharge(nolock)
where PayTime<'2016-01-01 00:00:00'
group by CONVERT(VARCHAR(7), PayTime, 120) ,
        PayType
order by PayType ,
        CONVERT(VARCHAR(7), PayTime, 120)

--���в�ѯ
select  CONVERT(VARCHAR(7), PayTime, 120) �·� ,
        SUM(1) as ���� ,
        SUM(payAmount) ��ֵ�ܽ�� ,
        CASE PayType
          when ( 1 ) then '֧����'
          when ( 2 ) then '΢��'
          when ( 3 ) then '��̨'
          when ( 4 ) then '����'
        end as ��ֵ��ʽ ,
        business.City as ����
from    dbo.BusinessRecharge(nolock)
        join dbo.business on dbo.BusinessRecharge.BusinessId = business.id
where   PayTime < '2016-01-01 00:00:00'
group by CONVERT(VARCHAR(7), PayTime, 120) ,
        PayType ,
        dbo.business.City
order by PayType ,
        CONVERT(VARCHAR(7), PayTime, 120) ,
        dbo.business.City


-- 33 �������ͳ��
--��������  
select  COUNT(1)
from    dbo.[order]  (nolock)
where   PubDate < '2016-01-01 00:00:00'

--�������
select  COUNT(1)
from    OrderOther(nolock)
        join [order](nolock) on dbo.OrderOther.OrderId = dbo.[order].Id
where   AuditStatus != 0
        and AuditOptName is not null
        and PubDate < '2016-01-01 00:00:00'
        
        
--�¶����������������
select  CONVERT(VARCHAR(7), PubDate, 120) �·� ,
        COUNT(dbo.[order].Id) as ���¶������� ,
        SUM(CASE when AuditStatus != 0
                      and AuditOptName is not null then 1
                 else 0
            end) �����˹���˶�����
from    [order] (nolock)
        left  join OrderOther(nolock) on dbo.OrderOther.OrderId = dbo.[order].Id
where   PubDate < '2016-01-01 00:00:00'
group by CONVERT(VARCHAR(7), PubDate, 120)
order by CONVERT(VARCHAR(7), PubDate, 120)