--���������������̼�ר����ʿ�ӵ���Ӧʱ��
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
          N'�̼�ר����ʿ�ӵ���Ӧʱ��' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
        );