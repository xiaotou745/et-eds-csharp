use superman
go

alter table dbo.clienter
add [HeadPhoto] [nvarchar](300) NULL
go
exec sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Í·ÏñµØÖ·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'clienter', @level2type=N'COLUMN',@level2name=N'HeadPhoto'
go