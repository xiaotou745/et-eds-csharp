use superman
go
--回调列
alter table dbo.OrderOther add ReturnUrl nvarchar(400) not null  default ('')
EXEC sp_addextendedproperty N'MS_Description', N'回调地址', 'SCHEMA', N'dbo', 'TABLE', N'OrderOther', 'COLUMN', N'ReturnUrl'
GO


--开放平台商户表
alter table dbo.[group] add PhoneNo varchar(20) not null default('') 
alter table dbo.[group] add [Password] varchar(255) not null default('') 
alter table dbo.[group] add [Description] varchar(255) not null default('') 
alter table dbo.[group] add [AverageCount] INT not null default(0) 
alter table dbo.[group] add [AveragePrice] NUMERIC(9,2) not null default(0.00) 
alter table dbo.[group] add [AuditStatu] INT not null default(0) 
alter table dbo.[group] add [RefuseReason] varchar(255) 
EXEC sp_addextendedproperty N'MS_Description', N'登录帐号', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'PhoneNo'
GO
EXEC sp_addextendedproperty N'MS_Description', N'登录密码', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'Password'
GO
EXEC sp_addextendedproperty N'MS_Description', N'业务描述', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'Description'
GO
EXEC sp_addextendedproperty N'MS_Description', N'日均单量', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'AverageCount'
GO
EXEC sp_addextendedproperty N'MS_Description', N'日均单价', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'AveragePrice'
GO
EXEC sp_addextendedproperty N'MS_Description', N'审核状态 0待审核 1审核通过 2 审核拒绝', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'AuditStatu'
GO
EXEC sp_addextendedproperty N'MS_Description', N'拒绝原因 审核拒绝时必填有效', 'SCHEMA', N'dbo', 'TABLE', N'group', 'COLUMN', N'RefuseReason'
GO

