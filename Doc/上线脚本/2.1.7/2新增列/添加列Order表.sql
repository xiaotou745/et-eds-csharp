use superman
go
alter table dbo.[order]
add GroupBusinessId int default 0  not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'¼¯ÍÅId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'GroupBusinessId'
GO

