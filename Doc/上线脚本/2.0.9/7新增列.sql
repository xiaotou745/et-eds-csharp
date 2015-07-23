use superman
go

----------------------------------------------�������������˾������+���͵�������˾id

ALTER TABLE dbo.[order]
ADD DeliveryCompanySettleMoney numeric(18,2) DEFAULT 0 NOT NULL
GO 
ALTER TABLE dbo.[order]
ADD DeliveryCompanyID int DEFAULT 0 NOT NULL
GO 
--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'������˾������' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'DeliveryCompanySettleMoney'
GO
--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'���͵�������˾id' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'DeliveryCompanyID'
GO

-----------------------------------------��ʿ������ע���õ���ʱ���
ALTER TABLE [dbo].[clienter] ADD [Timespan] nvarchar(50) COLLATE Chinese_PRC_CI_AS NULL ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʱ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'clienter', @level2type=N'COLUMN',@level2name=N'Timespan'
GO


-----------------------------------------�̻�������ע���õ���ʱ���
ALTER TABLE [dbo].[business] ADD [Timespan] nvarchar(50) COLLATE Chinese_PRC_CI_AS NULL ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʱ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'business', @level2type=N'COLUMN',@level2name=N'Timespan'
GO

----------------------------------------��ʿ���п���
ALTER TABLE [dbo].[ClienterFinanceAccount] ADD [IDCard] varchar(90) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����˻�ʱ�����֤�ţ���˾�˻�ʱ����Ӫҵִ�պ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'IDCard'
GO
ALTER TABLE [dbo].[ClienterFinanceAccount] ADD [OpenProvince] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʡ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenProvince'
GO
ALTER TABLE [dbo].[ClienterFinanceAccount] ADD [OpenCity] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenCity'
GO
ALTER TABLE [dbo].[ClienterFinanceAccount] ADD [YeepayKey] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ױ�key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'YeepayKey'
GO
ALTER TABLE [dbo].[ClienterFinanceAccount] ADD [YeepayStatus] smallint NULL DEFAULT ((1)) ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ױ��˻�״̬  0���� 1ʧ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'YeepayStatus'
GO
alter table dbo.ClienterFinanceAccount
add OpenProvinceCode int null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʡCode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenProvinceCode'
GO
alter table dbo.ClienterFinanceAccount
add OpenCityCode int null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenCityCode'
GO


----------------------------------------�̻����п���
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [IDCard] varchar(90) NULL DEFAULT '' ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����˻�ʱ�����֤�ţ���˾�˻�ʱ����Ӫҵִ�պ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'IDCard'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [OpenProvince] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʡ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenProvince'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [OpenCity] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenCity'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [YeepayKey] varchar(45) NULL DEFAULT '' ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ױ�key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'YeepayKey'
GO
ALTER TABLE [dbo].[BusinessFinanceAccount] ADD [YeepayStatus] smallint NULL DEFAULT ((1)) ;
  EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ױ��˻�״̬  0���� 1ʧ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'YeepayStatus'
GO
alter table dbo.BusinessFinanceAccount
add OpenProvinceCode int null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʡCode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenProvinceCode'
GO
alter table dbo.BusinessFinanceAccount
add OpenCityCode int null
go
exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessFinanceAccount', @level2type=N'COLUMN',@level2name=N'OpenCityCode'
GO 


---------------------------------------------------�޸��̻����ּ�¼�������ֶ�
ALTER TABLE dbo.BusinessWithdrawForm
ADD OpenProvince NVARCHAR(25)   NOT NULL DEFAULT('')  --ʡ��
GO 
ALTER TABLE dbo.BusinessWithdrawForm
ADD OpenCity  NVARCHAR(25)  NOT NULL DEFAULT('')  --����
GO 
ALTER TABLE dbo.BusinessWithdrawForm
ADD OpenProvinceCode INT   NOT NULL DEFAULT(0)  --ʡ��Code
GO 
ALTER TABLE dbo.BusinessWithdrawForm
ADD OpenCityCode INT   NOT NULL DEFAULT(0)  --����Code
GO 
ALTER TABLE dbo.BusinessWithdrawForm
ADD IDCard NVARCHAR(20)   NOT NULL DEFAULT('')  --�����������֤��
GO 
ALTER TABLE dbo.BusinessWithdrawForm
ADD HandChargeThreshold decimal   NOT NULL DEFAULT(0)  --��������ֵ,����100
GO 

ALTER TABLE dbo.BusinessWithdrawForm
ADD HandCharge decimal   NOT NULL DEFAULT(0)  --������,����1Ԫ
GO 

ALTER TABLE dbo.BusinessWithdrawForm
ADD HandChargeOutlay INT   NOT NULL DEFAULT(0)  --������֧����:0����,1�״���
GO 
--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'��������ֵ,����100' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandChargeThreshold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'������,����1Ԫ' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandCharge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'������֧����:0����,1�״���' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandChargeOutlay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'ʡ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenProvince'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'ʡ��Code' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenProvinceCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����Code' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'OpenCityCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�����������֤��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'BusinessWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'IDCard'
GO
    




--------------------------------------------------�޸����ּ�¼�������ֶ�209
    
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
    