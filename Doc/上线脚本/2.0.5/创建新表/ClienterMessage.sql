USE [superman]
GO

/****** Object:  Table [dbo].[ClienterMessage]    Script Date: 06/23/2015 16:53:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClienterMessage](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ClienterId] [int] NOT NULL,
	[Content] [nvarchar](1024) NOT NULL,
	[IsRead] [smallint] NOT NULL,
	[PubDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ClienterMessage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ID(PK)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterMessage', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʿId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterMessage', @level2type=N'COLUMN',@level2name=N'ClienterId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ϣ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterMessage', @level2type=N'COLUMN',@level2name=N'Content'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��Ѷ� 0δ�� 1 �Ѷ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterMessage', @level2type=N'COLUMN',@level2name=N'IsRead'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterMessage', @level2type=N'COLUMN',@level2name=N'PubDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʿapp��Ϣ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterMessage'
GO

ALTER TABLE [dbo].[ClienterMessage] ADD  DEFAULT ((0)) FOR [IsRead]
GO

ALTER TABLE [dbo].[ClienterMessage] ADD  DEFAULT (getdate()) FOR [PubDate]
GO


