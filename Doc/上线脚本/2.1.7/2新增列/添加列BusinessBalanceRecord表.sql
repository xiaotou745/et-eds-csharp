use superman
go
--
alter table dbo.BusinessBalanceRecord
add GroupId int default 0  not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessBalanceRecord', @level2type=N'COLUMN',@level2name=N'GroupId'
GO

alter table dbo.BusinessBalanceRecord
add GroupAfterBalance decimal(18, 3) default 0  not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���Ų�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessBalanceRecord', @level2type=N'COLUMN',@level2name=N'GroupAfterBalance'
GO

alter table dbo.BusinessBalanceRecord
add GroupAmount decimal(18, 3) default 0  not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���Ž��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessBalanceRecord', @level2type=N'COLUMN',@level2name=N'GroupAmount'
GO
