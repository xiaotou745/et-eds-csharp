USE [superman]
GO

  UPDATE [superman].[dbo].[GlobalConfig] SET KeyName='HasUnFinishedOrderUploadTimeInterval' ,Value='10',Remark='骑士端有未完成订单上传一次经纬度给到服务端的时间间隔(单位为秒)' WHERE KeyName='UploadTimeInterval' and GroupId=0;

  INSERT [superman].[dbo].[GlobalConfig](KeyName,Value,LastUpdateTime,Remark,GroupId,StrategyId) VALUES('AllFinishedOrderUploadTimeInterval','60',GETDATE(),'骑士端没有未完成订单上传一次经纬度给到服务端的时间间隔(单位为秒)',0,-1);


INSERT INTO [dbo].[GlobalConfig] ([KeyName], [Value], [LastUpdateTime], [Remark], [GroupId], [StrategyId]) VALUES (N'ClienterWithdrawCommissionAccordingMoney', N'100', GETDATE(), N'骑士提现小于等于X元支付手续费', '0', '-1');
