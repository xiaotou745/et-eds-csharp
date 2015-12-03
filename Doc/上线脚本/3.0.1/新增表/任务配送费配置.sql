USE [superman]
GO

/****** Object:  Table [dbo].[TaskDistributionConfig]    Script Date: 11/30/2015 10:36:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TaskDistributionConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KM] [float] NOT NULL,
	[KG] [float] NOT NULL,
	[DistributionPrice] [numeric](18, 2) NOT NULL,
	[IsMaster] [int] NOT NULL,
 CONSTRAINT [PK_TaskDistributionConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公里' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDistributionConfig', @level2type=N'COLUMN',@level2name=N'KM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公斤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDistributionConfig', @level2type=N'COLUMN',@level2name=N'KG'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDistributionConfig', @level2type=N'COLUMN',@level2name=N'DistributionPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否为主配置默认0非主，1主配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDistributionConfig', @level2type=N'COLUMN',@level2name=N'IsMaster'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务配送费配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskDistributionConfig'
GO

ALTER TABLE [dbo].[TaskDistributionConfig] ADD  CONSTRAINT [DF_TaskDistributionConfig_KM]  DEFAULT ((0)) FOR [KM]
GO

ALTER TABLE [dbo].[TaskDistributionConfig] ADD  CONSTRAINT [DF_TaskDistributionConfig_KG]  DEFAULT ((0)) FOR [KG]
GO

ALTER TABLE [dbo].[TaskDistributionConfig] ADD  CONSTRAINT [DF_TaskDistributionConfig_DistributionPrice]  DEFAULT ((0)) FOR [DistributionPrice]
GO

ALTER TABLE [dbo].[TaskDistributionConfig] ADD  CONSTRAINT [DF_TaskDistributionConfig_IsBase]  DEFAULT ((0)) FOR [IsMaster]
GO


