USE [superman]
GO

--���������ʿID
CREATE INDEX [IX_order_clienterId] ON [dbo].[order]([clienterId] ASC) ;
go


--����������ʱ��
create index IX_order_actualDoneDate on [order](ActualDoneDate) 
go


