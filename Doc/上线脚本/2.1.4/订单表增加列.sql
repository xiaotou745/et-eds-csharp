use superman
 go
--订单表 新增列 
alter table dbo.[order]
add IsComplain int not null default 0
go 
 EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'是否被投诉过1已投诉0未投诉，默认0' , @level0type=N'SCHEMA',
  @level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', 
  @level2type=N'COLUMN',@level2name=N'IsComplain'
GO

--订单表Other 新增列 
 go
  alter table dbo.OrderOther
  add IsAllowCashPay int not null default 0;

 go 
  exec sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否允许现金支付1允许0不允许，默认0', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderOther',
    @level2type = N'COLUMN', @level2name = N'IsAllowCashPay';
go

--2015年8月20日 11:35:15 
  alter table dbo.OrderOther  add CancelTime datetime ;
go
  exec sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'取消时间', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderOther',
    @level2type = N'COLUMN', @level2name = N'CancelTime';
    go