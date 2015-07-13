use [superman]
go

/****** Object:  Table [dbo].[DeliveryCompanyLog]    Script Date: 07/13/2015 13:11:43 ******/
set ansi_nulls on
go

set quoted_identifier on
go

create table [dbo].[DeliveryCompanyLog](
	[Id] [int] identity(1,1) not null,
	[DeliveryCompanyId] [int] not null,
	[DeliveryCompanyName] [nvarchar](200) null,
	[IsEnable] [int] null,
	[SettleType] [int] null,
	[ClienterFixMoney] [decimal](9, 3) null,
	[ClienterSettleRatio] [decimal](9, 2) null,
	[DeliveryCompanySettleMoney] [decimal](9, 3) null,
	[DeliveryCompanyRatio] [decimal](9, 2) null,
	[BusinessQuantity] [int] null,
	[ClienterQuantity] [int] null,
	[IsDisplay] [int] null,
	[ModifyName] [nvarchar](50) null,
	[ModifyTime] [datetime] null,
 constraint [PK_DeliveryCompanyLog] primary key clustered 
(
	[Id] asc
)with (pad_index  = off, statistics_norecompute  = off, ignore_dup_key = off, allow_row_locks  = on, allow_page_locks  = on) on [PRIMARY]
) on [PRIMARY]

go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送公司Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'DeliveryCompanyId'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配送公司名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'DeliveryCompanyName'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结算类型（1结算比例、2固定金额）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'SettleType'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'骑士固定金额值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'ClienterFixMoney'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'骑士结算比例值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'ClienterSettleRatio'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物流公司固定金额值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'DeliveryCompanySettleMoney'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物流公司结算比例值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'DeliveryCompanyRatio'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商家数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'BusinessQuantity'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'骑士数量,该物流公司下有多少骑士' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'ClienterQuantity'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否隐藏(0隐藏1不隐藏)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'IsDisplay'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'ModifyName'
go

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliveryCompanyLog', @level2type=N'COLUMN',@level2name=N'ModifyTime'
GO


