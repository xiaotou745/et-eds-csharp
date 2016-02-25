use superman
  alter table dbo.OrderOther add ReceivableType int not null  default 1 ;
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'应收类型 1默认标准(a+b+c) 2,阶梯收费 默认1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'ReceivableType'
GO


	    USE superman
	    alter table dbo.business drop constraint DF__business__TaskDi__15B0212B
		--说明：删除表的字段的原有约束
		alter table dbo.business add constraint DF_BUSINESS_TaskDistributionId DEFAULT 1 for TaskDistributionId


