USE superman
go

--------------------------�ױ�����������
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
          N'�ױ�����������' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
)



--------------------------��Ч�����ж�ʱ���������ɵ�ľ���(��)
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
          N'��Ч�����ж�ʱ���������ɵ�ľ���(��)' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
        )



------------------------��Ч�����ж�ʱ�ۼ���ɶ�������
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
          N'��Ч�����ж�ʱ�ۼ���ɶ�������' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
        )



UPDATE [superman].[dbo].[GlobalConfig] SET KeyName='HasUnFinishedOrderUploadTimeInterval' ,Value='10',Remark='��ʿ����δ��ɶ����ϴ�һ�ξ�γ�ȸ�������˵�ʱ����(��λΪ��)' WHERE KeyName='UploadTimeInterval' and GroupId=0;

INSERT [superman].[dbo].[GlobalConfig](KeyName,Value,LastUpdateTime,Remark,GroupId,StrategyId) VALUES('AllFinishedOrderUploadTimeInterval','60',GETDATE(),'��ʿ��û��δ��ɶ����ϴ�һ�ξ�γ�ȸ�������˵�ʱ����(��λΪ��)',0,-1);

INSERT INTO [dbo].[GlobalConfig] ([KeyName], [Value], [LastUpdateTime], [Remark], [GroupId], [StrategyId]) VALUES (N'ClienterWithdrawCommissionAccordingMoney', N'100', GETDATE(), N'��ʿ����С�ڵ���XԪ֧��������', '0', '-1');

