use superman
go

INSERT INTO [superman].[dbo].[GlobalConfig] ([KeyName], [Value], [LastUpdateTime], [Remark], [GroupId], [StrategyId]) VALUES ( N'BaseCommission', N'0', GETDATE(), N'基本补贴佣金', '0', '-1');
INSERT INTO [superman].[dbo].[GlobalConfig] ([KeyName], [Value], [LastUpdateTime], [Remark], [GroupId], [StrategyId]) VALUES ( N'BaseSiteSubsidies', N'2', GETDATE(), N'基本补贴佣金网站补贴', '0', '-1');




-------------------修复老数据

insert into GlobalConfig(KeyName,[Value],LastUpdateTime,Remark,GroupId,StrategyId) SELECT 
 'BaseCommission' as KeyName,0 as Value,GETDATE(),'基本补贴佣金' as Remark,bg.id,-1 from BusinessGroup bg;
 
 
 
 
 

insert into GlobalConfig(KeyName,[Value],LastUpdateTime,Remark,GroupId,StrategyId) SELECT 
 'BaseSiteSubsidies' as KeyName,0 as Value,GETDATE(),'基本补贴佣金网站补贴' as Remark,bg.id,-1 from BusinessGroup bg;