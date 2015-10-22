USE [superman]
GO

/****** Object:  Table [dbo].[AlipayBatch]    Script Date: 10/22/2015 14:46:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AlipayBatch](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchNo] [varchar](50) NOT NULL,
	[TotalWithdraw] [decimal](18, 3) NOT NULL,
	[OptTimes] [smallint] NOT NULL,
	[SuccessTimes] [smallint] NOT NULL,
	[FailTimes] [smallint] NOT NULL,
	[Status] [smallint] NOT NULL,
	[WithdrawNos] [varchar](512) NOT NULL,
	[WithdrawIds] [varchar](512) NOT NULL,
	[CreateBy] [varchar](100) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CallbackTime] [datetime] NULL,
	[LastOptUser] [varchar](100) NOT NULL,
	[LastOptTime] [datetime] NOT NULL,
	[Remarks] [varchar](512) NOT NULL,
 CONSTRAINT [PK_AlipayBatch] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ID(PK)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ε���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'BatchNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ֽ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'TotalWithdraw'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'OptTimes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɹ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'SuccessTimes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʧ�ܱ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'FailTimes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ε�״̬  0����� 1 ������ Ĭ��0 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ε��������ֵ��ż���  ������ֵ����� '','' �ָ� ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'WithdrawNos'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ε��������ֵ�id����  ������ֵ�id�� '','' �ָ� ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'WithdrawIds'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ε�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ε�����ʱ�� Ĭ��ϵͳʱ�� ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧�����ص�ʱ�� ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'CallbackTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'LastOptUser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch', @level2type=N'COLUMN',@level2name=N'LastOptTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧�����������α�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AlipayBatch'
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch_TotalWithdraw]  DEFAULT ((0)) FOR [TotalWithdraw]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__OptTimes]  DEFAULT ((0)) FOR [OptTimes]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__SuccessTimes]  DEFAULT ((0)) FOR [SuccessTimes]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__FailTimes]  DEFAULT ((0)) FOR [FailTimes]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__Status]  DEFAULT ((0)) FOR [Status]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__WithdrawNos]  DEFAULT ('') FOR [WithdrawNos]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__WithdrawIds]  DEFAULT ('') FOR [WithdrawIds]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__CreateBy]  DEFAULT ('') FOR [CreateBy]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__LastOptUser]  DEFAULT ('') FOR [LastOptUser]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__LastOptTime]  DEFAULT (getdate()) FOR [LastOptTime]
GO

ALTER TABLE [dbo].[AlipayBatch] ADD  CONSTRAINT [DF__AlipayBatch__Remarks]  DEFAULT ('') FOR [Remarks]
GO


