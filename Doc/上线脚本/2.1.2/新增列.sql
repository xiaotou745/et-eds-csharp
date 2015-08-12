USE superman
go
----------------------------------------商户表 增加 该商户的订单是否需要审核
ALTER TABLE dbo.business ADD IsOrderChecked int NOT NULL DEFAULT (1)
go 

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'订单是否需要审核 0不需要 1 需要 默认1' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'business', 
  @level2type=N'COLUMN',
  @level2name=N'IsOrderChecked'
GO



----------------------------------------订单表增加该订单是否需要审核
ALTER TABLE dbo.OrderOther ADD IsOrderChecked int NOT NULL DEFAULT (1)
go 

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
  @value=N'订单是否需要审核(快照) 0不需要 1 需要' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'OrderOther', 
  @level2type=N'COLUMN',
  @level2name=N'IsOrderChecked'
GO


----------------------------------------基本补贴佣金
ALTER TABLE [dbo].[order] ADD [BaseCommission] decimal(18,2) not null default 0;--基本补贴佣金
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'基本补贴佣金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'BaseCommission'
GO

