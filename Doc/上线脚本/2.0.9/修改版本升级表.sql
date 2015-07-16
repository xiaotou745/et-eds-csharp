USE superman
GO
ALTER TABLE dbo.AppVersion
ADD CreateBy nvarchar(50) NULL--添加创建人
GO
ALTER TABLE dbo.AppVersion
ADD UpdateDate datetime NOT NULL DEFAULT(GETDATE()) --添加更新时间
GO 
ALTER TABLE dbo.AppVersion
ADD UpdateBy nvarchar(50) NULL --添加更新人
GO 
ALTER TABLE dbo.AppVersion
ADD IsTiming bit NOT NULL DEFAULT(0) --是否定时 0否 1 是
GO 
ALTER TABLE dbo.AppVersion
ADD TimingDate datetime NULL --定时发布时间
GO 
ALTER TABLE dbo.AppVersion
ADD PubStatus INT NOT  NULL DEFAULT(0) --发布状态
GO 


--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'发布状态 0待发布 1 已发布 2 取消发布' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'PubStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'创建人' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'更新时间' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'UpdateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'更新人' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'是否定时 0否 1 是' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'IsTiming'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'定时发布时间' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AppVersion', 
  @level2type=N'COLUMN',
  @level2name=N'TimingDate'
GO

