use superman
go

INSERT INTO [superman].[dbo].[GlobalConfig] ([KeyName], [Value], [LastUpdateTime], [Remark], [GroupId], [StrategyId]) VALUES ( N'BaseCommission', N'0', GETDATE(), N'��������Ӷ��', '0', '-1');
INSERT INTO [superman].[dbo].[GlobalConfig] ([KeyName], [Value], [LastUpdateTime], [Remark], [GroupId], [StrategyId]) VALUES ( N'BaseSiteSubsidies', N'2', GETDATE(), N'��������Ӷ����վ����', '0', '-1');




-------------------�޸�������

insert into GlobalConfig(KeyName,[Value],LastUpdateTime,Remark,GroupId,StrategyId) SELECT 
 'BaseCommission' as KeyName,0 as Value,GETDATE(),'��������Ӷ��' as Remark,bg.id,-1 from BusinessGroup bg;
 
 
 
 
 

insert into GlobalConfig(KeyName,[Value],LastUpdateTime,Remark,GroupId,StrategyId) SELECT 
 'BaseSiteSubsidies' as KeyName,0 as Value,GETDATE(),'��������Ӷ����վ����' as Remark,bg.id,-1 from BusinessGroup bg;