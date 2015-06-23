USE [superman]
GO

/****** Object:  Table [dbo].[ClienterAccountChecking]    Script Date: 06/23/2015 09:55:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClienterAccountChecking](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClienterId] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[FlowStatMoney] [decimal](18, 2) NOT NULL,
	[ClienterTotalMoney] [decimal](18, 2) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[LastTotalMoney] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʿID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterAccountChecking', @level2type=N'COLUMN',@level2name=N'ClienterId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterAccountChecking', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ˮ���ϼ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterAccountChecking', @level2type=N'COLUMN',@level2name=N'FlowStatMoney'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʿ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterAccountChecking', @level2type=N'COLUMN',@level2name=N'ClienterTotalMoney'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʼͳ��ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterAccountChecking', @level2type=N'COLUMN',@level2name=N'StartDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ͳ��ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterAccountChecking', @level2type=N'COLUMN',@level2name=N'EndDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʿ���˼�¼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterAccountChecking'
GO

ALTER TABLE [dbo].[ClienterAccountChecking] ADD  DEFAULT ((0)) FOR [LastTotalMoney]
GO

