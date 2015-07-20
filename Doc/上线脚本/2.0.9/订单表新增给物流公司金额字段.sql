use superman
go
ALTER TABLE dbo.[order]
ADD DeliveryCompanySettleMoney numeric(18,2) DEFAULT 0 NOT NULL
GO 
ALTER TABLE dbo.[order]
ADD DeliveryCompanyID int DEFAULT 0 NOT NULL
GO 
--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'������˾������' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'DeliveryCompanySettleMoney'
GO
--�����ע��
EXEC sys.sp_addextendedproperty @name=N'MS_Description',
 @value=N'���͵�������˾id' ,
  @level0type=N'SCHEMA',
  @level0name=N'dbo', 
  @level1type=N'TABLE',
  @level1name=N'Order', 
  @level2type=N'COLUMN',
  @level2name=N'DeliveryCompanyID'
GO

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
        go
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
go
create index index_order_actualDoneDate on [order](ActualDoneDate) 
go


