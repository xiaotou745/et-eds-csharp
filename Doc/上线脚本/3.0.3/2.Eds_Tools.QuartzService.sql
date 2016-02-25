USE [Eds_Tools]
GO

/****** Object:  Table [dbo].[QuartzService]    Script Date: 02/16/2016 11:47:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[QuartzService](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppSource] [int] NOT NULL,
	[ReqUrl] [varchar](500) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[ExecTime] [varchar](50) NOT NULL,
	[IsStart] [int] NOT NULL,
	[Remark] [nvarchar](4000) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateName] [nvarchar](50) NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[UpdateName] [nvarchar](50) NOT NULL
 CONSTRAINT [PK_QuartzServiceTbl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'app名称id，1是易代送，2是人人推' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuartzService', @level2type=N'COLUMN',@level2name=N'AppSource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务的请求地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuartzService', @level2type=N'COLUMN',@level2name=N'ReqUrl'
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

ALTER TABLE [dbo].[QuartzService] ADD  CONSTRAINT [DF_QuartzService_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

ALTER TABLE [dbo].[QuartzService] ADD  CONSTRAINT [DF_QuartzService_CreateName]  DEFAULT ('') FOR [CreateName]
GO

ALTER TABLE [dbo].[QuartzService] ADD  CONSTRAINT [DF_QuartzService_UpdateTime]  DEFAULT (getdate()) FOR [UpdateTime]
GO

ALTER TABLE [dbo].[QuartzService] ADD  CONSTRAINT [DF_QuartzService_UpdateName]  DEFAULT ('') FOR [UpdateName]
GO

ALTER TABLE [dbo].[QuartzService] ADD  CONSTRAINT [DF_QuartzService_AppSource]  DEFAULT ((0)) FOR [AppSource]
GO

ALTER TABLE [dbo].[QuartzService] ADD  CONSTRAINT [DF_QuartzService_ReqUrl]  DEFAULT ('') FOR [ReqUrl]
GO


