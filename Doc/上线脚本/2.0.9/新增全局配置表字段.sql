USE [superman]
GO

  UPDATE [superman].[dbo].[GlobalConfig] SET KeyName='HasUnFinishedOrderUploadTimeInterval' ,Value='10',Remark='��ʿ����δ��ɶ����ϴ�һ�ξ�γ�ȸ�������˵�ʱ����(��λΪ��)' WHERE KeyName='UploadTimeInterval' and GroupId=0;

  INSERT [superman].[dbo].[GlobalConfig](KeyName,Value,LastUpdateTime,Remark,GroupId,StrategyId) VALUES('AllFinishedOrderUploadTimeInterval','60',GETDATE(),'��ʿ��û��δ��ɶ����ϴ�һ�ξ�γ�ȸ�������˵�ʱ����(��λΪ��)',0,-1);


INSERT INTO [dbo].[GlobalConfig] ([KeyName], [Value], [LastUpdateTime], [Remark], [GroupId], [StrategyId]) VALUES (N'ClienterWithdrawCommissionAccordingMoney', N'100', GETDATE(), N'��ʿ����С�ڵ���XԪ֧��������', '0', '-1');
