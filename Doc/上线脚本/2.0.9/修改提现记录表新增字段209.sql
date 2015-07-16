

USE superman
GO
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenProvince NVARCHAR(25)   NULL  --省份
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenCity  NVARCHAR(25)  NULL  --城市
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenProvinceCode INT   NULL  --省份Code
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenCityCode INT   NULL  --城市Code
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD IDCard NVARCHAR(20)   NULL  --申请提现身份证号
GO 


--添加列注释
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
    