----------------������˾��
ALTER TABLE dbo.[DeliveryCompany] ADD [IsShowAccount] int DEFAULT 1 NOT NULL
GO 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ���ʾ����ģ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'IsShowAccount'
go

---------------������˾��־��
ALTER TABLE dbo.[DeliveryCompanyLog] ADD [IsShowAccount] int DEFAULT 1 NOT NULL
GO 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ���ʾ����ģ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'IsShowAccount'
go