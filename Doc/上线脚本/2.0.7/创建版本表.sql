USE [superman]
GO

/****** Object:  Table [dbo].[AppVersion]    Script Date: 07/03/2015 16:42:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AppVersion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[IsMust] [bit] NOT NULL,
	[UpdateUrl] [nvarchar](100) NULL,
	[Message] [nvarchar](500) NULL,
	[PlatForm] [tinyint] NOT NULL,
	[UserType] [tinyint] NOT NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_AppVersion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'Version'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否强制升级 1 是 0否 默认0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'IsMust'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下载地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'UpdateUrl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'升级信息 可以不填' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'Message'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户端类型 1:Android 2 :IOS 默认Android' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'PlatForm'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户版本 1 骑士 2 商家 默认1骑士' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'UserType'
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_IsMust]  DEFAULT ((0)) FOR [IsMust]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_PlatForm]  DEFAULT ((1)) FOR [PlatForm]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_UserType]  DEFAULT ((1)) FOR [UserType]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO


