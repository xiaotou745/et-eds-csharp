use superman
go
--OrderOther 
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

--商户表
  alter table dbo.business add TaskDistributionId int not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'配置Id 默认0' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'business', 
  @level2type=N'COLUMN',
  @level2name=N'TaskDistributionId'
go

alter table dbo.business add SetpChargeId int not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'阶梯收费策略Id 默认0' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'business', 
  @level2type=N'COLUMN',
  @level2name=N'SetpChargeId'
  go
  
alter table dbo.business add ReceivableType int not null  default 1 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'应收类型 1默认标准(a+b+c) 2,阶梯收费 默认1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'business', 
  @level2type=N'COLUMN',
  @level2name=N'ReceivableType'
  go
  
--配置表
alter table dbo.TaskDistributionConfig add TaskDistributionId int not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'配置Id 默认0' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'TaskDistributionConfig', 
  @level2type=N'COLUMN',
  @level2name=N'TaskDistributionId'
  go
