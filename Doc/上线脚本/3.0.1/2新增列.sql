--------------------------------------新增订单表相关列
 alter table [Order] add IsConsiderDeliveryFee int not null  default 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'结算时是否考虑外送费0不考虑1考虑默认0' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'IsConsiderDeliveryFee'
  
   alter table [Order] add TipAmount decimal(18, 2) not null  default 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'小费金额' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'TipAmount'

 alter table [Order] add PubName NVARCHAR(90) not null  default '' ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'发货人' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'PubName'
  
 alter table [Order] add PubPhoneNo NVARCHAR(90) not null  default '' ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'发货人地址' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'PubPhoneNo'

alter table [Order] add [TakeType] smallint not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'取货状态默认0立即，1预约' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'TakeType'

alter table [Order] add [ProductName] NVARCHAR(200)  not null  default '' ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'物品名称' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'ProductName'

alter table [Order] add [PickUpLongitude] float  not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'取货地点经度' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'PickUpLongitude'

alter table [Order] add [PickUpLatitude] float  not null  default 0 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'取货地点纬度' ,
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


 
------------------------------------------------订单Other表相关
 alter table [OrderOther] add DeliveryOrderNo bigint not null  default 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'送货单号' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'DeliveryOrderNo'
  
   alter table [OrderOther] add NotifyTime datetime ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'通知时间' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'NotifyTime'
  
  alter table [OrderOther] add EndTime datetime ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'结束时间' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'EndTime'
  
 alter table [OrderOther] add ExpectedTakeTime datetime ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'期望取件时间' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'ExpectedTakeTime'

 alter table [OrderOther] add ExpectedDelivery datetime ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'期望送达时间' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'ExpectedDelivery'
  
  alter table [OrderOther] add ReceiptId nvarchar(50) not null  default '';
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'小票ID' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'ReceiptId'
------------------------------------------------商户表相关
  alter table dbo.business
 add IsEnable int not null  default 1
 go 
  exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认1启用;0禁用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'business', @level2type=N'COLUMN',@level2name=N'IsEnable'
 go


  alter table dbo.business
 add RegisterFrom int not null  default 1
 go 
  exec sys.sp_addextendedproperty @name=N'MS_Description', 
  @value=N'注册来源,默认1原注册商户;2闪送商户,' , @level0type=N'SCHEMA',@level0name=N'dbo', 
  @level1type=N'TABLE',@level1name=N'business', @level2type=N'COLUMN',@level2name=N'RegisterFrom'
 go


  alter table dbo.business
 add OriginalBusiUnitId nvarchar(255) not null  default  ''
 go 
  exec sys.sp_addextendedproperty @name=N'MS_Description', 
  @value=N'淘点点商户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', 
  @level1type=N'TABLE',@level1name=N'business', @level2type=N'COLUMN',@level2name=N'OriginalBusiUnitId'
 go
--------------------------------------------骑士相关
alter table dbo.clienter
add vehicleName nvarchar(100) null
exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交通工具名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'clienter', @level2type=N'COLUMN',@level2name=N'vehicleName'

----------------------------------------------菜单表添加AuthCode
USE superman
alter table dbo.AuthorityMenuClass add AuthCode NVARCHAR(500) NOT NULL default('') 
--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'权限标识' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AuthorityMenuClass', 
  @level2type=N'COLUMN',
  @level2name=N'AuthCode'
GO

