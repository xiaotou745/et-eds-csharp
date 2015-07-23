USE [superman]
GO

/****** Object:  Table [dbo].[AccountDeliveryRelation]    Script Date: 07/14/2015 10:15:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccountDeliveryRelation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[DeliveryCompanyID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateBy] [nvarchar](50) NULL,
	[IsEnable] [int] NOT NULL
 CONSTRAINT [PK_AccountDeliveryRelation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自增ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountDeliveryRelation', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountDeliveryRelation', @level2type=N'COLUMN',@level2name=N'AccountId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送公司ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountDeliveryRelation', @level2type=N'COLUMN',@level2name=N'DeliveryCompanyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountDeliveryRelation', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountDeliveryRelation', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用 1 启用 0不启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountDeliveryRelation', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO

ALTER TABLE [dbo].[AccountDeliveryRelation] ADD  CONSTRAINT [DF_AccountDeliveryRelation_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[AccountDeliveryRelation] ADD  CONSTRAINT [DF_AccountDeliveryRelation_IsEnable]  DEFAULT (0) FOR [IsEnable]
GO


