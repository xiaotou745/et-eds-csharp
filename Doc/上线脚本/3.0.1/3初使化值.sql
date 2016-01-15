--任务配送费配置
SET IDENTITY_INSERT [dbo].[TaskDistributionConfig] ON
INSERT [dbo].[TaskDistributionConfig] ([Id], [KM], [KG], [DistributionPrice], [IsMaster]) VALUES (1, 5, 5, CAST(8.00 AS Numeric(18, 2)), 1)
INSERT [dbo].[TaskDistributionConfig] ([Id], [KM], [KG], [DistributionPrice], [IsMaster]) VALUES (2, 1, 0, CAST(5.00 AS Numeric(18, 2)), 0)
INSERT [dbo].[TaskDistributionConfig] ([Id], [KM], [KG], [DistributionPrice], [IsMaster]) VALUES (3, 0, 1, CAST(8.00 AS Numeric(18, 2)), 0)
SET IDENTITY_INSERT [dbo].[TaskDistributionConfig] OFF

go

--公共配置表
insert into GlobalConfig(KeyName,Value,Remark,GroupId,StrategyId) values('CashAndTime',72,'闪送模式加可提现时间(单位：小时)',0,-1)
insert into GlobalConfig(KeyName,Value,Remark,GroupId,StrategyId) values('SSCancelOrder',24,'闪送模式待支付取消订单(单位：小时)',0,-1)
go

--小费基础配置表
insert into OrderTip(Amount) values(1)
insert into OrderTip(Amount) values(2)
insert into OrderTip(Amount) values(5)
insert into OrderTip(Amount) values(8)
insert into OrderTip(Amount) values(10)
go
