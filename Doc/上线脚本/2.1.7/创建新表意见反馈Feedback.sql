
USE [superman]
GO

/****** Object:  Table [dbo].[Feedback]    Script Date: 09/14/2015 16:25:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Feedback](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FeedbackId] [int] NOT NULL,
	[UserType] [smallint] NOT NULL,
	[FeedbackType] [smallint] NOT NULL,
	[Content] [nvarchar](1024) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_BusinessFeedback] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PKID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Feedback', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投诉人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Feedback', @level2type=N'COLUMN',@level2name=N'FeedbackId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户类型1、门店2、骑士' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Feedback', @level2type=N'COLUMN',@level2name=N'UserType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'反馈类型(1功能意见、2页面意见、3您的新需求、4操作意见、5其它)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Feedback', @level2type=N'COLUMN',@level2name=N'FeedbackType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'反馈内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Feedback', @level2type=N'COLUMN',@level2name=N'Content'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Feedback', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

ALTER TABLE [dbo].[Feedback] ADD  CONSTRAINT [DF_BusinessFeedback_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO


