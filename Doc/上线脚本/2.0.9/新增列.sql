use superman
go

----------------------------------------------订单表的物流公司结算金额+配送的物流公司id

ALTER TABLE dbo.[order]
ADD DeliveryCompanySettleMoney numeric(18,2) DEFAULT 0 NOT NULL
GO 
ALTER TABLE dbo.[order]
ADD DeliveryCompanyID int DEFAULT 0 NOT NULL
GO 
--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'物流公司结算金额' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'DeliveryCompanySettleMoney'
GO
--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'配送的物流公司id' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'DeliveryCompanyID'
GO

-----------------------------------------骑士表新增注册用到的时间戳
ALTER TABLE [dbo].[clienter] ADD [Timespan] nvarchar(50) COLLATE Chinese_PRC_CI_AS NULL ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'时间戳' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'clienter', @level2type=N'COLUMN',@level2name=N'Timespan'
GO


-----------------------------------------商户表新增注册用到的时间戳
ALTER TABLE [dbo].[business] ADD [Timespan] nvarchar(50) COLLATE Chinese_PRC_CI_AS NULL ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'时间戳' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'business', @level2type=N'COLUMN',@level2name=N'Timespan'
GO

----------------------------------------骑士银行卡绑定
ALTER TABLE [dbo].[ClienterFinanceAccount] ADD [IDCard] varchar(90) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'个人账户时是身份证号，公司账户时候是营业执照号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'IDCard'
GO
ALTER TABLE [dbo].[ClienterFinanceAccount] ADD [OpenProvince] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户省' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenProvince'
GO
ALTER TABLE [dbo].[ClienterFinanceAccount] ADD [OpenCity] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户市' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenCity'
GO
ALTER TABLE [dbo].[ClienterFinanceAccount] ADD [YeepayKey] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'易宝key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'YeepayKey'
GO
ALTER TABLE [dbo].[ClienterFinanceAccount] ADD [YeepayStatus] smallint NULL DEFAULT ((1)) ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'易宝账户状态  0正常 1失败' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'YeepayStatus'
GO
alter table dbo.ClienterFinanceAccount
add OpenProvinceCode int null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户省Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenProvinceCode'
GO
alter table dbo.ClienterFinanceAccount
add OpenCityCode int null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户市区Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenCityCode'
GO


----------------------------------------商户银行卡绑定
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [IDCard] varchar(90) NULL DEFAULT '' ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'个人账户时是身份证号，公司账户时候是营业执照号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'IDCard'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [OpenProvince] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户省' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenProvince'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [OpenCity] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户市' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenCity'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [YeepayKey] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'易宝key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'YeepayKey'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [YeepayStatus] smallint NULL DEFAULT ((1)) ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'易宝账户状态  0正常 1失败' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'YeepayStatus'
GO
alter table dbo.BusinessFinanceAccount
add OpenProvinceCode int null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户省Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenProvinceCode'
GO
alter table dbo.BusinessFinanceAccount
add OpenCityCode int null
go
exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户市区Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenCityCode'
GO 


---------------------------------------------------修改商户提现记录表新增字段
ALTER TABLE dbo.BusinessWithdrawForm
ADD OpenProvince NVARCHAR(25)   NOT NULL DEFAULT('')  --省份
GO 
ALTER TABLE dbo.BusinessWithdrawForm
ADD OpenCity  NVARCHAR(25)  NOT NULL DEFAULT('')  --城市
GO 
ALTER TABLE dbo.BusinessWithdrawForm
ADD OpenProvinceCode INT   NOT NULL DEFAULT(0)  --省份Code
GO 
ALTER TABLE dbo.BusinessWithdrawForm
ADD OpenCityCode INT   NOT NULL DEFAULT(0)  --城市Code
GO 
ALTER TABLE dbo.BusinessWithdrawForm
ADD IDCard NVARCHAR(20)   NOT NULL DEFAULT('')  --申请提现身份证号
GO 
ALTER TABLE dbo.BusinessWithdrawForm
ADD HandChargeThreshold decimal   NOT NULL DEFAULT(0)  --手续费阈值,例如100
GO 

ALTER TABLE dbo.BusinessWithdrawForm
ADD HandCharge decimal   NOT NULL DEFAULT(0)  --手续费,例如1元
GO 

ALTER TABLE dbo.BusinessWithdrawForm
ADD HandChargeOutlay INT   NOT NULL DEFAULT(0)  --手续费支出方:0个人,1易代送
GO 
--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'手续费阈值,例如100' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandChargeThreshold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'手续费,例如1元' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandCharge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'手续费支出方:0个人,1易代送' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandChargeOutlay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'省份' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenProvince'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'城市' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'省份Code' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenProvinceCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'城市Code' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenCityCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'申请提现身份证号' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'IDCard'
GO
    




--------------------------------------------------修改提现记录表新增字段209
    
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenProvince NVARCHAR(25)   NOT NULL DEFAULT('')  --省份
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenCity  NVARCHAR(25)  NOT NULL DEFAULT('')  --城市
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenProvinceCode INT   NOT NULL DEFAULT(0)  --省份Code
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenCityCode INT   NOT NULL DEFAULT(0)  --城市Code
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD IDCard NVARCHAR(20)   NOT NULL DEFAULT('')  --申请提现身份证号
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD HandChargeThreshold decimal   NOT NULL DEFAULT(0)  --手续费阈值,例如100
GO 

ALTER TABLE dbo.ClienterWithdrawForm
ADD HandCharge decimal   NOT NULL DEFAULT(0)  --手续费,例如1元
GO 

ALTER TABLE dbo.ClienterWithdrawForm
ADD HandChargeOutlay INT   NOT NULL DEFAULT(0)  --手续费支出方:0个人,1易代送
GO 
--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'手续费阈值,例如100' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandChargeThreshold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'手续费,例如1元' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandCharge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'手续费支出方:0个人,1易代送' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandChargeOutlay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'省份' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenProvince'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'城市' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'省份Code' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenProvinceCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'城市Code' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenCityCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'申请提现身份证号' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'IDCard'
GO
    