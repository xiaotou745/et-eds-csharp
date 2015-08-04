use superman
go
ALTER TABLE [dbo].[OrderOther] ADD [DeductCommissionType] int NULL ;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣除补贴类型: 1 自动扣除    2 人工扣除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderOther', @level2type=N'COLUMN',@level2name=N'DeductCommissionType'
GO
go