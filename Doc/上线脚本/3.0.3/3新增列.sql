use superman
  alter table dbo.OrderOther add ReceivableType int not null  default 1 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'Ӧ������ 1Ĭ�ϱ�׼(a+b+c) 2,�����շ� Ĭ��1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'ReceivableType'
GO


