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

INSERT INTO AuthorityMenuClass(ParId,MenuName,belock,IsButton,JavaUrl) VALUES(3,'�����б�',0,0,'/clienter/forzenlist')
INSERT INTO AuthorityMenuClass(ParId,MenuName,belock,IsButton,JavaUrl) VALUES(88,'����뷴��',0,0,'/feedback/list')
INSERT INTO AuthorityMenuClass(ParId,MenuName,belock,IsButton,JavaUrl) VALUES(91,'�����ŵ�',0,0,'/groupbusiness/list')