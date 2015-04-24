declare @startdate varchar(10)
set @startdate='2015-04-08'

drop table #b,#c,#order,#butie
    
--�̼�����
select @startdate 'date', count(1) 'count' into #b from dbo.business b(nolock) where b.Status=1

--��ʿ����
select @startdate 'date', isnull(sum(case when [Status] = 1 then 1
                        else 0
                   end), 0) as RzqsCount, --��֤��ʿ����
        isnull(sum(case when [Status] = 0 then 1
                        else 0
                   end), 0) as DdrzqsCount --�ȴ���֤��ʿ
                   into #c
from dbo.clienter c(nolock)

--������һЩͳ��
select convert(char(10), PubDate, 120) 'date',
	count(distinct o.businessId) 'businessCount',--��Ծ�̼�����
	count(distinct o.clienterId) 'clientorCount',--��Ծ��ʿ����
	count(1) 'taskCount',--��������
	isnull(sum(case o.Status when 1 then 1 else 0 end),0) 'completeCount',--�������������
	isnull(sum(case o.Status when 2 then 1 else 0 end),0) 'DealingCount',--δ�����������
	isnull(sum(case o.Status when 0 then 1 else 0 end),0) 'notJiedanCount', --δ�ӵ���������
	sum(isnull(o.OrderCount,0)) 'orderCount', --��������
	sum(isnull(o.Amount,0)) 'totalPrice', --�������
	sum(isnull(o.SettleMoney,0)) 'SettleMoney', --Ӧ��
	sum(isnull(o.OrderCommission,0)) 'OrderCommission' --Ӧ��
into #order 
from dbo.[order] o(nolock) 
where o.PubDate>=@startdate and o.Status<>3 
group by convert(char(10), PubDate, 120)

---��̬������һЩͳ��
select convert(char(10), PubDate, 120) 'date',
	isnull(sum(case o.DealCount when 0 then 1 else 0 end),0) 'zeroTime',
	isnull(sum(case o.DealCount when 1 then 1 else 0 end),0) 'oneTime',
	isnull(sum(case o.DealCount when 2 then 1 else 0 end),0) 'twoTime',
	isnull(sum(case o.DealCount when 3 then 1 else 0 end),0) 'threeTime'
into #butie
from dbo.[order] o(nolock)
where o.PubDate>=@startdate and o.Status<>3 
group by convert(char(10), PubDate, 120)


select b.date '����',
	b.count '�̼�����',
	c.RzqsCount '����֤��ʿ����',
	c.DdrzqsCount '����֤��ʿ����',
	o.businessCount '��Ծ�̼�����',
	o.clientorCount '��Ծ��ʿ����',
	o.taskCount '����������',
	o.notJiedanCount 'δ����������',
	o.DealingCount 'δ���������',
	o.orderCount '�ܶ�������',
	convert(decimal(18,2),o.orderCount*1.0 / o.taskCount) '����ƽ��������',
	o.totalPrice '�����ܽ��',
	convert(decimal(18,2),o.totalPrice*1.0 / o.orderCount) '�͵���',
	convert(decimal(18,2),o.totalPrice*1.0 /o.taskCount) '���񵥼�',
	b2.zeroTime '0�β�����������',
	b2.oneTime '1�β�����������',
	b2.twoTime '2�β�����������',
	b2.threeTime '3�β�����������',
	convert(decimal(18,2),o.taskCount*1.0/o.businessCount) '�̼�ƽ����������',
	convert(decimal(18,2),o.orderCount*1.0/o.businessCount) '�̼�ƽ����������',
	convert(decimal(18,2),o.taskCount*1.0/o.clientorCount) '��ʿƽ���������',
	convert(decimal(18,2),o.orderCount*1.0/o.clientorCount) '��ʿƽ����ɶ���',
	o.SettleMoney 'Ӧ��',
	o.OrderCommission 'Ӧ��',
	o.SettleMoney-o.OrderCommission 'ӯ��'
from #b b
	join #c c on b.date = c.date
	join #order o on b.date = o.date
	join #butie b2(nolock) on b.date = b2.date