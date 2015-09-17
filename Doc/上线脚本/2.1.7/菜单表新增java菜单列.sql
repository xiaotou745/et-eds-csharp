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

INSERT INTO AuthorityMenuClass(ParId,MenuName,belock,IsButton,JavaUrl) VALUES(3,'余额冻结列表',0,0,'/clienter/forzenlist')
INSERT INTO AuthorityMenuClass(ParId,MenuName,belock,IsButton,JavaUrl) VALUES(88,'意见与反馈',0,0,'/feedback/list')
INSERT INTO AuthorityMenuClass(ParId,MenuName,belock,IsButton,JavaUrl) VALUES(91,'集团门店',0,0,'/groupbusiness/list')