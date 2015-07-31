alter table dbo.business
add Appkey uniqueidentifier default newid() NOT NULL
go