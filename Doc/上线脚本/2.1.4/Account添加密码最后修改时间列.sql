USE superman
go
--�û��� �����ϴ������޸�ʱ��
ALTER TABLE dbo.account ADD LastChangeTime DATETIME NOT NULL DEFAULT ('2015-07-01 00:00:00')
go 

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'��������޸�ʱ��' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'account', 
  @level2type=N'COLUMN',
  @level2name=N'LastChangeTime'
GO