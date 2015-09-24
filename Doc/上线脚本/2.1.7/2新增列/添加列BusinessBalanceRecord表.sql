use superman
go
--
alter table dbo.BusinessBalanceRecord
add GroupId int default 0  not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集团Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessBalanceRecord', @level2type=N'COLUMN',@level2name=N'GroupId'
GO

alter table dbo.BusinessBalanceRecord
add GroupAfterBalance decimal(18, 3) default 0  not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集团操作后金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessBalanceRecord', @level2type=N'COLUMN',@level2name=N'GroupAfterBalance'
GO

alter table dbo.BusinessBalanceRecord
add GroupAmount decimal(18, 3) default 0  not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集团金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessBalanceRecord', @level2type=N'COLUMN',@level2name=N'GroupAmount'
GO




alter table business
add LastLoginTime datetime null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后一次登录时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'business', @level2type=N'COLUMN',@level2name=N'LastLoginTime'
GO
