use superman
go
ALTER TABLE [dbo].[OrderOther] ADD [DeductCommissionType] int NULL ;

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�۳���������: 1 �Զ��۳�    2 �˹��۳�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderOther', @level2type=N'COLUMN',@level2name=N'DeductCommissionType'
GO
go