USE superman
go
----------------------------------------�̻��� ���� ���̻��Ķ����Ƿ���Ҫ���
ALTER TABLE dbo.business ADD IsOrderChecked int NOT NULL DEFAULT (1)
go 

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�����Ƿ���Ҫ��� 0����Ҫ 1 ��Ҫ Ĭ��1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'business', 
  @level2type=N'COLUMN',
  @level2name=N'IsOrderChecked'
GO



----------------------------------------���������Ӹö����Ƿ���Ҫ���
ALTER TABLE dbo.OrderOther ADD IsOrderChecked int NOT NULL DEFAULT (1)
go 

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'�����Ƿ���Ҫ���(����) 0����Ҫ 1 ��Ҫ' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'IsOrderChecked'
GO


----------------------------------------��������Ӷ��
ALTER TABLE [dbo].[order] ADD [BaseCommission] decimal(18,2) not null default 0;--��������Ӷ��
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������Ӷ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'BaseCommission'
GO

