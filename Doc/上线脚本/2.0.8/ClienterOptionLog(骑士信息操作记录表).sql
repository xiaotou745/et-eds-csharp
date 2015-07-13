USE [superman]
GO

/****** Object:  Table [dbo].[ClienterOptionLog]    Script Date: 07/13/2015 10:04:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ClienterOptionLog](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[ClienterId] [INT] NOT NULL,
	[OptId] [INT] NOT NULL,
	[OptName] [VARCHAR](50) NOT NULL,
	[InsertTime] [DATETIME] NOT NULL,
	[Platform] [INT] NOT NULL,
	[Remark] [NVARCHAR](500) NOT NULL,
 CONSTRAINT [PK_ClienterOptionLog] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自增Id(PK)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterOptionLog', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'骑士Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterOptionLog', @level2type=N'COLUMN',@level2name=N'ClienterId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterOptionLog', @level2type=N'COLUMN',@level2name=N'OptId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterOptionLog', @level2type=N'COLUMN',@level2name=N'OptName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterOptionLog', @level2type=N'COLUMN',@level2name=N'InsertTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平台属性：0：商家端;1：配送端;2：服务平台;3：管理后台' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterOptionLog', @level2type=N'COLUMN',@level2name=N'Platform'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterOptionLog', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'骑士信息操作日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterOptionLog'
GO

ALTER TABLE [dbo].[ClienterOptionLog] ADD  DEFAULT ((0)) FOR [ClienterId]
GO

ALTER TABLE [dbo].[ClienterOptionLog] ADD  DEFAULT ((0)) FOR [OptId]
GO

ALTER TABLE [dbo].[ClienterOptionLog] ADD  DEFAULT ('') FOR [OptName]
GO

ALTER TABLE [dbo].[ClienterOptionLog] ADD  DEFAULT (GETDATE()) FOR [InsertTime]
GO

ALTER TABLE [dbo].[ClienterOptionLog] ADD  DEFAULT ((3)) FOR [Platform]
GO

ALTER TABLE [dbo].[ClienterOptionLog] ADD  DEFAULT ('') FOR [Remark]
GO


