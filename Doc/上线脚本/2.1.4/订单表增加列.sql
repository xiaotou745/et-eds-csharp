use superman
 go
--������ ������ 
alter table dbo.[order]
add IsComplain int not null default 0
go 
 EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�Ƿ�Ͷ�߹�1��Ͷ��0δͶ�ߣ�Ĭ��0' , @level0type=N'SCHEMA',
  @level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', 
  @level2type=N'COLUMN',@level2name=N'IsComplain'
GO

--������Other ������ 
 go
  alter table dbo.OrderOther
  add IsAllowCashPay int not null default 0;

 go 
  exec sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'�Ƿ������ֽ�֧��1����0������Ĭ��0', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderOther',
    @level2type = N'COLUMN', @level2name = N'IsAllowCashPay';
go

--2015��8��20�� 11:35:15 
  alter table dbo.OrderOther  add CancelTime datetime ;
go
  exec sys.sp_addextendedproperty @name = N'MS_Description',
    @value = N'ȡ��ʱ��', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderOther',
    @level2type = N'COLUMN', @level2name = N'CancelTime';
    go