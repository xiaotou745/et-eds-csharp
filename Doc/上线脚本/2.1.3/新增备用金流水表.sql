USE superman
go

CREATE TABLE [dbo].[ImprestBalanceRecord] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Amount] numeric(18,2) NOT NULL ,
[BeforeAmount] numeric(18,2) NOT NULL ,
[AfterAmount] numeric(18,2) NOT NULL ,
[OptName] nvarchar(50) NOT NULL ,
[OptTime] datetime NOT NULL DEFAULT (getdate()) ,
[OptType] int NOT NULL ,
[Remark] nvarchar(500) NOT NULL ,
[ClienterName] nvarchar(20) NULL ,
[ClienterPhoneNo] varchar(50) NULL ,
[ImprestReceiver] nvarchar(20) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[ImprestBalanceRecord]', RESEED, 13)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestBalanceRecord', 
'COLUMN', N'Amount')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'Amount'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'Amount'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestBalanceRecord', 
'COLUMN', N'BeforeAmount')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'操作前金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'BeforeAmount'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'操作前金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'BeforeAmount'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestBalanceRecord', 
'COLUMN', N'AfterAmount')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'操作后金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'AfterAmount'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'操作后金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'AfterAmount'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestBalanceRecord', 
'COLUMN', N'OptName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'操作人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'OptName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'操作人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'OptName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestBalanceRecord', 
'COLUMN', N'OptTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'操作时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'OptTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'操作时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'OptTime'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestBalanceRecord', 
'COLUMN', N'OptType')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'类型：1、充值、2、骑士支出'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'OptType'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'类型：1、充值、2、骑士支出'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'OptType'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestBalanceRecord', 
'COLUMN', N'Remark')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'备注'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'Remark'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'备注'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'Remark'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestBalanceRecord', 
'COLUMN', N'ClienterName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'骑士姓名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'ClienterName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'骑士姓名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'ClienterName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestBalanceRecord', 
'COLUMN', N'ClienterPhoneNo')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'骑士电话'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'ClienterPhoneNo'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'骑士电话'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'ClienterPhoneNo'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestBalanceRecord', 
'COLUMN', N'ImprestReceiver')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'备用金接收人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'ImprestReceiver'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'备用金接收人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestBalanceRecord'
, @level2type = 'COLUMN', @level2name = N'ImprestReceiver'
GO

-- ----------------------------
-- Indexes structure for table ImprestBalanceRecord
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table ImprestBalanceRecord
-- ----------------------------
ALTER TABLE [dbo].[ImprestBalanceRecord] ADD PRIMARY KEY ([Id])
GO
