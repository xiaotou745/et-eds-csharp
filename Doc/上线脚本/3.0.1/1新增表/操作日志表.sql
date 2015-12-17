USE [superman]
GO

/****** Object:  Table [dbo].[OptLog]    Script Date: 11/30/2015 10:37:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OptLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UpdateValue] [nvarchar](3000) NOT NULL,
	[OptId] [int] NOT NULL,
	[OptName] [nvarchar](50) NOT NULL,
	[OptTime] [datetime] NOT NULL,
	[OptType] [int] NOT NULL,
 CONSTRAINT [PK_TaskDistributionConfigLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OptLog', @level2type=N'COLUMN',@level2name=N'UpdateValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OptLog', @level2type=N'COLUMN',@level2name=N'OptId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OptLog', @level2type=N'COLUMN',@level2name=N'OptName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OptLog', @level2type=N'COLUMN',@level2name=N'OptTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1普通任务配送费配置日志 默认0未知' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OptLog', @level2type=N'COLUMN',@level2name=N'OptType'
GO

ALTER TABLE [dbo].[OptLog] ADD  CONSTRAINT [DF_TaskDistributionConfigLog_NewValue]  DEFAULT ('') FOR [UpdateValue]
GO

ALTER TABLE [dbo].[OptLog] ADD  CONSTRAINT [DF_TaskDistributionConfigLog_AdminId]  DEFAULT ((0)) FOR [OptId]
GO

ALTER TABLE [dbo].[OptLog] ADD  CONSTRAINT [DF_TaskDistributionConfigLog_OptName]  DEFAULT ('') FOR [OptName]
GO

ALTER TABLE [dbo].[OptLog] ADD  CONSTRAINT [DF_TaskDistributionConfigLog_OptTime]  DEFAULT (getdate()) FOR [OptTime]
GO

ALTER TABLE [dbo].[OptLog] ADD  CONSTRAINT [DF_OptLog_OptType]  DEFAULT ((0)) FOR [OptType]
GO

