use superman
go
alter table dbo.clienter
add GradeType int default 1  not null
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʿ�ȼ�1�ڰ�2ȫְ3����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'clienter', @level2type=N'COLUMN',@level2name=N'GradeType'
GO
