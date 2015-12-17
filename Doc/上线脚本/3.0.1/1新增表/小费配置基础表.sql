USE [superman]
GO

/****** Object:  Table [dbo].[OrderTip]    Script Date: 12/17/2015 10:59:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderTip](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](9, 2) NOT NULL,
	[CreateName] [nvarchar](40) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ModifyName] [nvarchar](40) NOT NULL,
	[ModifyTime] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderTip] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTip', @level2type=N'COLUMN',@level2name=N'CreateName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTip', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTip', @level2type=N'COLUMN',@level2name=N'ModifyName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderTip', @level2type=N'COLUMN',@level2name=N'ModifyTime'
GO

ALTER TABLE [dbo].[OrderTip] ADD  CONSTRAINT [DF_OrderTip_Amount]  DEFAULT ((0)) FOR [Amount]
GO

ALTER TABLE [dbo].[OrderTip] ADD  CONSTRAINT [DF_OrderTip_CreateName]  DEFAULT ('') FOR [CreateName]
GO

ALTER TABLE [dbo].[OrderTip] ADD  CONSTRAINT [DF_OrderTip_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

ALTER TABLE [dbo].[OrderTip] ADD  CONSTRAINT [DF_OrderTip_ModifyName]  DEFAULT ('') FOR [ModifyName]
GO

ALTER TABLE [dbo].[OrderTip] ADD  CONSTRAINT [DF_OrderTip_ModifyTime]  DEFAULT (getdate()) FOR [ModifyTime]
GO


