alter table dbo.[Order]
add FinishAll int default 0 NOT NULL
go

update [Order]  set FinishAll=1 
where ID in
(
select o.id from [OrderOther]   (nolock)
left join [Order] o (nolock) on o.Id=OrderOther.OrderId
where OrderOther.HadUploadCount>=OrderOther.NeedUploadCount
)