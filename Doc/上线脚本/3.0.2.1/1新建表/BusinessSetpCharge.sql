USE [superman]
GO
/****** Object:  Table [dbo].[BusinessSetpCharge]    Script Date: 02/18/2016 11:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessSetpCharge](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Remark] [nvarchar](250) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateName] [nvarchar](50) NOT NULL,
	[ModifyName] [nvarchar](50) NOT NULL,
	[Enable] [int] NOT NULL,
	[SetpLength] [decimal](18, 2) NOT NULL,
	[MinLimit] [decimal](18, 2) NOT NULL,
	[MaxLimit] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_BusinessSetpCharge] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'阶梯标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpCharge', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述文本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpCharge', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpCharge', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpCharge', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpCharge', @level2type=N'COLUMN',@level2name=N'CreateName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpCharge', @level2type=N'COLUMN',@level2name=N'ModifyName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用 1 启用 0不启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpCharge', @level2type=N'COLUMN',@level2name=N'Enable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'该策略步长,默认1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpCharge', @level2type=N'COLUMN',@level2name=N'SetpLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最低下限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpCharge', @level2type=N'COLUMN',@level2name=N'MinLimit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大上限(包含)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessSetpCharge', @level2type=N'COLUMN',@level2name=N'MaxLimit'
GO
ALTER TABLE [dbo].[BusinessSetpCharge] ADD  CONSTRAINT [DF_BusinessSetpCharge_Title]  DEFAULT ('') FOR [Title]
GO
ALTER TABLE [dbo].[BusinessSetpCharge] ADD  CONSTRAINT [DF_BusinessSetpCharge_Remark]  DEFAULT ('') FOR [Remark]
GO
ALTER TABLE [dbo].[BusinessSetpCharge] ADD  CONSTRAINT [DF_BusinessSetpCharge_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[BusinessSetpCharge] ADD  CONSTRAINT [DF_BusinessSetpCharge_ModifyDate]  DEFAULT (getdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[BusinessSetpCharge] ADD  CONSTRAINT [DF_BusinessSetpCharge_CreateName]  DEFAULT ('') FOR [CreateName]
GO
ALTER TABLE [dbo].[BusinessSetpCharge] ADD  CONSTRAINT [DF_BusinessSetpCharge_ModifyName]  DEFAULT ('') FOR [ModifyName]
GO
ALTER TABLE [dbo].[BusinessSetpCharge] ADD  CONSTRAINT [DF_BusinessSetpCharge_Enable]  DEFAULT ((1)) FOR [Enable]
GO
ALTER TABLE [dbo].[BusinessSetpCharge] ADD  CONSTRAINT [DF_BusinessSetpCharge_SetpLength]  DEFAULT ((1.00)) FOR [SetpLength]
GO
ALTER TABLE [dbo].[BusinessSetpCharge] ADD  CONSTRAINT [DF_BusinessSetpCharge_MinLimit]  DEFAULT ((0.00)) FOR [MinLimit]
GO
ALTER TABLE [dbo].[BusinessSetpCharge] ADD  CONSTRAINT [DF_BusinessSetpCharge_MaxLimit]  DEFAULT ((0.00)) FOR [MaxLimit]
GO
