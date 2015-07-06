ALTER TABLE Clienter
ADD DeliveryCompanyId int NOT NULL DEFAULT 0
GO 
--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'所属物流公司ID' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Clienter', 
  @level2type=N'COLUMN',
  @level2name=N'DeliveryCompanyId'
GO

--上线时 更新所有的旧数据为0
UPDATE Clienter SET DeliveryCompanyId=0
