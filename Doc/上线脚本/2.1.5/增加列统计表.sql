
use superman
go
alter table dbo.Statistic
add SystemRecharge	decimal(18, 2) not null default 0
alter table dbo.Statistic
add SystemPresented	decimal(18, 2)	not null default 0
alter table dbo.Statistic
add ZhiFuBaoRecharge	decimal(18, 2)not null default 0
alter table dbo.Statistic
add WeiXinRecharge	decimal(18, 2)	not null default 0
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统充值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistic', @level2type=N'COLUMN',@level2name=N'SystemRecharge'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统赠送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistic', @level2type=N'COLUMN',@level2name=N'SystemPresented'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付宝充值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistic', @level2type=N'COLUMN',@level2name=N'ZhiFuBaoRecharge'
GO

exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信充值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Statistic', @level2type=N'COLUMN',@level2name=N'WeiXinRecharge'
go