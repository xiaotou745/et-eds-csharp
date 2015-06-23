USE [superman]
GO

/****** Object:  Table [dbo].[BusinessMessage]    Script Date: 06/23/2015 16:54:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BusinessMessage](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BusinessId] [int] NOT NULL,
	[Content] [nvarchar](1024) NOT NULL,
	[IsRead] [smallint] NOT NULL,
	[PubDate] [datetime] NOT NULL,
 CONSTRAINT [PK_BusinessMessage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自增ID(PK)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessMessage', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessMessage', @level2type=N'COLUMN',@level2name=N'BusinessId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息体' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessMessage', @level2type=N'COLUMN',@level2name=N'Content'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已读 0未读 1 已读' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessMessage', @level2type=N'COLUMN',@level2name=N'IsRead'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推送时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessMessage', @level2type=N'COLUMN',@level2name=N'PubDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商户app消息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessMessage'
GO

ALTER TABLE [dbo].[BusinessMessage] ADD  DEFAULT ((0)) FOR [IsRead]
GO

ALTER TABLE [dbo].[BusinessMessage] ADD  DEFAULT (getdate()) FOR [PubDate]
GO


