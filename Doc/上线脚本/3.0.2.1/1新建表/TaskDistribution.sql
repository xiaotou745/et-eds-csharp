USE [superman]
GO

/****** Object:  Table [dbo].[TaskDistribution]    Script Date: 02/18/2016 16:23:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TaskDistribution](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[CreateName] [nvarchar](40) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateName] [nvarchar](40) NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TaskDistribution] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDistribution', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDistribution', @level2type=N'COLUMN',@level2name=N'CreateName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDistribution', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDistribution', @level2type=N'COLUMN',@level2name=N'UpdateName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDistribution', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO

ALTER TABLE [dbo].[TaskDistribution] ADD  CONSTRAINT [DF_TaskDistribution_Name]  DEFAULT ('') FOR [Name]
GO

ALTER TABLE [dbo].[TaskDistribution] ADD  CONSTRAINT [DF_TaskDistribution_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[TaskDistribution] ADD  CONSTRAINT [DF_TaskDistribution_CreateName]  DEFAULT ('') FOR [CreateName]
GO

ALTER TABLE [dbo].[TaskDistribution] ADD  CONSTRAINT [DF_TaskDistribution_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

ALTER TABLE [dbo].[TaskDistribution] ADD  CONSTRAINT [DF_TaskDistribution_UpdateName]  DEFAULT ('') FOR [UpdateName]
GO

ALTER TABLE [dbo].[TaskDistribution] ADD  CONSTRAINT [DF_TaskDistribution_UpdateTime]  DEFAULT (getdate()) FOR [UpdateTime]
GO


