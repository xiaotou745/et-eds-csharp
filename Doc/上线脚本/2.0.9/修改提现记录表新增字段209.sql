

USE superman
GO
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenProvince NVARCHAR(25)   NULL  --ʡ��
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenCity  NVARCHAR(25)  NULL  --����
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenProvinceCode INT   NULL  --ʡ��Code
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenCityCode INT   NULL  --����Code
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD IDCard NVARCHAR(20)   NULL  --�����������֤��
GO 


--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'ʡ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenProvince'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'ʡ��Code' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenProvinceCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����Code' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenCityCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�����������֤��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'IDCard'
GO
    