USE superman
go
--用户表 增加上次密码修改时间
ALTER TABLE dbo.account ADD LastChangeTime DATETIME NOT NULL DEFAULT ('2015-07-01 00:00:00')
go 

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'订单是否需要审核 0不需要 1 需要 默认1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'account', 
  @level2type=N'COLUMN',
  @level2name=N'LastChangeTime'
GO