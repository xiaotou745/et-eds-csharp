USE [superman]
GO

/****** Object:  Table [dbo].[HttpLog]    Script Date: 08/25/2015 10:35:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[HttpLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Url] [varchar](800) NOT NULL,
	[HType] [int] NOT NULL,
	[RequestBody] [varchar](800) NOT NULL,
	[ResponseType] [int] NOT NULL,
	[Msg] [text] NOT NULL,
	[Status] [int] NOT NULL,
	[Remark] [text] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_HttpLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ַ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'Url'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������� 1���� 2��Ӧ 3�ص� Ĭ��0δ֪' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'HType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ӧ���� 1��Ϣ 2�쳣 Ĭ��0δ֪' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'ResponseType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ӧ��������Ϣ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'Msg'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����״̬ 1�ɹ� Ĭ��0ʧ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_Url]  DEFAULT ('') FOR [Url]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_HType]  DEFAULT ((0)) FOR [HType]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_RequestBody]  DEFAULT ('') FOR [RequestBody]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_ResponseType]  DEFAULT ((0)) FOR [ResponseType]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_Msg]  DEFAULT ('') FOR [Msg]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_Status]  DEFAULT ((0)) FOR [Status]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO


