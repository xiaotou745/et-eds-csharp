USE [superman]
GO

  ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [IDCard] varchar(90) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'个人账户时是身份证号，公司账户时候是营业执照号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'IDCard'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [OpenProvince] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户省' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenProvince'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [OpenCity] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户市' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenCity'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [YeepayKey] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'易宝key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'YeepayKey'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [YeepayStatus] smallint NULL DEFAULT ((1)) ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'易宝账户状态  0正常 1失败' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'YeepayStatus'
GO