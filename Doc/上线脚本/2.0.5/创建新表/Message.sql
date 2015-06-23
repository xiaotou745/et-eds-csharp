USE [superman]
GO

/****** Object:  Table [dbo].[Message]    Script Date: 06/18/2015 13:04:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Message](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PushWay] [smallint] NOT NULL,
	[MessageType] [smallint] NOT NULL,
	[Content] [nvarchar](1024) NOT NULL,
	[SentStatus] [smallint] NOT NULL,
	[PushType] [smallint] NOT NULL,
	[PushTarget] [smallint] NOT NULL,
	[PushCity] [nvarchar](2048) NOT NULL,
	[PushPhone] [nvarchar](max) NULL,
	[SendType] [smallint] NOT NULL,
	[SendTime] [datetime] NOT NULL,
	[OverTime] [datetime] NULL,
	[CreateBy] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [nvarchar](50) NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_MESSAGE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ͷ�ʽ1����2app֪ͨ3���ź�app' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'PushWay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ϣ����1֪ͨ2���Ե���3�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'MessageType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ϣ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'Content'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����״̬  0������ 1������ 2�ѷ��� 3 ��ȡ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'SentStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������1ϵͳȺ��2ָ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'PushType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���Ͷ���1�̼�2��ʿ3�̼Һ���ʿ4��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'PushTarget'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ͳ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'PushCity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ֻ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'PushPhone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������1ʱʱ����2��ʱ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'SendType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'SendTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'OverTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Message', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO

ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF__Message__PushWay__62CF9BA3]  DEFAULT ((1)) FOR [PushWay]
GO

ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF__Message__Message__63C3BFDC]  DEFAULT ((1)) FOR [MessageType]
GO

ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF__Message__IsSent__64B7E415]  DEFAULT ((2)) FOR [SentStatus]
GO

ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF__Message__PushTyp__65AC084E]  DEFAULT ((1)) FOR [PushType]
GO

ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF__Message__SendTyp__66A02C87]  DEFAULT ((1)) FOR [SendType]
GO

ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF__Message__SendTim__679450C0]  DEFAULT (getdate()) FOR [SendTime]
GO


