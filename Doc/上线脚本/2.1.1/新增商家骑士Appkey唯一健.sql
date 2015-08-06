----新增商家标识列
alter table dbo.business
add Appkey uniqueidentifier default newid() NOT NULL
go



----新增骑士标识列
alter table dbo.clienter
add Appkey uniqueidentifier default newid() NOT NULL
go



