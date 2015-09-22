USE [superman]
GO


CREATE TABLE [GroupBusinessRecharge](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupBusinessId] [int] NOT NULL,
	[PayType] [varchar](100) NOT NULL,
	[OrderNo] [varchar](100) NOT NULL,
	[PayAmount] [decimal](18, 2) NOT NULL,
	[PayStatus] [int] NOT NULL,
	[PayBy] [nvarchar](100) NULL default '',
	[RequestTime] [datetime]  NOT NULL,
	[PayTime] [datetime]  NULL,
	[OriginalOrderNo] [varchar](100)  NULL default ''
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自增id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集团商家id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'GroupBusinessId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'PayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'etao充值单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'充值金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'PayAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'充值状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'PayStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付人账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'PayBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'充值时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'PayTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'RequestTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方平台订单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'OriginalOrderNo'
GO



