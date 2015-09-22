USE [superman]
GO


CREATE TABLE [GroupBusinessRecharge](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupBusinessId] [int] NOT NULL,
	[PayType] [varchar](100) NOT NULL,
	[OrderNo] [varchar](100) NOT NULL,
	[PayAmount] [decimal](18, 2) NOT NULL,
	[PayStatus] [int] NOT NULL,
	[PayBy] [nvarchar](100) NULL default '',
	[RequestTime] [datetime]  NOT NULL,
	[PayTime] [datetime]  NULL,
	[OriginalOrderNo] [varchar](100)  NULL default ''
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����̼�id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'GroupBusinessId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'PayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'etao��ֵ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֵ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'PayAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֵ״̬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'PayStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧�����˺�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'PayBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֵʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'PayTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'RequestTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ƽ̨������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupBusinessRecharge', @level2type=N'COLUMN',@level2name=N'OriginalOrderNo'
GO



