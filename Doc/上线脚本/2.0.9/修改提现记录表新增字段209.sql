

USE superman
GO
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenProvince NVARCHAR(25)   NOT NULL DEFAULT('')  --ʡ��
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenCity  NVARCHAR(25)  NOT NULL DEFAULT('')  --����
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenProvinceCode INT   NOT NULL DEFAULT(0)  --ʡ��Code
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD OpenCityCode INT   NOT NULL DEFAULT(0)  --����Code
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD IDCard NVARCHAR(20)   NOT NULL DEFAULT('')  --�����������֤��
GO 
ALTER TABLE dbo.ClienterWithdrawForm
ADD HandChargeThreshold decimal   NOT NULL DEFAULT(0)  --��������ֵ,����100
GO 

ALTER TABLE dbo.ClienterWithdrawForm
ADD HandCharge decimal   NOT NULL DEFAULT(0)  --������,����1Ԫ
GO 

ALTER TABLE dbo.ClienterWithdrawForm
ADD HandChargeOutlay INT   NOT NULL DEFAULT(0)  --������֧����:0����,1�״���
GO 
--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'��������ֵ,����100' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandChargeThreshold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'������,����1Ԫ' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandCharge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'������֧����:0����,1�״���' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandChargeOutlay'
GO
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
    