use superman
go

ALTER TABLE dbo.[AuthorityMenuClass]
ADD JavaUrl VARCHAR(500)
GO 

--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'java�е�url' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'AuthorityMenuClass', 
  @level2type=N'COLUMN',
  @level2name=N'JavaUrl'
GO
