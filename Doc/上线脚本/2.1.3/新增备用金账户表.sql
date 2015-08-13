USE superman
go

CREATE TABLE [dbo].[ImprestRecharge] (
[Id] int NOT NULL IDENTITY(1,1) ,
[TotalRecharge] numeric(18,2) NOT NULL ,
[RemainingAmount] numeric(18,2) NOT NULL ,
[TotalPayment] numeric(18,2) NOT NULL ,
[CreateTime] datetime NOT NULL DEFAULT (getdate()) 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestRecharge', 
'COLUMN', N'TotalRecharge')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'充值总金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestRecharge'
, @level2type = 'COLUMN', @level2name = N'TotalRecharge'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'充值总金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestRecharge'
, @level2type = 'COLUMN', @level2name = N'TotalRecharge'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestRecharge', 
'COLUMN', N'RemainingAmount')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'剩余金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestRecharge'
, @level2type = 'COLUMN', @level2name = N'RemainingAmount'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'剩余金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestRecharge'
, @level2type = 'COLUMN', @level2name = N'RemainingAmount'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestRecharge', 
'COLUMN', N'TotalPayment')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'累计支出'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestRecharge'
, @level2type = 'COLUMN', @level2name = N'TotalPayment'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'累计支出'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestRecharge'
, @level2type = 'COLUMN', @level2name = N'TotalPayment'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ImprestRecharge', 
'COLUMN', N'CreateTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'创建时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestRecharge'
, @level2type = 'COLUMN', @level2name = N'CreateTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ImprestRecharge'
, @level2type = 'COLUMN', @level2name = N'CreateTime'
GO

-- ----------------------------
-- Indexes structure for table ImprestRecharge
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table ImprestRecharge
-- ----------------------------
ALTER TABLE [dbo].[ImprestRecharge] ADD PRIMARY KEY ([Id])
GO
