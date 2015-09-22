use superman
go

ALTER TABLE dbo.[AuthorityMenuClass]
ADD JavaUrl VARCHAR(500)
GO 

--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'java中的url' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AuthorityMenuClass', 
  @level2type=N'COLUMN',
  @level2name=N'JavaUrl'
GO
