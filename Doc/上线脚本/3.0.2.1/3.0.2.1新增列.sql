use superman
go
--�ص���
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

