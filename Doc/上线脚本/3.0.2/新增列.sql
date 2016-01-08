use  superman
go
--------------------------------------新增订单表相关列
 alter table dbo.clienter add PushShanSongOrderSet int not null  default 0;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'里程计算骑士端推单设置  0不推单 1推单 默认0' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'clienter', 
  @level2type=N'COLUMN',
  @level2name=N'PushShanSongOrderSet'  