use [superman]
go

/****** Object:  Table [dbo].[DeliveryCompany]    Script Date: 07/13/2015 09:46:06 ******/
set ansi_nulls on
go

set quoted_identifier on
go

set ansi_padding on
go

create table [dbo].[DeliveryCompany](
	[Id] [int] identity(1,1) not null,
	[DeliveryCompanyName] [nvarchar](100) null,
	[DeliveryCompanyCode] [varchar](11) null,
	[IsEnable] [int] not null,
	[SettleType] [int] null,
	[ClienterFixMoney] [decimal](18, 3) null,
	[ClienterSettleRatio] [decimal](18, 2) null,
	[DeliveryCompanySettleMoney] [decimal](18, 3) null,
	[DeliveryCompanyRatio] [decimal](18, 2) null,
	[BusinessQuantity] [int] null,
	[ClienterQuantity] [int] null,
	[CreateTime] [datetime] null,
	[CreateName] [nvarchar](40) null,
	[ModifyName] [nvarchar](40) null,
	[ModifyTime] [datetime] null,
	[IsDisplay] [int] null,
 constraint [PK__Delivery__3214EC0740457975] primary key clustered 
(
	[Id] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [PRIMARY]
) on [PRIMARY]

go

set ansi_padding off
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'Id'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������˾����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'DeliveryCompanyName'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������˾Code��11λ����1��ͷ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'DeliveryCompanyCode'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ���Ч��˾(Ĭ��1��Ч,0��Ч)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'IsEnable'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������ͣ�1���������2�̶���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'SettleType'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʿ�̶����ֵ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'ClienterFixMoney'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʿ�������ֵ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'ClienterSettleRatio'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������˾�̶����ֵ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'DeliveryCompanySettleMoney'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������˾�������ֵ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'DeliveryCompanyRatio'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�̼�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'BusinessQuantity'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʿ����,��������˾���ж�����ʿ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'ClienterQuantity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'CreateName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'ModifyName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸�ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'ModifyTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�����(0����1������)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany', @level2type=N'COLUMN',@level2name=N'IsDisplay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���͹�˾��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompany'
GO

ALTER TABLE [dbo].[DeliveryCompany] ADD  CONSTRAINT [DF__DeliveryC__IsEna__422DC1E7]  DEFAULT ((1)) FOR [IsEnable]
GO

ALTER TABLE [dbo].[DeliveryCompany] ADD  CONSTRAINT [DF__DeliveryC__Clien__4321E620]  DEFAULT ((0)) FOR [ClienterFixMoney]
GO

ALTER TABLE [dbo].[DeliveryCompany] ADD  CONSTRAINT [DF_DeliveryCompany_ClienterSettleRatio]  DEFAULT ((0)) FOR [ClienterSettleRatio]
GO

ALTER TABLE [dbo].[DeliveryCompany] ADD  CONSTRAINT [DF__DeliveryC__Deliv__44160A59]  DEFAULT ((0)) FOR [DeliveryCompanySettleMoney]
GO

ALTER TABLE [dbo].[DeliveryCompany] ADD  CONSTRAINT [DF_DeliveryCompany_DeliveryCompanyRatio]  DEFAULT ((0)) FOR [DeliveryCompanyRatio]
GO

ALTER TABLE [dbo].[DeliveryCompany] ADD  CONSTRAINT [DF_DeliveryCompany_ClienterQuantity]  DEFAULT ((0)) FOR [ClienterQuantity]
GO

ALTER TABLE [dbo].[DeliveryCompany] ADD  CONSTRAINT [DF__DeliveryC__Creat__5540965B]  DEFAULT (getdate()) FOR [CreateTime]
GO

ALTER TABLE [dbo].[DeliveryCompany] ADD  CONSTRAINT [DF_DeliveryCompany_IsDisplay]  DEFAULT ((0)) FOR [IsDisplay]
GO


