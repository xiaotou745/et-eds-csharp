USE [superman]
GO

/****** Object:  Table [dbo].[GroupBusinessRelation]    Script Date: 09/22/2015 22:13:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GroupBusinessRelation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[BusinessId] [int] NOT NULL,
	[CreateBy] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateBy] [nvarchar](50) NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsBind] [smallint] NOT NULL,
	[IsEnable] [smallint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRelation', @level2type=N'COLUMN',@level2name=N'GroupId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�̻�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRelation', @level2type=N'COLUMN',@level2name=N'BusinessId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRelation', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRelation', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���һ�θ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRelation', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���һ�θ���ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRelation', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��(0:�� 1:��)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRelation', @level2type=N'COLUMN',@level2name=N'IsBind'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�����(0:�� 1:��)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRelation', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ŵ��ϵ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRelation'
GO

ALTER TABLE [dbo].[GroupBusinessRelation] ADD  DEFAULT ((0)) FOR [GroupId]
GO

ALTER TABLE [dbo].[GroupBusinessRelation] ADD  DEFAULT ((0)) FOR [BusinessId]
GO

ALTER TABLE [dbo].[GroupBusinessRelation] ADD  DEFAULT ('') FOR [CreateBy]
GO

ALTER TABLE [dbo].[GroupBusinessRelation] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO

ALTER TABLE [dbo].[GroupBusinessRelation] ADD  DEFAULT ('') FOR [UpdateBy]
GO

ALTER TABLE [dbo].[GroupBusinessRelation] ADD  DEFAULT (getdate()) FOR [UpdateTime]
GO

ALTER TABLE [dbo].[GroupBusinessRelation] ADD  DEFAULT ((1)) FOR [IsBind]
GO

ALTER TABLE [dbo].[GroupBusinessRelation] ADD  DEFAULT ((1)) FOR [IsEnable]
GO


