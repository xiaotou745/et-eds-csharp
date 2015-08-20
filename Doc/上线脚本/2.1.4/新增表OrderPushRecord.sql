USE [superman]
GO

/****** Object:  Table [dbo].[OrderPushRecord]    Script Date: 08/20/2015 09:34:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderPushRecord](
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL,
	[OrderId] [INT] NOT NULL,
	[ClienterIdList] [NVARCHAR](MAX) NULL,
	[TaskType] [INT] NOT NULL,
	[PushTime] [DATETIME] NOT NULL,
	[PushCount] [INT] NOT NULL,
	[ClienterCount] [INT] NOT NULL,
	[IsEnable] [INT] NOT NULL,
	[Remark] [NVARCHAR](MAX) NULL,
 CONSTRAINT [PK_OrderPushRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPushRecord', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPushRecord', @level2type=N'COLUMN',@level2name=N'OrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʿId����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPushRecord', @level2type=N'COLUMN',@level2name=N'ClienterIdList'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������(0:�ǵ������� 1:��������)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPushRecord', @level2type=N'COLUMN',@level2name=N'TaskType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPushRecord', @level2type=N'COLUMN',@level2name=N'PushTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʹ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPushRecord', @level2type=N'COLUMN',@level2name=N'PushCount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ʿ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPushRecord', @level2type=N'COLUMN',@level2name=N'ClienterCount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����Ƿ�ɹ�(0:ʧ�� 1:�ɹ�)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPushRecord', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPushRecord', @level2type=N'COLUMN',@level2name=N'Remark'
GO

ALTER TABLE [dbo].[OrderPushRecord] ADD  CONSTRAINT [DF_Table_1_PushType]  DEFAULT ((0)) FOR [TaskType]
GO

ALTER TABLE [dbo].[OrderPushRecord] ADD  CONSTRAINT [DF_OrderPushRecord_PushTime]  DEFAULT (GETDATE()) FOR [PushTime]
GO

ALTER TABLE [dbo].[OrderPushRecord] ADD  CONSTRAINT [DF_OrderPushRecord_PushCount]  DEFAULT ((0)) FOR [PushCount]
GO

ALTER TABLE [dbo].[OrderPushRecord] ADD  CONSTRAINT [DF_Table_1_ClienterQty]  DEFAULT ((0)) FOR [ClienterCount]
GO

ALTER TABLE [dbo].[OrderPushRecord] ADD  CONSTRAINT [DF_OrderPushRecord_IsEnable]  DEFAULT ((1)) FOR [IsEnable]
GO


