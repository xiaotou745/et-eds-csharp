use superman
go
alter table dbo.clienter
add GradeType int default 1  not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'骑士等级1众包2全职3测试' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'clienter', @level2type=N'COLUMN',@level2name=N'GradeType'
GO
