USE superman
alter table ClienterWithdrawForm add AlipayBatchNo NVARCHAR(50) default('')
alter table ClienterWithdrawForm add HandChargeShot DECIMAL default(0)




--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'ϵͳ���������ÿ���' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandChargeShot'
GO
--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'֧�������κ�' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'AlipayBatchNo'
GO