USE superman
go

ALTER TABLE dbo.[OrderOther]
ADD IsNotRealOrder INT DEFAULT 0 NOT NULL
GO

ALTER TABLE dbo.[Order]
ADD RealOrderCommission numeric(18,2) DEFAULT 0 NOT NULL
GO

EXEC sp_addextendedproperty N'MS_Description', N'�Ƿ���Ч����', 'SCHEMA', N'dbo', 'TABLE', N'OrderOther', 'COLUMN', N'IsNotRealOrder'
EXEC sp_addextendedproperty N'MS_Description', N'���ո���ʿ��Ӷ��', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'RealOrderCommission'
go

update [Order] set RealOrderCommission=OrderCommission where RealOrderCommission=0 







