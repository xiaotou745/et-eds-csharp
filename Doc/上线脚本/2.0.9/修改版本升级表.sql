USE [superman]
GO

/****** Object:  Table [dbo].[AppVersion]    Script Date: 07/16/2015 08:57:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF((select COUNT(1) from sysobjects where id = object_id('AppVersion') and type = 'u')>0)
BEGIN
	DROP TABLE AppVersion
END
CREATE TABLE [dbo].[AppVersion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[IsMust] [int] NULL,
	[UpdateUrl] [nvarchar](100) NULL,
	[Message] [nvarchar](500) NULL,
	[PlatForm] [tinyint] NOT NULL,
	[UserType] [tinyint] NOT NULL,
	[CreateDate] [datetime] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[IsTiming] [int] NULL,
	[TimingDate] [datetime] NULL,
	[PubStatus] [int] NOT NULL,
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'UpdateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否定时 0否 1 是' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'IsTiming'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'定时发布时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'TimingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布状态 0待发布 1 已发布 2 取消发布' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion', @level2type=N'COLUMN',@level2name=N'PubStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'App版本控制表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppVersion'
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_IsMust]  DEFAULT ((0)) FOR [IsMust]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_PlatForm]  DEFAULT ((1)) FOR [PlatForm]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_UserType]  DEFAULT ((1)) FOR [UserType]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[AppVersion] ADD  DEFAULT (getdate()) FOR [UpdateDate]
GO

ALTER TABLE [dbo].[AppVersion] ADD  CONSTRAINT [DF_AppVersion_IsTiming]  DEFAULT ((0)) FOR [IsTiming]
GO

ALTER TABLE [dbo].[AppVersion] ADD  DEFAULT ((0)) FOR [PubStatus]
GO


SELECT * FROM dbo.AppVersion


