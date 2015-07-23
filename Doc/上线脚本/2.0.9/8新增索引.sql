USE [superman]
GO

--订单表的骑士ID
CREATE INDEX [IX_order_clienterId] ON [dbo].[order]([clienterId] ASC) ;
go


--订单表的完成时间
create index IX_order_actualDoneDate on [order](ActualDoneDate) 
go


