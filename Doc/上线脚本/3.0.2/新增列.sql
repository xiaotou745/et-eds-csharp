use  superman
go
--------------------------------------���������������
 alter table dbo.clienter add PushShanSongOrderSet int not null  default 1;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'��̼�����ʿ���Ƶ�����  0���Ƶ� 1�Ƶ� Ĭ��1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'clienter', 
  @level2type=N'COLUMN',
  @level2name=N'PushShanSongOrderSet'  