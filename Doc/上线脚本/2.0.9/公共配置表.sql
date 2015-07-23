USE superman
go

--------------------------易宝提现手续费
INSERT INTO dbo.GlobalConfig 
        ( KeyName ,
          Value ,
          LastUpdateTime ,
          Remark ,
          GroupId ,
          StrategyId
        )
VALUES  ( N'WithdrawCommission' , -- KeyName - nvarchar(100)
          N'1' , -- Value - nvarchar(500)
          '2015-07-16 06:46:59' , -- LastUpdateTime - datetime
          N'易宝提现手续费' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
)



--------------------------无效订单判定时抢单点和完成点的距离(米)
INSERT INTO dbo.GlobalConfig
        ( KeyName ,
          Value ,
          LastUpdateTime ,
          Remark ,
          GroupId ,
          StrategyId
        )
VALUES  ( N'GrabToCompleteDistance' , -- KeyName - nvarchar(100)
          N'0' , -- Value - nvarchar(500)
          GETDATE(),
          N'无效订单判定时抢单点和完成点的距离(米)' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
        )



------------------------无效订单判定时累计完成订单数量
INSERT INTO dbo.GlobalConfig
        ( KeyName ,
          Value ,
          LastUpdateTime ,
          Remark ,
          GroupId ,
          StrategyId
        )
VALUES  ( N'OrderCountSetting' , -- KeyName - nvarchar(100)
          N'50' , -- Value - nvarchar(500)
          GETDATE(),
          N'无效订单判定时累计完成订单数量' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
        )



UPDATE [superman].[dbo].[GlobalConfig] SET KeyName='HasUnFinishedOrderUploadTimeInterval' ,Value='10',Remark='骑士端有未完成订单上传一次经纬度给到服务端的时间间隔(单位为秒)' WHERE KeyName='UploadTimeInterval' and GroupId=0;

INSERT [superman].[dbo].[GlobalConfig](KeyName,Value,LastUpdateTime,Remark,GroupId,StrategyId) VALUES('AllFinishedOrderUploadTimeInterval','60',GETDATE(),'骑士端没有未完成订单上传一次经纬度给到服务端的时间间隔(单位为秒)',0,-1);

INSERT INTO [dbo].[GlobalConfig] ([KeyName], [Value], [LastUpdateTime], [Remark], [GroupId], [StrategyId]) VALUES (N'ClienterWithdrawCommissionAccordingMoney', N'100', GETDATE(), N'骑士提现小于等于X元支付手续费', '0', '-1');

