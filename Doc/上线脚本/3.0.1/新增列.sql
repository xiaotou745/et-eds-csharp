--------------------------------------新增订单表相关列
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
  

