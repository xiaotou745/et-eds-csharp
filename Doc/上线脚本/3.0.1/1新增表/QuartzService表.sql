USE [superman]
GO

/****** Object:  Table [dbo].[QuartzService]    Script Date: 12/17/2015 11:23:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[QuartzService](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FilePath] [nvarchar](50) NOT NULL,
	[Packages] [varchar](500) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[ExecTime] [varchar](50) NOT NULL,
	[IsStart] [int] NOT NULL,
	[Remark] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_QuartzServiceTbl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuartzService', @level2type=N'COLUMN',@level2name=N'FilePath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包名类名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuartzService', @level2type=N'COLUMN',@level2name=N'Packages'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuartzService', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuartzService', @level2type=N'COLUMN',@level2name=N'ExecTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否开启1开启，默认0关闭' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuartzService', @level2type=N'COLUMN',@level2name=N'IsStart'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuartzService', @level2type=N'COLUMN',@level2name=N'Remark'
GO

ALTER TABLE [dbo].[QuartzService] ADD  CONSTRAINT [DF_QuartzService_IsStart]  DEFAULT ((0)) FOR [IsStart]
GO

ALTER TABLE [dbo].[QuartzService] ADD  CONSTRAINT [DF_QuartzService_Remark]  DEFAULT ('') FOR [Remark]
GO


