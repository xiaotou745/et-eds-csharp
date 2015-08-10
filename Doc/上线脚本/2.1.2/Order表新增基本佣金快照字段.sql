use superman
go

ALTER TABLE [dbo].[order] ADD [BaseCommission] decimal(18,2) NULL ;--基本补贴佣金



EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'基本补贴佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'BaseCommission'
GO