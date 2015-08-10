use superman
go

INSERT INTO [superman].[dbo].[GlobalConfig] ([KeyName], [Value], [LastUpdateTime], [Remark], [GroupId], [StrategyId]) VALUES ( N'BaseCommission', N'0', GETDATE(), N'基本补贴佣金', '0', '-1');
INSERT INTO [superman].[dbo].[GlobalConfig] ([KeyName], [Value], [LastUpdateTime], [Remark], [GroupId], [StrategyId]) VALUES ( N'BaseSiteSubsidies', N'2', GETDATE(), N'基本补贴佣金网站补贴', '0', '-1');