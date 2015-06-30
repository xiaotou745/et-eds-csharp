alter table  dbo.business
add IsEmployerTask smallint default 0 NOT NULL
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否只显示顾主任务，默认0不允许，1允许' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'business', @level2type=N'COLUMN',@level2name=N'IsEmployerTask'
GO