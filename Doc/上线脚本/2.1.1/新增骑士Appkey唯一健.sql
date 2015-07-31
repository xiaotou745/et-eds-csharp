
alter table dbo.clienter
add Appkey uniqueidentifier default newid() NOT NULL
go