----�����̼ұ�ʶ��
alter table dbo.business
add Appkey uniqueidentifier default newid() NOT NULL
go



----������ʿ��ʶ��
alter table dbo.clienter
add Appkey uniqueidentifier default newid() NOT NULL
go



