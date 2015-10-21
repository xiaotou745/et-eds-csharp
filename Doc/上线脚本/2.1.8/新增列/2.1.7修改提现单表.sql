USE superman
alter table ClienterWithdrawForm add AlipayBatchNo NVARCHAR(50) default('')
alter table ClienterWithdrawForm add HandChargeShot DECIMAL default(0)




--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'系统手续费配置快照' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'HandChargeShot'
GO
--添加列注释
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'支付宝批次号' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'ClienterWithdrawForm', 
  @level2type=N'COLUMN',
  @level2name=N'AlipayBatchNo'
GO