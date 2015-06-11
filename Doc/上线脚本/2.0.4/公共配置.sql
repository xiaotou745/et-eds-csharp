--公共配置里新增商家专属骑士接单响应时间
use superman
INSERT dbo.GlobalConfig
        ( KeyName ,
          Value ,
          LastUpdateTime ,
          Remark ,
          GroupId ,
          StrategyId
        )
VALUES  ( 'ExclusiveOrderTime', -- KeyName - nvarchar(100)
          N'5' , -- Value - nvarchar(500)
          GETDATE() , -- LastUpdateTime - datetime
          N'商家专属骑士接单响应时间' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
        );