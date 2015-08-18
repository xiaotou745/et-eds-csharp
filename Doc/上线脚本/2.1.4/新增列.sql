ALTER TABLE dbo.[DeliveryCompany] ADD [IsShowAccount] int DEFAULT 1 NOT NULL
GO 

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否显示记账模块' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'IsShowAccount'
go