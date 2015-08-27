use superman
 go
--订单Other 新增列 
alter table dbo.OrderOther
add IsPubDateTimely int not null default 0
go 
 EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'发单是否实时上传坐标' , @level0type=N'SCHEMA',
  @level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderOther', 
  @level2type=N'COLUMN',@level2name=N'IsPubDateTimely'
GO

alter table dbo.OrderOther
add IsGrabTimely int not null default 0
go 
 EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'抢单是否实时上传坐标' , @level0type=N'SCHEMA',
  @level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderOther', 
  @level2type=N'COLUMN',@level2name=N'IsGrabTimely'
GO

alter table dbo.OrderOther
add IsTakeTimely int not null default 0
go 
 EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'取货是否实时上传坐标' , @level0type=N'SCHEMA',
  @level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderOther', 
  @level2type=N'COLUMN',@level2name=N'IsTakeTimely'
GO

alter table dbo.OrderOther
add IsCompleteTimely int not null default 0
go 
 EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'完成订单是否实时上传坐标' , @level0type=N'SCHEMA',
  @level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderOther', 
  @level2type=N'COLUMN',@level2name=N'IsCompleteTimely'
GO


 