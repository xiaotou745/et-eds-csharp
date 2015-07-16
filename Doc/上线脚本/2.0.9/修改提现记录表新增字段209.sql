

USE superman
GO
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
    