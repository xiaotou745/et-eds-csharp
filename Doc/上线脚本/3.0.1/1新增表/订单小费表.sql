USE [superman]
GO

/****** Object:  Table [dbo].[OrderTipCost]    Script Date: 12/21/2015 09:47:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[OrderTipCost](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[TipAmount] [decimal](18, 2) NOT NULL,
	[CreateName] [nvarchar](40) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateName] [nvarchar](40) NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[PayStates] [smallint] NOT NULL,
	[OriginalOrderNo] [varchar](256) NOT NULL,
	[PayType] [smallint] NULL,
	[OutTradeNo] [varchar](256) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ID(order��)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'OrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'С�ѽ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'TipAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'CreateName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'UpdateName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸�ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧��״̬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'PayStates'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ƽ̨������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'OriginalOrderNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧������(1 ֧���� 2 ΢��0�ֽ�)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTipCost', @level2type=N'COLUMN',@level2name=N'PayType'
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_Amount]  DEFAULT ((0)) FOR [Amount]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_TipAmount]  DEFAULT ((0)) FOR [TipAmount]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_CreateName]  DEFAULT ('') FOR [CreateName]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_UpdateName]  DEFAULT ('') FOR [UpdateName]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_UpdateTime]  DEFAULT (getdate()) FOR [UpdateTime]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_PayStates_1]  DEFAULT ((1)) FOR [PayStates]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_OriginalOrderNo]  DEFAULT ('') FOR [OriginalOrderNo]
GO

ALTER TABLE [dbo].[OrderTipCost] ADD  CONSTRAINT [DF_OrderTipCost_PayType]  DEFAULT ((0)) FOR [PayType]
GO


