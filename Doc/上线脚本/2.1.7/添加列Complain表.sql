use superman
go
alter table dbo.Complain
add IsHandle int default 0  not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Complain', @level2type=N'COLUMN',@level2name=N'IsHandle'
GO

alter table dbo.Complain
add HandleOpinion nvarchar(200)
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Complain', @level2type=N'COLUMN',@level2name=N'HandleOpinion'
GO

alter table dbo.Complain
add Operator nvarchar(50)
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Complain', @level2type=N'COLUMN',@level2name=N'Operator'
GO

alter table dbo.Complain
add OperateTime datetime default getdate() not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Complain', @level2type=N'COLUMN',@level2name=N'OperateTime'
GO

