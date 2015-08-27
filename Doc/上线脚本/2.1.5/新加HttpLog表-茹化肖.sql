USE [superman]
GO

/****** Object:  Table [dbo].[HttpLog]    Script Date: 08/27/2015 15:28:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HttpLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](800) NOT NULL,
	[Htype] [int] NOT NULL,
	[RequestBody] [nvarchar](800) NOT NULL,
	[ResponseBody] [nvarchar](800) NOT NULL,
	[ReuqestMethod] [nvarchar](200) NOT NULL,
	[ReuqestPlatForm] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Remark] [nvarchar](100) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_HttpLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求地址URL(站点请求,调用第三方HTTP请求)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'Url'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作类型 1请求 2响应 3回调 默认0未知' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'Htype'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求体(包括get post参数)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'RequestBody'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'响应体(JSON)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'ResponseBody'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求方法名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'ReuqestMethod'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求平台 0:默认未知 1:管理后台 2:WebApi 3:OpenApi 4 :第三方' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'ReuqestPlatForm'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返回的状态 1 成功 0 失败 默认0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HttpLog', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_Url]  DEFAULT ('') FOR [Url]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_Htype]  DEFAULT ((0)) FOR [Htype]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_RequestBody]  DEFAULT ('') FOR [RequestBody]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_ResponseBody]  DEFAULT ('') FOR [ResponseBody]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_ReuqestMethod]  DEFAULT ('') FOR [ReuqestMethod]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_ReuqestPlatForm]  DEFAULT ((0)) FOR [ReuqestPlatForm]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_Status]  DEFAULT ((0)) FOR [Status]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[HttpLog] ADD  CONSTRAINT [DF_HttpLog_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO


