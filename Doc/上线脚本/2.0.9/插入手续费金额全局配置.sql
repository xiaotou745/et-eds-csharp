USE superman
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
        
SELECT * FROM GlobalConfig ORDER BY id DESC