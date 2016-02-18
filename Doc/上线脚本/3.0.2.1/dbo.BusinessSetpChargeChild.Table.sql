USE [superman]
GO
/****** Object:  Table [dbo].[BusinessSetpChargeChild]    Script Date: 02/18/2016 11:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessSetpChargeChild](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SetpChargeId] [bigint] NOT NULL,
	[MinValue] [decimal](18, 2) NOT NULL,
	[MaxValue] [decimal](18, 2) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ChargeValue] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_BusinessSetpChargeChild] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'步长策略ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpChargeChild', @level2type=N'COLUMN',@level2name=N'SetpChargeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'该阶段最低值(不包含)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpChargeChild', @level2type=N'COLUMN',@level2name=N'MinValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'该区间最高值(包含)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpChargeChild', @level2type=N'COLUMN',@level2name=N'MaxValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收费金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpChargeChild', @level2type=N'COLUMN',@level2name=N'ChargeValue'
GO
ALTER TABLE [dbo].[BusinessSetpChargeChild] ADD  CONSTRAINT [DF_BusinessSetpChargeChild_SetpChargeId]  DEFAULT ((0)) FOR [SetpChargeId]
GO
ALTER TABLE [dbo].[BusinessSetpChargeChild] ADD  CONSTRAINT [DF_BusinessSetpChargeChild_MinValue]  DEFAULT ((0.00)) FOR [MinValue]
GO
ALTER TABLE [dbo].[BusinessSetpChargeChild] ADD  CONSTRAINT [DF_BusinessSetpChargeChild_MaxValue]  DEFAULT ((0.00)) FOR [MaxValue]
GO
ALTER TABLE [dbo].[BusinessSetpChargeChild] ADD  CONSTRAINT [DF_BusinessSetpChargeChild_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[BusinessSetpChargeChild] ADD  CONSTRAINT [DF_BusinessSetpChargeChild_ChargeValue]  DEFAULT ((0.00)) FOR [ChargeValue]
GO
