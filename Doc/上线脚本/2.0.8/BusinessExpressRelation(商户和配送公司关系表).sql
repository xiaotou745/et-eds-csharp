USE [superman]
GO

/****** Object:  Table [dbo].[BusinessExpressRelation]    Script Date: 07/13/2015 10:02:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BusinessExpressRelation](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[BusinessId] [INT] NOT NULL,
	[ExpressId] [INT] NOT NULL,
	[IsEnable] [SMALLINT] NOT NULL,
	[CreateBy] [NVARCHAR](50) NOT NULL,
	[CreateTime] [DATETIME] NOT NULL,
	[UpdateBy] [NVARCHAR](50) NOT NULL,
	[UpdateTime] [DATETIME] NOT NULL,
 CONSTRAINT [PK_BusinessExpressRelation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessExpressRelation', @level2type=N'COLUMN',@level2name=N'BusinessId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送公司ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessExpressRelation', @level2type=N'COLUMN',@level2name=N'ExpressId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效(0:否 1:是)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessExpressRelation', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessExpressRelation', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessExpressRelation', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后一次更新人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessExpressRelation', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后一次更改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessExpressRelation', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO

ALTER TABLE [dbo].[BusinessExpressRelation] ADD  CONSTRAINT [DF_BusinessExpressRelation_BusinessId]  DEFAULT ((0)) FOR [BusinessId]
GO

ALTER TABLE [dbo].[BusinessExpressRelation] ADD  CONSTRAINT [DF_BusinessExpressRelation_ExpressId]  DEFAULT ((0)) FOR [ExpressId]
GO

ALTER TABLE [dbo].[BusinessExpressRelation] ADD  CONSTRAINT [DF_BusinessExpressRelation_IsEnable]  DEFAULT ((1)) FOR [IsEnable]
GO

ALTER TABLE [dbo].[BusinessExpressRelation] ADD  CONSTRAINT [DF_BusinessExpressRelation_CreateBy]  DEFAULT ('') FOR [CreateBy]
GO

ALTER TABLE [dbo].[BusinessExpressRelation] ADD  CONSTRAINT [DF_BusinessExpressRelation_CreateTime]  DEFAULT (GETDATE()) FOR [CreateTime]
GO

ALTER TABLE [dbo].[BusinessExpressRelation] ADD  CONSTRAINT [DF_BusinessExpressRelation_UpdateBy]  DEFAULT ('') FOR [UpdateBy]
GO

ALTER TABLE [dbo].[BusinessExpressRelation] ADD  CONSTRAINT [DF_BusinessExpressRelation_UpdateTime]  DEFAULT (GETDATE()) FOR [UpdateTime]
GO


