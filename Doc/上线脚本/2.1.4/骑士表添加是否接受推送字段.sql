USE superman
go
--�û��� �����ϴ������޸�ʱ��
ALTER TABLE dbo.clienter ADD IsReceivePush INT NOT NULL DEFAULT (1)
go 

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'��ʿ�Ƿ�������� 1 �ӿ� 0 ������ Ĭ��1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'clienter', 
  @level2type=N'COLUMN',
  @level2name=N'IsReceivePush'
GO
