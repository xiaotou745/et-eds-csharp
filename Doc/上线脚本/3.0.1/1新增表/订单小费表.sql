USE [superman]
GO

/****** Object:  Table [dbo].[OrderTipCost]    Script Date: 12/17/2015 11:10:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderTipCost](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Amount] [decimal](9, 2) NOT NULL,
	[CreateName] [nvarchar](40) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[PayStates] [smallint] NOT NULL
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父任务ID(order表)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'OrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'CreateName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'PayStates'
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_Amount]  DEFAULT ((0)) FOR [Amount]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_CreateName]  DEFAULT ('') FOR [CreateName]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_PayStates_1]  DEFAULT ((0)) FOR [PayStates]
GO


