ALTER TABLE dbo.ClienterWithdrawForm ADD CallBackTime DATETIME DEFAULT GETDATE() NOT NULL
GO 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'易宝回调时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterWithdrawForm', @level2type=N'COLUMN',@level2name=N'CallBackTime'
GO

ALTER TABLE dbo.BusinessWithdrawForm ADD CallBackTime DATETIME DEFAULT GETDATE() NOT NULL
GO 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'易宝回调时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessWithdrawForm', @level2type=N'COLUMN',@level2name=N'CallBackTime'
go


ALTER TABLE dbo.ClienterWithdrawForm ADD CallBackRequestId NVARCHAR(256) DEFAULT '' NOT NULL
GO 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'易宝回调请求Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterWithdrawForm', @level2type=N'COLUMN',@level2name=N'CallBackRequestId'
GO

ALTER TABLE dbo.BusinessWithdrawForm ADD CallBackRequestId NVARCHAR(256) DEFAULT '' NOT NULL
GO 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'易宝回调请求Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessWithdrawForm', @level2type=N'COLUMN',@level2name=N'CallBackRequestId'
go
