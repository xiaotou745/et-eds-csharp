CREATE TABLE [dbo].[ClienterBindOptionLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessId] [int] NOT NULL,
	[ClienterId] [int] NOT NULL,
	[OptId] [int] NOT NULL,
	[OptName] [varchar](50) NOT NULL,
	[InsertTime] [datetime] NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_ClienterBindOptionLog] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自增Id(PK)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterBindOptionLog', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商家Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterBindOptionLog', @level2type=N'COLUMN',@level2name=N'BusinessId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'骑士Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterBindOptionLog', @level2type=N'COLUMN',@level2name=N'ClienterId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterBindOptionLog', @level2type=N'COLUMN',@level2name=N'OptId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterBindOptionLog', @level2type=N'COLUMN',@level2name=N'OptName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterBindOptionLog', @level2type=N'COLUMN',@level2name=N'InsertTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterBindOptionLog', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商家信息操作日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterBindOptionLog'
GO

ALTER TABLE [dbo].[ClienterBindOptionLog] ADD  DEFAULT ((0)) FOR [BusinessId]
GO

ALTER TABLE [dbo].[ClienterBindOptionLog] ADD  DEFAULT ((0)) FOR [ClienterId]
GO

ALTER TABLE [dbo].[ClienterBindOptionLog] ADD  DEFAULT ((0)) FOR [OptId]
GO

ALTER TABLE [dbo].[ClienterBindOptionLog] ADD  DEFAULT ('') FOR [OptName]
GO

ALTER TABLE [dbo].[ClienterBindOptionLog] ADD  DEFAULT (getdate()) FOR [InsertTime]
GO

ALTER TABLE [dbo].[ClienterBindOptionLog] ADD  DEFAULT ('') FOR [Remark]
GO


