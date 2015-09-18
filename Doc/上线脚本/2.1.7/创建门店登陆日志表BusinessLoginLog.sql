USE [superman]
GO

/****** Object:  Table [dbo].[BusinessLoginLog]    Script Date: 09/18/2015 09:53:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BusinessLoginLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BusinessId] [int] NULL,
	[PhoneNo] [nvarchar](20) NOT NULL,
	[SSID] [nvarchar](100) NULL,
	[OperSystem] [nvarchar](20) NULL,
	[OperSystemModel] [nvarchar](20) NULL,
	[PhoneType] [nvarchar](20) NULL,
	[AppVersion] [nvarchar](20) NULL,
	[CreateTime] [datetime] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[IsSuccess] [smallint] NOT NULL,
 CONSTRAINT [PK__Business__3214EC072AE0483B] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog', @level2type=N'COLUMN',@level2name=N'PhoneNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SSID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog', @level2type=N'COLUMN',@level2name=N'SSID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机操作系统android,ios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog', @level2type=N'COLUMN',@level2name=N'OperSystem'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机具体型号5.0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog', @level2type=N'COLUMN',@level2name=N'OperSystemModel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机类型,三星、苹果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog', @level2type=N'COLUMN',@level2name=N'PhoneType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'App版本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog', @level2type=N'COLUMN',@level2name=N'AppVersion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否成功   1 成功   0失败' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog', @level2type=N'COLUMN',@level2name=N'IsSuccess'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商家登录日志表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessLoginLog'
GO

ALTER TABLE [dbo].[BusinessLoginLog] ADD  CONSTRAINT [DF__BusinessL__Creat__2CC890AD]  DEFAULT (getdate()) FOR [CreateTime]
GO


