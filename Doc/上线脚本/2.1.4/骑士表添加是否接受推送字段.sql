USE superman
go
--用户表 增加上次密码修改时间
ALTER TABLE dbo.clienter ADD IsReceivePush INT NOT NULL DEFAULT (1)
go 

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'骑士是否接受推送 1 接口 0 不接受 默认1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'clienter', 
  @level2type=N'COLUMN',
  @level2name=N'IsReceivePush'
GO
