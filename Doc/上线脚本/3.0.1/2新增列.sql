--------------------------------------���������������
 alter table [Order] add IsConsiderDeliveryFee int not null  default 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����ʱ�Ƿ������ͷ�0������1����Ĭ��0' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'IsConsiderDeliveryFee'
  
   alter table [Order] add TipAmount decimal(18, 2) not null  default 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'С�ѽ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'TipAmount'

 alter table [Order] add PubName NVARCHAR(90) not null  default '' ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'������' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'PubName'
  
 alter table [Order] add PubPhoneNo NVARCHAR(90) not null  default '' ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�����˵�ַ' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'PubPhoneNo'

alter table [Order] add [TakeType] smallint not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'ȡ��״̬Ĭ��0������1ԤԼ' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'TakeType'

alter table [Order] add [ProductName] NVARCHAR(200)  not null  default '' ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'��Ʒ����' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'ProductName'

alter table [Order] add [PickUpLongitude] float  not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'ȡ���ص㾭��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'PickUpLongitude'

alter table [Order] add [PickUpLatitude] float  not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'ȡ���ص�γ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'PickUpLatitude'

UPDATE  [order]
SET     km = 0
WHERE   km IS NULL;

ALTER TABLE [order] ADD CONSTRAINT c_order_KM   DEFAULT 0 FOR KM;

ALTER TABLE [order]
ALTER COLUMN KM  FLOAT NOT NULL 


 
------------------------------------------------����Other�����
 alter table [OrderOther] add DeliveryOrderNo bigint not null  default 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�ͻ�����' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'DeliveryOrderNo'
  
   alter table [OrderOther] add NotifyTime datetime ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'֪ͨʱ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'NotifyTime'
  
  alter table [OrderOther] add EndTime datetime ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����ʱ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'EndTime'
  
 alter table [OrderOther] add ExpectedTakeTime datetime ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'����ȡ��ʱ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'ExpectedTakeTime'

 alter table [OrderOther] add ExpectedDelivery datetime ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�����ʹ�ʱ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'ExpectedDelivery'
  
  alter table [OrderOther] add ReceiptId nvarchar(50) not null  default '';
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'СƱID' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'ReceiptId'
------------------------------------------------�̻������
  alter table dbo.business
 add IsEnable int not null  default 1
 go 
  exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ĭ��1����;0����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'business', @level2type=N'COLUMN',@level2name=N'IsEnable'
 go


  alter table dbo.business
 add RegisterFrom int not null  default 1
 go 
  exec sys.sp_addextendedproperty @name=N'MS_Description', 
  @value=N'ע����Դ,Ĭ��1ԭע���̻�;2�����̻�,' , @level0type=N'SCHEMA',@level0name=N'dbo', 
  @level1type=N'TABLE',@level1name=N'business', @level2type=N'COLUMN',@level2name=N'RegisterFrom'
 go


  alter table dbo.business
 add OriginalBusiUnitId nvarchar(255) not null  default  ''
 go 
  exec sys.sp_addextendedproperty @name=N'MS_Description', 
  @value=N'�Ե���̻�Id' , @level0type=N'SCHEMA',@level0name=N'dbo', 
  @level1type=N'TABLE',@level1name=N'business', @level2type=N'COLUMN',@level2name=N'OriginalBusiUnitId'
 go
--------------------------------------------��ʿ���
alter table dbo.clienter
add vehicleName nvarchar(100) null
exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ͨ��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'clienter', @level2type=N'COLUMN',@level2name=N'vehicleName'

----------------------------------------------�˵������AuthCode
USE superman
alter table dbo.AuthorityMenuClass add AuthCode NVARCHAR(500) NOT NULL default('') 
--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'Ȩ�ޱ�ʶ' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AuthorityMenuClass', 
  @level2type=N'COLUMN',
  @level2name=N'AuthCode'
GO

