use  superman
go
--------------------------------------订单表
 alter table dbo.[order] add ReceiveCode varchar(16) not null  DEFAULT '';
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'收货码' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'order', 
  @level2type=N'COLUMN',
  @level2name=N'ReceiveCode'
  go  

  
   alter table dbo.[order] add PubCity nvarchar(45) not null  DEFAULT '';
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'发货城市' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'order', 
  @level2type=N'COLUMN',
  @level2name=N'PubCity'
  go 
  
   alter table dbo.[order] add IsReceiveCode int not null  DEFAULT 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'是否收取验证码' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'order', 
  @level2type=N'COLUMN',
  @level2name=N'IsReceiveCode'
  go 
  
  
--------------------------------------OrderOther
alter table dbo.OrderOther
add IsOrderRemind int default 0
go
alter table dbo.OrderOther
add OrderRemindTime datetime null
go
exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否催单默认0没有1有' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderOther', @level2type=N'COLUMN',@level2name=N'IsOrderRemind'
go
exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'淘店点催单时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderOther', @level2type=N'COLUMN',@level2name=N'OrderRemindTime'
go
update dbo.OrderOther
set IsOrderRemind=0;
go

--------------------------------------新增订单表相关列
 alter table dbo.clienter add PushShanSongOrderSet int not null  default 1;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'里程计算骑士端推单设置  0不推单 1推单 默认1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'clienter', 
  @level2type=N'COLUMN',
  @level2name=N'PushShanSongOrderSet'  

---------------------------------任务配送费
 alter table TaskDistributionConfig add Steps int not null  default 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'计价阶梯' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'TaskDistributionConfig', 
  @level2type=N'COLUMN',
  @level2name=N'Steps'
go 
  
   alter table TaskDistributionConfig add   Remark NVARCHAR(1000) not null  default '';
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'备注' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'TaskDistributionConfig', 
  @level2type=N'COLUMN',
  @level2name=N'Remark'
go