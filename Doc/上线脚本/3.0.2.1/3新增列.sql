use superman
go
--OrderOther 
alter table dbo.OrderOther add ReturnUrl nvarchar(400) not null  default ('')
EXEC sp_addextendedproperty N'MS_Description', N'�ص���ַ', 'SCHEMA', N'dbo', 'TABLE', N'OrderOther', 'COLUMN', N'ReturnUrl'
GO


--����ƽ̨�̻���
alter table dbo.[group] add PhoneNo varchar(20) not null default('') 
alter table dbo.[group] add [Password] varchar(255) not null default('') 
alter table dbo.[group] add [Description] varchar(255) not null default('') 
alter table dbo.[group] add [AverageCount] INT not null default(0) 
alter table dbo.[group] add [AveragePrice] NUMERIC(9,2) not null default(0.00) 
alter table dbo.[group] add [AuditStatu] INT not null default(0) 
alter table dbo.[group] add [RefuseReason] varchar(255) 
EXEC sp_addextendedproperty N'MS_Description', N'��¼�ʺ�', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'PhoneNo'
GO
EXEC sp_addextendedproperty N'MS_Description', N'��¼����', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'Password'
GO
EXEC sp_addextendedproperty N'MS_Description', N'ҵ������', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'Description'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�վ�����', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'AverageCount'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�վ�����', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'AveragePrice'
GO
EXEC sp_addextendedproperty N'MS_Description', N'���״̬ 0����� 1���ͨ�� 2 ��˾ܾ�', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'AuditStatu'
GO
EXEC sp_addextendedproperty N'MS_Description', N'�ܾ�ԭ�� ��˾ܾ�ʱ������Ч', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'RefuseReason'
GO

--�̻���
  alter table dbo.business add TaskDistributionId int not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����Id Ĭ��0' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'business', 
  @level2type=N'COLUMN',
  @level2name=N'TaskDistributionId'
go

alter table dbo.business add SetpChargeId int not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�����շѲ���Id Ĭ��0' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'business', 
  @level2type=N'COLUMN',
  @level2name=N'SetpChargeId'
  go
  
alter table dbo.business add ReceivableType int not null  default 1 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'Ӧ������ 1Ĭ�ϱ�׼(a+b+c) 2,�����շ� Ĭ��1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'business', 
  @level2type=N'COLUMN',
  @level2name=N'ReceivableType'
  go
  
--���ñ�
alter table dbo.TaskDistributionConfig add TaskDistributionId int not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����Id Ĭ��0' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'TaskDistributionConfig', 
  @level2type=N'COLUMN',
  @level2name=N'TaskDistributionId'
  go
