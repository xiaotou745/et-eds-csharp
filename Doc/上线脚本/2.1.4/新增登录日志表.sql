USE [superman]
GO

/****** Object:  Table [dbo].[AccountLoginLog]    Script Date: 08/18/2015 18:11:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AccountLoginLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[LoginTime] [datetime] NOT NULL,
	[Mac] [nvarchar](50) NOT NULL,
	[LoginType] [int] NOT NULL,
	[Ip] [varchar](50) NOT NULL,
	[Browser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_AccountLoginLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountLoginLog', @level2type=N'COLUMN',@level2name=N'LoginName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountLoginLog', @level2type=N'COLUMN',@level2name=N'LoginTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mac地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountLoginLog', @level2type=N'COLUMN',@level2name=N'Mac'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录类型0失败，1登录成功，2退出登录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountLoginLog', @level2type=N'COLUMN',@level2name=N'LoginType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'浏览器' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountLoginLog', @level2type=N'COLUMN',@level2name=N'Browser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountLoginLog', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理后台登录日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountLoginLog'
GO

ALTER TABLE [dbo].[AccountLoginLog] ADD  CONSTRAINT [DF_AccountLoginLog_LoginTime]  DEFAULT (getdate()) FOR [LoginTime]
GO

ALTER TABLE [dbo].[AccountLoginLog] ADD  CONSTRAINT [DF_AccountLoginLog_Mac]  DEFAULT ('') FOR [Mac]
GO

ALTER TABLE [dbo].[AccountLoginLog] ADD  CONSTRAINT [DF_AccountLoginLog_LoginType]  DEFAULT ((0)) FOR [LoginType]
GO

ALTER TABLE [dbo].[AccountLoginLog] ADD  CONSTRAINT [DF_AccountLoginLog_Ip]  DEFAULT ('') FOR [Ip]
GO

ALTER TABLE [dbo].[AccountLoginLog] ADD  CONSTRAINT [DF_AccountLoginLog_Browser]  DEFAULT ('') FOR [Browser]
GO

ALTER TABLE [dbo].[AccountLoginLog] ADD  CONSTRAINT [DF_AccountLoginLog_Remark]  DEFAULT ('') FOR [Remark]
GO


