ALTER TABLE dbo.ClienterWithdrawForm ADD PaidAmount DECIMAL(18,3) DEFAULT 0 NOT NULL
GO 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʵ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClienterWithdrawForm', @level2type=N'COLUMN',@level2name=N'PaidAmount'
GO

INSERT dbo.GlobalConfig
        ( KeyName ,
          Value ,
          LastUpdateTime ,
          Remark ,
          GroupId ,
          StrategyId
        )
VALUES  ( N'YeepayWithdrawCommission' , -- KeyName - nvarchar(100)
          N'1' , -- Value - nvarchar(500)
          GETDATE() , -- LastUpdateTime - datetime
          N'�ױ�ʵ��������' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
        );
INSERT dbo.GlobalConfig
        ( KeyName ,
          Value ,
          LastUpdateTime ,
          Remark ,
          GroupId ,
          StrategyId
        )
VALUES  ( N'AlipayWithdrawCommission' , -- KeyName - nvarchar(100)
          N'0.5' , -- Value - nvarchar(500)
          GETDATE() , -- LastUpdateTime - datetime
          N'֧����ʵ��������' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
        );

INSERT dbo.GlobalConfig
        ( KeyName ,
          Value ,
          LastUpdateTime ,
          Remark ,
          GroupId ,
          StrategyId
        )
VALUES  ( N'AlipayPassword' , -- KeyName - nvarchar(100)
          N'' , -- Value - nvarchar(500)
          GETDATE() , -- LastUpdateTime - datetime
          N'֧����ת������' , -- Remark - nvarchar(200)
          0 , -- GroupId - int
          -1  -- StrategyId - int
        );

