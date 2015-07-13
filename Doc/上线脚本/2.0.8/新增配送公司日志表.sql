use [superman]
go

/****** Object:  Table [dbo].[DeliveryCompanyLog]    Script Date: 07/13/2015 13:01:03 ******/
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


