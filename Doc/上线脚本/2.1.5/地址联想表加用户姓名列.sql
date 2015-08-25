ALTER TABLE dbo.ReceviceAddress ADD UserName NVARCHAR(256) DEFAULT '' NOT NULL
GO 
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'”√ªß–’√˚' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceviceAddress', @level2type=N'COLUMN',@level2name=N'UserName'
go