ALTER TABLE [dbo].OrderChild ADD prepay_id varchar(200) not null default '' ;--微信支付生订微信订单号
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信支付生订微信订单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderChild', @level2type=N'COLUMN',@level2name=N'prepay_id'






ALTER TABLE [dbo].BusinessRecharge ADD prepay_id varchar(200) not null default '' ;--微信支付生订微信订单号
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信支付生订微信订单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BusinessRecharge', @level2type=N'COLUMN',@level2name=N'prepay_id'
